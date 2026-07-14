using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using FlowGuide.Core.Models;
using FlowGuide.Core.Services;
using FlowGuide.Core.Utilities;
using System;
using System.Threading.Tasks;

namespace FlowGuide.Recorder;

public partial class MainWindow : Window
{
    // Services
    private readonly UIAutomationService _uiAutomationService;
    private readonly ScreenshotService _screenshotService;
    private readonly StorageService _storageService;
    
    // Components
    private RecordingOverlay? _overlay;
    private ControlPanel? _controlPanel;
    private FlowGuide.Core.Utilities.MouseHook? _mouseHook;
    private DispatcherTimer? _trackingTimer;

    // State
    private bool _isRecording = false;
    private FlowGuide.Core.Models.FlowGuide? _currentGuide;
    private string? _currentGuideDir;
    private int _stepCounter = 0;

    // WinAPI
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    public MainWindow()
    {
        InitializeComponent();
        _uiAutomationService = new UIAutomationService();
        _screenshotService = new ScreenshotService();
        _storageService = new StorageService();
    }

    private void LaunchRecorderBtn_Click(object sender, RoutedEventArgs e)
    {
        // Инициализация нового гайда
        _currentGuide = new FlowGuide.Core.Models.FlowGuide
        {
            Title = GuideTitleInput.Text,
            Author = AuthorInput.Text
        };
        _stepCounter = 0;

        // Создание директории для сохранения
        string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlowGuide");
        _currentGuideDir = _storageService.CreateGuideDirectory(baseDir, _currentGuide.Title);

        Log($"Создана директория: {_currentGuideDir}");

        // Скрываем главное окно
        this.Hide();

        // Запускаем панель управления
        _controlPanel = new ControlPanel();
        _controlPanel.StartRecordingRequested += OnStartRecording;
        _controlPanel.PauseRecordingRequested += OnPauseRecording;
        _controlPanel.StopRecordingRequested += OnStopRecording;
        _controlPanel.Closed += (s, args) => OnStopRecording(s, args);
        _controlPanel.Show();

        // Создаем оверлей
        _overlay = new RecordingOverlay();
        _overlay.Show();

        // Инициализируем хук мыши
        _mouseHook = new FlowGuide.Core.Utilities.MouseHook();
        _mouseHook.LeftButtonUp += OnMouseClick;
        _mouseHook.Start();

        // Таймер для подсветки
        _trackingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
        _trackingTimer.Tick += TrackingTimer_Tick;
    }

    private async void EditGuideBtn_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Select guide.json",
            Filter = "Guide files (guide.json)|guide.json|All files (*.*)|*.*",
            InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlowGuide")
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                string guideFilePath = openFileDialog.FileName;
                string guideDir = Path.GetDirectoryName(guideFilePath)!;

                var guide = await _storageService.LoadGuideAsync(guideDir);

