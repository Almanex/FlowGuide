using System.Windows;
using System.Windows.Input;

namespace FlowGuide.Recorder;

public partial class ControlPanel : Window
{
    public event EventHandler? StartRecordingRequested;
    public event EventHandler? PauseRecordingRequested;
    public event EventHandler? StopRecordingRequested;

    private bool _isRecording = false;

    public ControlPanel()
    {
        InitializeComponent();
        
        // Позиционирование в правом нижнем углу по умолчанию
        this.Left = SystemParameters.WorkArea.Width - this.Width - 20;
        this.Top = SystemParameters.WorkArea.Height - this.Height - 20;
    }

    private void RecordBtn_Click(object sender, RoutedEventArgs e)
    {
        _isRecording = !_isRecording;
        UpdateVisualState();
        
        if (_isRecording)
            StartRecordingRequested?.Invoke(this, EventArgs.Empty);
        else
            PauseRecordingRequested?.Invoke(this, EventArgs.Empty);
    }

    private void PauseBtn_Click(object sender, RoutedEventArgs e)
    {
        _isRecording = false;
        UpdateVisualState();
        PauseRecordingRequested?.Invoke(this, EventArgs.Empty);
    }

    private void StopBtn_Click(object sender, RoutedEventArgs e)
    {
        _isRecording = false;
        UpdateVisualState();
        StopRecordingRequested?.Invoke(this, EventArgs.Empty);
    }

    private void DragHandle_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    private void UpdateVisualState()
    {
        if (_isRecording)
        {
            // Состояние записи: показываем Паузу, скрываем Старт (или меняем иконку)
            RecordBtn.Visibility = Visibility.Collapsed;
            PauseBtn.Visibility = Visibility.Visible;
        }
        else
        {
            // Состояние паузы/стоп: показываем Старт
            RecordBtn.Visibility = Visibility.Visible;
            PauseBtn.Visibility = Visibility.Collapsed;
        }
    }
}
