namespace FlowGuide.Core.Models;

/// <summary>
/// Интерактивный гайд - набор шагов для обучения пользователя
/// </summary>
public class FlowGuide
{
    /// <summary>
    /// Уникальный идентификатор гайда
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Название гайда
    /// Пример: "Создание таблицы в Excel"
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Автор гайда
    /// </summary>
    public string Author { get; set; } = Environment.UserName;

    /// <summary>
    /// Имя процесса целевого приложения
    /// Пример: "excel.exe", "WINWORD.EXE", "1cv8.exe"
    /// </summary>
    public string TargetProcessName { get; set; } = string.Empty;

    /// <summary>
    /// Список шагов гайда
    /// </summary>
    public List<Step> Steps { get; set; } = new();

    /// <summary>
    /// Дата создания гайда
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Версия формата .flow файла
    /// </summary>
    public int FormatVersion { get; set; } = 2;

    /// <summary>
    /// Минимальная версия Player, способная открыть этот гайд
    /// </summary>
    public string MinPlayerVersion { get; set; } = "1.1.0";

    /// <summary>
    /// Версия Recorder, создавшего гайд
    /// </summary>
    public string CreatedWithVersion { get; set; } = "1.1.0";
}
