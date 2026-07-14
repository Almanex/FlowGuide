using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace FlowGuide.Recorder;

public partial class RecordingOverlay : Window
{
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int GWL_EXSTYLE = -20;

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    public RecordingOverlay()
    {
        InitializeComponent();
        
        // Устанавливаем размеры окна на весь виртуальный экран
        this.Left = SystemParameters.VirtualScreenLeft;
        this.Top = SystemParameters.VirtualScreenTop;
        this.Width = SystemParameters.VirtualScreenWidth;
        this.Height = SystemParameters.VirtualScreenHeight;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        MakeTransparent();
    }

    private void MakeTransparent()
    {
        // Получаем хендл окна
        var hwnd = new WindowInteropHelper(this).Handle;
        
        // Получаем текущие стили
        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        
        // Добавляем стиль WS_EX_TRANSPARENT
        // Это делает окно "прозрачным" для мыши и hit-testing
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
    }

    public void UpdateHighlight(int x, int y, int width, int height)
    {
        // Проверка на валидность координат
        if (width <= 0 || height <= 0)
        {
            HighlightRect.Visibility = Visibility.Collapsed;
            return;
        }
        
        Canvas.SetLeft(HighlightRect, x);
        Canvas.SetTop(HighlightRect, y);
        HighlightRect.Width = width;
        HighlightRect.Height = height;
        
        HighlightRect.Visibility = Visibility.Visible;
    }

    public void HideHighlight()
    {
        HighlightRect.Visibility = Visibility.Collapsed;
    }
}
