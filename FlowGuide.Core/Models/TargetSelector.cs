using System.Drawing;

namespace FlowGuide.Core.Models;

/// <summary>
/// Селектор для идентификации UI элемента через UI Automation API.
/// </summary>
public class TargetSelector
{
    /// <summary>
    /// AutomationId элемента (уникальный идентификатор)
    /// </summary>
    public string? AutomationId { get; set; }

    /// <summary>
    /// Имя элемента (видимый текст)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Тип контрола (Button, TextBox, etc.)
    /// </summary>
    public string? ControlType { get; set; }

    /// <summary>
    /// Класс элемента
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    /// Путь в иерархии UI дерева (для точного поиска)
    /// Пример: "/Window/Pane/Button[2]"
    /// </summary>
    public string? HierarchyPath { get; set; }

    /// <summary>
    /// Границы элемента на экране (Left, Top, Width, Height)
    /// </summary>
    public Rectangle BoundingRectangle { get; set; }

    /// <summary>
    /// Флаг, указывающий, что это системный элемент Windows (Пуск, панель задач и т.д.)
    /// </summary>
    public bool IsSystemElement { get; set; }

    // Дополнительные свойства
    public string[]? NameVariants { get; set; } // альтернативные имена для разных локалей
    public NormalizedRect? NormalizedBoundingRect { get; set; } // нормализованные координаты (0‑1) относительно виртуального экрана
    public NormalizedRect? NormalizedRectInWindow { get; set; } // нормализованные координаты внутри окна (0‑1)
    public int RecordedDPI { get; set; } // DPI при записи
    public System.Drawing.Size RecordedScreenSize { get; set; } // размер экрана при записи
    public float? ScoreWeight { get; set; } // вес при ранжировании
}
