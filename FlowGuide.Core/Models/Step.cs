namespace FlowGuide.Core.Models;

/// <summary>
/// Один шаг в интерактивном гайде
/// </summary>
public class Step
{
    /// <summary>
    /// Уникальный идентификатор шага
    /// </summary>
    public string StepId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Тип действия (клик, ввод текста, нажатие клавиши)
    /// </summary>
    public ActionType ActionType { get; set; }

    /// <summary>
    /// Селектор целевого элемента
    /// </summary>
    public TargetSelector? TargetSelector { get; set; }

    /// <summary>
    /// Путь к скриншоту элемента (для fallback поиска через OpenCV)
    /// </summary>
    public string? FallbackImagePath { get; set; }

    /// <summary>
    /// Текст инструкции для пользователя
    /// Пример: "Нажмите кнопку Сохранить"
    /// </summary>
    public string? InstructionText { get; set; }

    /// <summary>
    /// Путь к скриншоту всего экрана (для отладки и редактора)
    /// </summary>
    public string? ScreenContextPath { get; set; }

    /// <summary>
    /// Текст для ввода (если ActionType = TypeText)
    /// </summary>
    public string? TextToType { get; set; }

    /// <summary>
    /// Клавиша для нажатия (если ActionType = KeyPress)
    /// Пример: "Enter", "Escape", "Tab"
    /// </summary>
    public string? KeyToPress { get; set; }

    /// <summary>
    /// Тип системного действия (для SystemAction)
    /// </summary>
    public SystemActionType? SystemActionType { get; set; }

    /// <summary>
    /// MS-Settings команда для прямого открытия параметров Windows
    /// Например: "ms-settings:display", "ms-settings:network"
    /// </summary>
    public string? MsSettingsCommand { get; set; }

    /// <summary>
    /// Показывать ли кнопку "Выполнить автоматически" в Player
    /// </summary>
    public bool ShowAutoExecuteButton { get; set; } = true;
}

/// <summary>
/// Типы действий пользователя
/// </summary>
public enum ActionType
{
    LeftClick,
    RightClick,
    DoubleClick,
    TypeText,
    KeyPress,
    SystemAction  // Специальное действие для системных элементов
}

public enum SystemActionType
{
    OpenStartMenu,      // Win
    OpenSettings,       // Win+I или ms-settings:
    OpenCommandPrompt,  // Win+R → cmd
    OpenFileExplorer,   // Win+E
    OpenTaskManager,    // Ctrl+Shift+Esc
    OpenRun,            // Win+R
    OpenNotifications,  // Win+N
    OpenActionCenter    // Win+A
}
