using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FlowGuide.Core.Utilities;

namespace FlowGuide.Core.Services;

public class ScreenshotService
{
    /// <summary>
    /// Сделать скриншот всего экрана
    /// </summary>
    public Bitmap CaptureScreen()
    {
        // Получаем границы виртуального экрана (все мониторы)
        int left = (int)System.Windows.SystemParameters.VirtualScreenLeft;
        int top = (int)System.Windows.SystemParameters.VirtualScreenTop;
        int width = (int)System.Windows.SystemParameters.VirtualScreenWidth;
        int height = (int)System.Windows.SystemParameters.VirtualScreenHeight;

        // Корректируем на DPI
        var physicalRect = DpiHelper.LogicalToPhysical(new Rectangle(left, top, width, height));

        Bitmap bmp = new Bitmap(physicalRect.Width, physicalRect.Height);
        
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(physicalRect.Left, physicalRect.Top, 0, 0, bmp.Size);
        }

        return bmp;
    }

    /// <summary>
    /// Сделать скриншот конкретной области (элемента) с отступом
    /// </summary>
    public Bitmap CaptureRegion(Rectangle region, int padding = 20)
    {
        // Добавляем отступ
        int x = Math.Max(0, region.X - padding);
        int y = Math.Max(0, region.Y - padding);
        int w = region.Width + (padding * 2);
        int h = region.Height + (padding * 2);

        // Проверяем границы экрана
        // TODO: Добавить проверку границ, чтобы не вылететь за пределы

        Bitmap bmp = new Bitmap(w, h);
        
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(x, y, 0, 0, bmp.Size);
        }

        return bmp;
    }

    /// <summary>
    /// Сохранить Bitmap в файл
    /// </summary>
    public string SaveImage(Bitmap image, string directory, string filename)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, filename);
        image.Save(path, ImageFormat.Png);
        return path;
    }
}
