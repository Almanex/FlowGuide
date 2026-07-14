# FlowGuide Desktop MVP - Руководство разработчика

## Требования

* .NET 8.0 SDK
* Windows 10/11

## Структура проекта

* **FlowGuide.Core**: Общая библиотека (модели, сервисы).
* **FlowGuide.Recorder**: Приложение для записи гайдов.
* **FlowGuide.Player**: Приложение для воспроизведения гайдов (оверлей).

## Команды сборки и запуска

### 1. Рекордер (Recorder)

Записывает действия пользователя и сохраняет их в JSON.

**Сборка:**
```powershell
dotnet build FlowGuide.Recorder/FlowGuide.Recorder.csproj
```

**Запуск:**
```powershell
dotnet run --project FlowGuide.Recorder/FlowGuide.Recorder.csproj
```

### 2. Плеер (Player)

Воспроизводит записанные гайды поверх интерфейса.

**Сборка:**
```powershell
dotnet build FlowGuide.Player/FlowGuide.Player.csproj
```

**Запуск:**
```powershell
dotnet run --project FlowGuide.Player/FlowGuide.Player.csproj
```

## Новые возможности (после улучшений)

* **Нормализованные координаты + DPI fallback**: При записи сохраняются `NormalizedBoundingRect`, `RecordedDPI` и `RecordedScreenSize`. При воспроизведении, если элемент не найден через UI Automation, плеер использует эти данные для визуального выделения элемента, корректно масштабируя их под текущий DPI и разрешение экрана.
* **Версионирование формата гида**: В `FlowGuide.Core.Models.FlowGuide` добавлены поля `FormatVersion`, `MinPlayerVersion` и `CreatedWithVersion`. `StorageService.LoadGuideAsync` проверяет совместимость версии и может выполнять миграцию старых форматов.
* **Ранжированный поиск элементов**: При неудачном точном поиске используется `ElementMatchScorer`, который оценивает кандидатов по нескольким признакам (AutomationId, Name, ControlType, ClassName, близость к ожидаемому прямоугольнику). Выбирается элемент с наивысшим весом (порог 0.7).

## Полезные заметки

* Гайды сохраняются в папку `Documents/FlowGuide`.
* Плеер использует UI Automation для поиска элементов. Если элемент не найден, поиск повторяется в фоновом режиме, а затем применяется fallback-логика.
* Для экстренного выхода из плеера нажмите `Esc` или кнопку закрытия в подсказке.
* При изменении DPI или разрешения экрана плеер автоматически пересчитывает координаты, используя сохранённые параметры записи.
