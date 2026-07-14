using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FlowGuide.Core.Models;
using FlowGuide.Core.Services;
using FlowGuide.Core.Utilities;
using System.Runtime.InteropServices;

namespace FlowGuide.Player;

public partial class PlaybackOverlay : Window
{
    private readonly FlowGuide.Core.Models.FlowGuide _guide;
    private readonly string _guideDir;
    private int _currentStepIndex = 0;
    private Step? _currentStep;
    private readonly UIAutomationService _uiAutomationService;

    public PlaybackOverlay(FlowGuide.Core.Models.FlowGuide guide, string guideDir, bool overlayEnabled = true)
    {
        InitializeComponent();

        // Cover the whole virtual screen
        this.Left = SystemParameters.VirtualScreenLeft;
        this.Top = SystemParameters.VirtualScreenTop;
        this.Width = SystemParameters.VirtualScreenWidth;
        this.Height = SystemParameters.VirtualScreenHeight;

        _guide = guide;
        _guideDir = guideDir;
        _uiAutomationService = new UIAutomationService();
        _overlayVisible = overlayEnabled;

        Loaded += OnLoaded;
        KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            _isClosing = true;
            Close();
        }
        else if (e.Key == System.Windows.Input.Key.H)
        {
            _overlayVisible = !_overlayVisible;
            if (_overlayVisible)
            {
                SpotlightPath.Visibility = Visibility.Visible;
                HighlightBorder.Visibility = Visibility.Visible;
                InstructionText.Text = InstructionText.Text.Replace("\n\n💡 Press 'H' to show overlay", "");
            }
            else
            {
                SpotlightPath.Visibility = Visibility.Collapsed;
                HighlightBorder.Visibility = Visibility.Collapsed;
                if (!InstructionText.Text.Contains("💡 Press 'H' to show overlay"))
                    InstructionText.Text += "\n\n💡 Press 'H' to show overlay";
            }
        }
    }

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        _isClosing = true;
        Close();
    }

    private FlowGuide.Core.Utilities.MouseHook? _mouseHook;
    private Rect _currentTargetRect;
    private bool _overlayVisible = true;
    private bool _isDraggingBubble = false;
    private Point _bubbleDragStart;
    private bool _isClosing = false;

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ScreenGeometry.Rect = new Rect(0, 0, SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
        _mouseHook = new FlowGuide.Core.Utilities.MouseHook();
        _mouseHook.LeftButtonDown += OnGlobalMouseDown;
        _mouseHook.Start();

        ShowStep(_currentStepIndex);
    }

    private void OnGlobalMouseDown(object? sender, System.Windows.Point point)
    {
        var logicalPoint = DpiHelper.PhysicalToLogical(point);
        if (_currentTargetRect.Contains(logicalPoint))
        {
            Dispatcher.Invoke(() =>
            {
                NextStepButton.IsEnabled = true;
                NextStepButton.Content = "Next Step";

                if (InstructionText.Text.Contains("⚠️"))
                {
                    InstructionText.Text = InstructionText.Text.Replace("\n\n⚠️ Perform the action before clicking Next", "");
                    InstructionText.Text += "\n\n✅ Action performed!";
                }
            });
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        _mouseHook?.Stop();
        base.OnClosed(e);
    }

    private async void ShowStep(int index)
    {
        if (_isClosing) return;

        if (index < 0 || index >= _guide.Steps.Count)
        {
            MessageBox.Show("Guide completed!", "FlowGuide", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            return;
        }

        var step = _guide.Steps[index];

        // System actions
        if (step.ActionType == ActionType.SystemAction && step.SystemActionType.HasValue)
        {
            SetInstruction(step.InstructionText ?? "No instruction", index + 1, _guide.Steps.Count);
            ShowSystemActionStep(step);
            return;
        }

        // Normal UI element step
        SetInstruction(step.InstructionText ?? "No instruction", index + 1, _guide.Steps.Count);
        InstructionBubble.Visibility = Visibility.Visible;
        InstructionText.Text += "\n(Searching...)";

        if (step.TargetSelector != null)
        {
            await Task.Run(async () =>
            {
                int retries = 0;
                while (retries < 10 && !_isClosing)
                {
                    try
                    {
                        var element = _uiAutomationService.FindElement(step.TargetSelector);
                        if (element != null && !_isClosing)
                        {
                            _uiAutomationService.ScrollIntoView(element);
                            var rect = element.Current.BoundingRectangle;

                            await Dispatcher.InvokeAsync(() =>
                            {
                                if (_isClosing) return;
                                try
                                {
                                    var topLeft = this.PointFromScreen(new System.Windows.Point(rect.Left, rect.Top));
                                    var bottomRight = this.PointFromScreen(new System.Windows.Point(rect.Right, rect.Bottom));
                                    UpdateSpotlight(new Rect(topLeft, bottomRight));
                                    if (InstructionText.Text.EndsWith("\n(Searching...)"))
                                        InstructionText.Text = InstructionText.Text.Replace("\n(Searching...)", "");
                                }
                                catch (Exception ex)
                                {
                                    InstructionText.Text += $"\nError updating UI: {ex.Message}";
                                }
                            });
                            return;
                        }
                    }
                    catch { }

                    await Task.Delay(500);
                    retries++;
                }

                // Fallback to normalized coordinates if available
                if (!_isClosing && step.TargetSelector?.NormalizedBoundingRect != null)
                {
                    var norm = step.TargetSelector.NormalizedBoundingRect;
                    double screenW = SystemParameters.VirtualScreenWidth;
                    double screenH = SystemParameters.VirtualScreenHeight;

                    var fallbackRect = new System.Windows.Rect(
                        norm.X * screenW,
                        norm.Y * screenH,
                        norm.Width * screenW,
                        norm.Height * screenH);

                    await Dispatcher.InvokeAsync(() =>
                    {
                        UpdateSpotlight(fallbackRect);
                        if (InstructionText.Text.EndsWith("\n(Searching...)"))
                            InstructionText.Text = InstructionText.Text.Replace("\n(Searching...)", "");
                    });
                }
                else
                {
                    // No element and no normalized data – inform the user
                    await Dispatcher.InvokeAsync(() =>
                    {
                        if (InstructionText.Text.EndsWith("\n(Searching...)"))
                            InstructionText.Text = InstructionText.Text.Replace("\n(Searching...)", "");
                        InstructionText.Text += "\n\n⚠️ Element not found. Press 'H' to hide overlay and follow instructions manually.";
                        NextStepButton.Visibility = Visibility.Visible;
                        NextStepButton.IsEnabled = true;
                        NextStepButton.Content = "Skip Step";
                    });
                }
            });
        }
    }

    public void UpdateSpotlight(Rect targetRect)
    {
        _currentTargetRect = targetRect;

        SpotlightPath.Visibility = Visibility.Visible;
        NextStepButton.Visibility = Visibility.Visible;
        NextStepButton.IsEnabled = false;
        NextStepButton.Content = "Next Step (Perform action first)";

        if (!InstructionText.Text.Contains("⚠️"))
            InstructionText.Text += "\n\n⚠️ Perform the action before clicking Next";

        double strokeThickness = 3.0;
        double strokeOffset = strokeThickness / 2.0;
        double padding = 5.0;
        double manualOffsetX = 8.0;
        double manualOffsetY = 8.0;

        var paddedRect = new Rect(
            targetRect.Left - padding + manualOffsetX,
            targetRect.Top - padding + manualOffsetY,
            targetRect.Width + (padding * 2),
            targetRect.Height + (padding * 2));

        HoleGeometry.Rect = paddedRect;
        HighlightBorder.Width = paddedRect.Width;
        HighlightBorder.Height = paddedRect.Height;
        System.Windows.Controls.Canvas.SetLeft(HighlightBorder, paddedRect.Left - strokeOffset);
        System.Windows.Controls.Canvas.SetTop(HighlightBorder, paddedRect.Top - strokeOffset);
        HighlightBorder.Visibility = Visibility.Visible;

        InstructionBubble.UpdateLayout();
        double bubbleWidth = InstructionBubble.ActualWidth > 0 ? InstructionBubble.ActualWidth : 300;
        double bubbleHeight = InstructionBubble.ActualHeight > 0 ? InstructionBubble.ActualHeight : 150;

        double bubbleX = targetRect.Left;
        double bubbleY = targetRect.Bottom + 10;
        double screenWidth = SystemParameters.VirtualScreenWidth;
        double screenHeight = SystemParameters.VirtualScreenHeight;
        double margin = 10;

        if (bubbleY + bubbleHeight > screenHeight - margin)
        {
            bubbleY = targetRect.Top - bubbleHeight - 10;
            if (bubbleY < margin) bubbleY = margin;
        }

        if (bubbleX + bubbleWidth > screenWidth - margin) bubbleX = screenWidth - bubbleWidth - margin;
        if (bubbleX < margin) bubbleX = margin;

        System.Windows.Controls.Canvas.SetLeft(InstructionBubble, bubbleX);
        System.Windows.Controls.Canvas.SetTop(InstructionBubble, bubbleY);
        InstructionBubble.Visibility = Visibility.Visible;
    }

    public void SetInstruction(string text, int stepIndex, int totalSteps)
    {
        InstructionText.Text = text;
        StepCounterText.Text = $"Step {stepIndex} of {totalSteps}";
    }

    private void ShowSystemActionStep(Step step)
    {
        _currentStep = step;
        SpotlightPath.Visibility = Visibility.Collapsed;
        HighlightBorder.Visibility = Visibility.Collapsed;

        AutoExecuteButton.Visibility = step.ShowAutoExecuteButton ? Visibility.Visible : Visibility.Collapsed;

        var nextStepBtn = InstructionBubble.FindName("NextStepButton") as System.Windows.Controls.Button;
        if (nextStepBtn != null) nextStepBtn.Visibility = Visibility.Collapsed;

        string keyboardHint = GetKeyboardShortcut(step.SystemActionType!.Value);
        InstructionText.Text = step.InstructionText + $"\n\n💡 Сочетание клавиш: {keyboardHint}";

        double bubbleX = (SystemParameters.VirtualScreenWidth - 300) / 2;
        double bubbleY = (SystemParameters.VirtualScreenHeight - 200) / 2;
        System.Windows.Controls.Canvas.SetLeft(InstructionBubble, Math.Max(10, bubbleX));
        System.Windows.Controls.Canvas.SetTop(InstructionBubble, Math.Max(10, bubbleY));
        InstructionBubble.Visibility = Visibility.Visible;
    }

    private string GetKeyboardShortcut(SystemActionType actionType) => actionType switch
    {
        SystemActionType.OpenStartMenu => "Win",
        SystemActionType.OpenSettings => "Win + I",
        SystemActionType.OpenCommandPrompt => "Win + R → cmd",
        SystemActionType.OpenFileExplorer => "Win + E",
        SystemActionType.OpenTaskManager => "Ctrl + Shift + Esc",
        SystemActionType.OpenRun => "Win + R",
        SystemActionType.OpenNotifications => "Win + N",
        SystemActionType.OpenActionCenter => "Win + A",
        _ => "N/A"
    };

    private async void AutoExecuteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_currentStep?.SystemActionType == null) return;

        ExecuteSystemAction(_currentStep.SystemActionType.Value);
        await Task.Delay(1000);
        this.Topmost = false;
        this.Topmost = true;
        this.Activate();

        _currentStepIndex++;
        AutoExecuteButton.Visibility = Visibility.Collapsed;
        ShowStep(_currentStepIndex);
    }

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    private const int KEYEVENTF_KEYDOWN = 0x0000;
    private const int KEYEVENTF_KEYUP = 0x0002;
    private const byte VK_LWIN = 0x5B;

    private void ExecuteSystemAction(SystemActionType actionType)
    {
        if (!string.IsNullOrEmpty(_currentStep?.MsSettingsCommand))
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = _currentStep.MsSettingsCommand,
                UseShellExecute = true
            });
            return;
        }

        switch (actionType)
        {
            case SystemActionType.OpenStartMenu:
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                break;
            case SystemActionType.OpenSettings:
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "ms-settings:",
                    UseShellExecute = true
                });
                break;
            case SystemActionType.OpenFileExplorer:
                System.Diagnostics.Process.Start("explorer.exe");
                break;
            case SystemActionType.OpenRun:
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                break;
            default:
                MessageBox.Show($"Автоматическое выполнение для {actionType} пока не реализовано.", "FlowGuide");
                break;
        }
    }

    private void NextStepBtn_Click(object sender, RoutedEventArgs e)
    {
        _currentStepIndex++;
        AutoExecuteButton.Visibility = Visibility.Collapsed;
        ShowStep(_currentStepIndex);
    }

    // Drag support for instruction bubble
    private void InstructionBubble_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _isDraggingBubble = true;
        _bubbleDragStart = e.GetPosition(OverlayCanvas);
        InstructionBubble.CaptureMouse();
        e.Handled = true;
    }

    private void InstructionBubble_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (_isDraggingBubble && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            Point currentPos = e.GetPosition(OverlayCanvas);
            double offsetX = currentPos.X - _bubbleDragStart.X;
            double offsetY = currentPos.Y - _bubbleDragStart.Y;

            double currentLeft = System.Windows.Controls.Canvas.GetLeft(InstructionBubble);
            double currentTop = System.Windows.Controls.Canvas.GetTop(InstructionBubble);

            double newLeft = currentLeft + offsetX;
            double newTop = currentTop + offsetY;

            double margin = 10;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;
            double bubbleWidth = InstructionBubble.ActualWidth;
            double bubbleHeight = InstructionBubble.ActualHeight;

            newLeft = Math.Max(margin, Math.Min(newLeft, screenWidth - bubbleWidth - margin));
            newTop = Math.Max(margin, Math.Min(newTop, screenHeight - bubbleHeight - margin));

            System.Windows.Controls.Canvas.SetLeft(InstructionBubble, newLeft);
            System.Windows.Controls.Canvas.SetTop(InstructionBubble, newTop);

            _bubbleDragStart = currentPos;
            e.Handled = true;
        }
    }

    private void InstructionBubble_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (_isDraggingBubble)
        {
            _isDraggingBubble = false;
            InstructionBubble.ReleaseMouseCapture();
            e.Handled = true;
        }
    }
}