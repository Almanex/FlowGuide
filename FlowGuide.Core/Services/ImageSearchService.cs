using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace FlowGuide.Core.Services;

public class ImageSearchService
{
    /// <summary>
    /// Finds the location of a template image within a source image (screen).
    /// </summary>
    /// <param name="screenBitmap">The full screen image.</param>
    /// <param name="templatePath">Path to the template image file.</param>
    /// <param name="threshold">Matching threshold (0.0 to 1.0). Default 0.8.</param>
    /// <returns>The bounding rectangle of the found area, or null if not found.</returns>
    public Rectangle? FindTemplateOnScreen(Bitmap screenBitmap, string templatePath, double threshold = 0.8)
    {
        if (!File.Exists(templatePath))
        {
            Console.WriteLine($"[OpenCV] Template file not found: {templatePath}");
            return null;
        }

        try
        {
            Console.WriteLine($"[OpenCV] Starting template matching. Template: {templatePath}, Threshold: {threshold}");
            
            using var screenMat = BitmapConverter.ToMat(screenBitmap);
            using var templateMat = new Mat(templatePath);

            Console.WriteLine($"[OpenCV] Screen size: {screenMat.Width}x{screenMat.Height}, Template size: {templateMat.Width}x{templateMat.Height}");

            // Convert to grayscale for better performance (optional, but recommended)
            using var screenGray = new Mat();
            using var templateGray = new Mat();
            
            Cv2.CvtColor(screenMat, screenGray, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(templateMat, templateGray, ColorConversionCodes.BGR2GRAY);

            // Result matrix
            int resultCols = screenGray.Cols - templateGray.Cols + 1;
            int resultRows = screenGray.Rows - templateGray.Rows + 1;
            
            if (resultCols <= 0 || resultRows <= 0)
            {
                Console.WriteLine($"[OpenCV] Template is larger than screen!");
                return null; // Template is larger than screen
            }

            using var result = new Mat(resultRows, resultCols, MatType.CV_32FC1);

            // Template Matching
            Cv2.MatchTemplate(screenGray, templateGray, result, TemplateMatchModes.CCoeffNormed);

            // Find best match
            Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out OpenCvSharp.Point minLoc, out OpenCvSharp.Point maxLoc);

            Console.WriteLine($"[OpenCV] Best match score: {maxVal:F4} (threshold: {threshold})");

            if (maxVal >= threshold)
            {
                Console.WriteLine($"[OpenCV] MATCH FOUND at ({maxLoc.X}, {maxLoc.Y})");
                return new Rectangle(maxLoc.X, maxLoc.Y, templateMat.Width, templateMat.Height);
            }
            else
            {
                Console.WriteLine($"[OpenCV] No match found. Best score {maxVal:F4} < threshold {threshold}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OpenCV] ERROR: {ex.Message}");
            Console.WriteLine($"[OpenCV] Stack trace: {ex.StackTrace}");
            Console.WriteLine($"OpenCV Error: {ex.Message}");
        }

        return null;
    }
}
