# FlowGuide - Project Context

This document provides architectural context, rules, and structures for AI agents and developer tools working on the FlowGuide codebase.

## 1. Project Purpose

FlowGuide is a desktop application suite designed for Windows 10/11 to record and play interactive guides.
* **FlowGuide.Recorder**: Captures user clicks on desktop application controls via global mouse hooks, analyzes control properties using UI Automation, crops screenshots, and saves instructions into a `guide.json` format.
* **FlowGuide.Player**: Renders a transparent overlay that intercepts user inputs (except on the target control), draws a spotlight geometry cut-out around the target control, and displays instruction bubbles to guide users step-by-step.

## 2. Directory Structure

* [FlowGuide.Core](file:///d:/Develop/FlowGuide/FlowGuide.Core): Shared library containing the data structures, serialization logic, and core services.
  * `Models/`: Data structures representing guides, steps, selectors, and coordinates.
  * `Services/`:
    * `UIAutomationService`: Scans controls under the cursor and retrieves automation parameters.
    * `ScreenshotService`: Crops specific regions and takes full screenshots.
    * `StorageService`: Serializes and deserializes guide datasets to and from disk.
    * `MouseHook`: Encapsulates low-level Win32 global mouse hooks (`SetWindowsHookEx` with `WH_MOUSE_LL`).
* [FlowGuide.Recorder](file:///d:/Develop/FlowGuide/FlowGuide.Recorder): WPF desktop application. Sets up global mouse listeners, overlay window highlighting during recording, and a step editor.
* [FlowGuide.Player](file:///d:/Develop/FlowGuide/FlowGuide.Player): WPF desktop application. Implements a Click Guard system, a spotlight drawing overlay using `CombinedGeometry` with `Exclude`, and a draggable information bubble for instructions.
* [TestGuide](file:///d:/Develop/FlowGuide/TestGuide): MS Test or similar unit testing assembly verifying core models, serialization, and scorers.

## 3. Core Data Structures

### FlowGuide
Represents a saved tutorial guide. Contains `GuideId`, `Title`, `Description`, `Author`, `CreatedAt`, and a list of `Step` items. Includes version fields: `FormatVersion` (currently 2), `MinPlayerVersion`, and `CreatedWithVersion` to support backward compatibility.

### Step
Defines a single instruction. Contains `StepId`, `ActionType` (LeftClick, DoubleClick, RightClick, SystemAction, etc.), `TargetSelector`, `FallbackImagePath` (for the cropped screenshot), `InstructionText`, `SystemActionType` (if applicable), `MsSettingsCommand`, and a `ShowAutoExecuteButton` flag.

### TargetSelector
Stores parameters used to find the UI control during playback. Contains properties like `AutomationId`, `Name`, `NameVariants` (for localization), `ClassName`, `RecordedBoundingRect` (physical coordinates), `NormalizedBoundingRect` (0 to 1 relative to the virtual screen), `NormalizedRectInWindow` (0 to 1 relative to the host window), `RecordedDPI`, `RecordedScreenSize`, and an `Ancestors` hierarchy list.

## 4. Key Implementation Rules

* **UAC and Admin Privileges**: The Recorder and Player manifest files must target `requireAdministrator`. Low-level mouse hooks and cross-application UI Automation require elevated system context to capture and interact with other admin-level windows.
* **Low-level Mouse Hook Performance**: The `WH_MOUSE_LL` callback must process events in less than 15 milliseconds. Long-running or heavy operations must be offloaded to a background thread to prevent GUI lagging in target applications.
* **DPI Independence**: Physical bounds coordinates recorded during a session must be converted to normalized values relative to the screen dimensions and system DPI. When playing back, these coordinates must scale back to the active user's current resolution and scaling configurations.
* **Spotlight rendering**: The Player uses a `Path` control with a `CombinedGeometry` (combining a screen-sized rectangle and a cropped rounded rectangle for the target element using `GeometryCombineMode.Exclude`) to dim the screen while leaving the target control brightly illuminated.