                if (guide != null)
                {
                    var editor = new StepEditorWindow(guide, guideDir);
                    if (editor.ShowDialog() == true)
                    {
                        await _storageService.SaveGuideAsync(guide, guideDir);
                        Log($"Гайд обновлен! Всего шагов: {guide.Steps.Count}");
                        MessageBox.Show("Гайд успешно обновлен!", "FlowGuide", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Ошибка при загрузке гайда: {ex.Message}");
                MessageBox.Show($"Ошибка: {ex.Message}", "FlowGuide", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void OnStartRecording(object? sender, EventArgs e)
    {
        _isRecording = true;
        _trackingTimer?.Start();
        Log("Запись началась...");
    }

    private void OnPauseRecording(object? sender, EventArgs e)
    {
        _isRecording = false;
        _trackingTimer?.Stop();
        _overlay?.HideHighlight();
        Log("Запись приостановлена.");
    }

    private bool _isStopping = false;

    private async void OnStopRecording(object? sender, EventArgs e)
    {
        if (_isStopping) return;
        _isStopping = true;

        try
        {
            _isRecording = false;
            _trackingTimer?.Stop();
            _mouseHook?.Stop();
            _overlay?.Close();
            
            if (_controlPanel != null)
            {
                _controlPanel.Close();
                _controlPanel = null;
            }

            if (_currentGuide != null && _currentGuideDir != null)
            {
                _overlay = null;

                var editor = new StepEditorWindow(_currentGuide, _currentGuideDir);
                if (editor.ShowDialog() == true)
                {
                    await _storageService.SaveGuideAsync(_currentGuide, _currentGuideDir);
                    Log($"Гайд сохранен! Всего шагов: {_currentGuide.Steps.Count}");
                }
                else
                {
                    Log("Сохранение отменено.");
                }
            }

            this.Show();
            StatusText.Text = "Запись завершена";
        }
        finally
        {
            _isStopping = false;
        }
    }

    private async void OnMouseClick(object? sender, System.Windows.Point point)
    {
        if (!_isRecording) return;

        try
        {
            var screenPoint = new System.Windows.Point((int)point.X, (int)point.Y);
            var element = _uiAutomationService.GetElementAtPoint(screenPoint);

            if (element == null) return;

            var selector = _uiAutomationService.ExtractSelector(element);
            var elementRect = selector.BoundingRectangle;
            
            string imgFileName = $"step_{_stepCounter + 1}.png";
            
            if (_overlay != null) _overlay.Visibility = Visibility.Hidden;
            await Task.Delay(50); 

            using (var bitmap = _screenshotService.CaptureRegion(elementRect))
            {
                _screenshotService.SaveImage(bitmap, _currentGuideDir!, imgFileName);
            }

            if (_overlay != null) _overlay.Visibility = Visibility.Visible;

            var step = new Step
            {
                StepId = Guid.NewGuid().ToString(),
                TargetSelector = selector,
                FallbackImagePath = imgFileName,
            };

            if (selector.IsSystemElement)
            {
                step.ActionType = ActionType.SystemAction;
                step.SystemActionType = DetermineSystemActionType(selector);
                step.InstructionText = GetSystemActionInstruction(step.SystemActionType.Value);
            }
            else
            {
                step.InstructionText = $"Нажмите {selector.Name ?? selector.ControlType}";
                step.ActionType = ActionType.LeftClick;
            }

            if (string.IsNullOrWhiteSpace(step.InstructionText) || 
                step.InstructionText.Contains("(null)") ||
                step.InstructionText == "Нажмите ")
            {
                Log($"Пропущен шаг с пустой инструкцией [Class: {selector.ClassName}]");
                return;
            }

            _currentGuide?.Steps.Add(step);
            _stepCounter++;

            Dispatcher.Invoke(() =>
            {
                StepsCountText.Text = _stepCounter.ToString();
                
                string logMessage = selector.IsSystemElement 
                    ? $"Записан шаг {_stepCounter}: СИСТЕМНОЕ ДЕЙСТВИЕ - {step.SystemActionType} [Class: {selector.ClassName}]"
                    : $"Записан шаг {_stepCounter}: Клик по {selector.Name} ({selector.ControlType}) [ID: {selector.AutomationId}] [Class: {selector.ClassName}]";
                
                Log(logMessage);
            });
        }
        catch (Exception ex)
        {
            Log($"Ошибка при записи шага: {ex.Message}");
        }
    }

    private void TrackingTimer_Tick(object? sender, EventArgs e)
    {
        if (!_isRecording) return;

        try
        {
            GetCursorPos(out POINT cursorPos);
            var screenPoint = new System.Windows.Point(cursorPos.X, cursorPos.Y);
            var element = _uiAutomationService.GetElementAtPoint(screenPoint);

            if (element != null)
            {
                var rect = element.Current.BoundingRectangle;
                var logicalRect = DpiHelper.PhysicalToLogical(new System.Drawing.Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height));
                
                double padding = 5.0;
                double manualOffsetX = 8.0;
                double manualOffsetY = 8.0;

                _overlay?.UpdateHighlight(
                    (int)(logicalRect.X - padding + manualOffsetX), 
                    (int)(logicalRect.Y - padding + manualOffsetY), 
                    (int)(logicalRect.Width + (padding * 2)), 
                    (int)(logicalRect.Height + (padding * 2))
                );
            }
            else
            {
                _overlay?.HideHighlight();
            }
        }
        catch { }
    }

    private SystemActionType DetermineSystemActionType(TargetSelector selector)
    {
        string className = selector.ClassName?.ToLower() ?? "";
        string name = selector.Name?.ToLower() ?? "";

        if (className.Contains("start") || name.Contains("пуск") || name.Contains("start"))
            return SystemActionType.OpenStartMenu;

        if (className.Contains("tray") || className.Contains("shell_traywnd"))
        {
            if (name.Contains("уведомления") || name.Contains("notification"))
                return SystemActionType.OpenNotifications;
            
            return SystemActionType.OpenStartMenu;
        }

        return SystemActionType.OpenStartMenu;
    }

    private string GetSystemActionInstruction(SystemActionType actionType)
    {
        return actionType switch
        {
            SystemActionType.OpenStartMenu => "Откройте меню Пуск",
            SystemActionType.OpenSettings => "Откройте Параметры Windows",
            SystemActionType.OpenCommandPrompt => "Откройте Командную строку",
            SystemActionType.OpenFileExplorer => "Откройте Проводник",
            SystemActionType.OpenTaskManager => "Откройте Диспетчер задач",
            SystemActionType.OpenRun => "Откройте окно 'Выполнить'",
            SystemActionType.OpenNotifications => "Откройте Центр уведомлений",
            SystemActionType.OpenActionCenter => "Откройте Центр действий",
            _ => "Выполните системное действие"
        };
    }

    private void Log(string message)
    {
        Dispatcher.Invoke(() =>
        {
            LogText.Text += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
            LogScrollViewer.ScrollToBottom();
        });
    }

    protected override void OnClosed(EventArgs e)
    {
        _trackingTimer?.Stop();
        _mouseHook?.Stop();
        _overlay?.Close();
        _controlPanel?.Close();
        base.OnClosed(e);
    }
}