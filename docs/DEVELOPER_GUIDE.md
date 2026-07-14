# FlowGuide Desktop MVP - Developer Guide

## Prerequisites

* .NET 8.0 SDK
* Windows 10/11

## Project Structure

* **FlowGuide.Core**: Shared class library containing data models and base services.
* **FlowGuide.Recorder**: WPF application used to record user guide sequences.
* **FlowGuide.Player**: WPF application used to play overlay highlights and guides.

## Build and Run Commands

### 1. Recorder

Captures user click actions and serializes the step parameters to a JSON dataset.

**Build:**
```powershell
dotnet build FlowGuide.Recorder/FlowGuide.Recorder.csproj
```

**Run:**
```powershell
dotnet run --project FlowGuide.Recorder/FlowGuide.Recorder.csproj
```

### 2. Player

Renders transparent spotlights and bubbles to guide the user.

**Build:**
```powershell
dotnet build FlowGuide.Player/FlowGuide.Player.csproj
```

**Run:**
```powershell
dotnet run --project FlowGuide.Player/FlowGuide.Player.csproj
```

## Features and Improvements

* **Normalized Coordinates & DPI Fallback**: Saves target bounding boxes as normalized screen configurations (`NormalizedBoundingRect`), recording DPI (`RecordedDPI`), and screen dimensions (`RecordedScreenSize`). If UI Automation fails to locate a control at runtime, the Player maps coordinates to the current screen layout, scaling them appropriately to match the active scaling and DPI.
* **Guide Versioning**: Data structures in `FlowGuide.Core.Models.FlowGuide` include `FormatVersion`, `MinPlayerVersion`, and `CreatedWithVersion` to support backward compatibility. The serialization layer `StorageService.LoadGuideAsync` performs automated format migrations if older guide configurations are loaded.
* **Ranked Control Discovery**: If a control is not found by ID, the discovery engine uses a heuristic scorer (`ElementMatchScorer`) to evaluate UI elements by comparing property weights (AutomationId, ClassName, ControlType, Name, and boundary distance). The element with the highest match score exceeding `0.7` is selected.

## Technical Notes

* Guides are saved to the user's local directory at `Documents/FlowGuide`.
* The playback engine scans the target window in a background thread using UI Automation. If the control is missing, coordinates serve as a fallback.
* Press `Esc` or the exit button in the instruction bubble to exit the Player.
* Changing screen resolution or DPI scale values triggers an automated recalculation of target coordinates using the saved baseline parameters.
