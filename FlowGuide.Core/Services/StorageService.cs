using System.IO;
using System.Text.Json;
using FlowGuide.Core.Models;

namespace FlowGuide.Core.Services;

public class StorageService
{
    private const string DEFAULT_EXTENSION = ".flow";

    /// <summary>
    /// Создать новую директорию для гайда
    /// </summary>
    public string CreateGuideDirectory(string baseDir, string guideTitle)
    {
        // Очистка названия от недопустимых символов
        string safeTitle = string.Join("_", guideTitle.Split(Path.GetInvalidFileNameChars()));
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string dirName = $"{safeTitle}_{timestamp}";
        
        string fullPath = Path.Combine(baseDir, dirName);
        Directory.CreateDirectory(fullPath);
        
        return fullPath;
    }

    /// <summary>
    /// Сохранить гайд в JSON файл
    /// </summary>
    public async Task SaveGuideAsync(Models.FlowGuide guide, string directory)
    {
        string filePath = Path.Combine(directory, "guide.json");
        
        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true 
        };

        using FileStream createStream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(createStream, guide, options);
    }

    /// <summary>
    /// Загрузить гайд из директории
    /// </summary>
    public async Task<Models.FlowGuide?> LoadGuideAsync(string directory)
    {
        string filePath = Path.Combine(directory, "guide.json");
        
        if (!File.Exists(filePath))
            return null;

        using FileStream openStream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<Models.FlowGuide>(openStream);
    }
}
