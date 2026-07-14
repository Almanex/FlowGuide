using System.Runtime.InteropServices;

namespace FlowGuide.Core.Utilities;

/// <summary>
/// Утилита для работы с DPI (масштабированием экрана)
/// Необходима для корректной работы на мониторах с scaling 125%, 150%, 200%
/// </summary>
public static class DpiHelper
{
    // Windows API для получения DPI
    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

    private const int LOGPIXELSX = 88;
    private const int LOGPIXELSY = 90;

    /// <summary>
    /// Стандартный DPI (96 = 100% масштаб)
    /// </summary>
    public const double StandardDpi = 96.0;

    /// <summary>
    /// Получить текущий DPI по горизонтали
    /// </summary>
    public static double GetDpiX()
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        int dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
        ReleaseDC(IntPtr.Zero, hdc);
        return dpiX;
    }

    /// <summary>
    /// Получить текущий DPI по вертикали
    /// </summary>
    public static double GetDpiY()
    {
        IntPtr hdc = GetDC(IntPtr.Zero);
        int dpiY = GetDeviceCaps(hdc, LOGPIXELSY);
        ReleaseDC(IntPtr.Zero, hdc);
        return dpiY;
    }

    /// <summary>
    /// Получить коэффициент масштабирования по горизонтали
    /// Например: 1.0 (100%), 1.25 (125%), 1.5 (150%)
    /// </summary>
    public static double GetScaleFactorX()
    {
        return GetDpiX() / StandardDpi;
    }

    /// <summary>
    /// Получить коэффициент масштабирования по вертикали
    /// </summary>
    public static double GetScaleFactorY()
    {
        return GetDpiY() / StandardDpi;
    }

    /// <summary>
    /// Конвертировать физические пиксели в логические (device-independent pixels)
    /// </summary>
    public static System.Windows.Point PhysicalToLogical(System.Windows.Point physicalPoint)
    {
        double scaleX = GetScaleFactorX();
        double scaleY = GetScaleFactorY();

        return new System.Windows.Point(
            physicalPoint.X / scaleX,
            physicalPoint.Y / scaleY
        );
    }

    /// <summary>
    /// Конвертировать логические пиксели в физические
    /// </summary>
    public static System.Windows.Point LogicalToPhysical(System.Windows.Point logicalPoint)
    {
        double scaleX = GetScaleFactorX();
        double scaleY = GetScaleFactorY();

        return new System.Windows.Point(
            logicalPoint.X * scaleX,
            logicalPoint.Y * scaleY
        );
    }

    /// <summary>
    /// Конвертировать физический Rectangle в логический
    /// </summary>
    public static System.Drawing.Rectangle PhysicalToLogical(System.Drawing.Rectangle physicalRect)
    {
        double scaleX = GetScaleFactorX();
        double scaleY = GetScaleFactorY();

        return new System.Drawing.Rectangle(
            (int)(physicalRect.X / scaleX),
            (int)(physicalRect.Y / scaleY),
            (int)(physicalRect.Width / scaleX),
            (int)(physicalRect.Height / scaleY)
        );
    }

    /// <summary>
    /// Конвертировать логический Rectangle в физический
    /// </summary>
    public static System.Drawing.Rectangle LogicalToPhysical(System.Drawing.Rectangle logicalRect)
    {
        double scaleX = GetScaleFactorX();
        double scaleY = GetScaleFactorY();

        return new System.Drawing.Rectangle(
            (int)(logicalRect.X * scaleX),
            (int)(logicalRect.Y * scaleY),
            (int)(logicalRect.Width * scaleX),
            (int)(logicalRect.Height * scaleY)
        );
    }

    /// <summary>
    /// Получить информацию о текущем DPI (для отладки)
    /// </summary>
    public static string GetDpiInfo()
    {
        return $"DPI: {GetDpiX()}x{GetDpiY()} | Scale: {GetScaleFactorX():P0}x{GetScaleFactorY():P0}";
    }
}
