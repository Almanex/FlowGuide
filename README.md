[README_RU](docs/README_RU.md) | [README_DE](docs/README_DE.md) | [README_EN](README.md) | [GUIDE_RU](docs/GUIDE_RU.md) | [GUIDE_DE](docs/GUIDE_DE.md) | [GUIDE_EN](docs/GUIDE_EN.md)
# FlowGuide

**An interactive step-by-step guide recording and playback system for Windows applications.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform: Windows](https://img.shields.io/badge/Platform-Windows%2010%20%2F%2011-blue.svg)](https://www.microsoft.com/windows)
[![Framework: .NET 8.0 WPF](https://img.shields.io/badge/Framework-.NET%208.0%20WPF-purple.svg)](https://dotnet.microsoft.com/en-us/apps/desktop/wpf)
[![Share](https://img.shields.io/twitter/url?style=social&url=https%3A%2F%2Fgithub.com%2FAlmanex%2FFlowGuide)](https://twitter.com/intent/tweet?text=Check%20out%20FlowGuide%20-%20an%20interactive%20guide%20recorder%20and%20player%20for%20Windows&url=https%3A%2F%2Fgithub.com%2FAlmanex%2FFlowGuide)

## 1. Overview

FlowGuide is a desktop automation and training utility designed for Windows 10/11. It allows developers, technical writers, and support teams to record interactive step-by-step guides inside any standard Windows application. These guides can then be played back as an overlay on the user's screen, guiding them through complex workflows by highlighting the target buttons, inputs, and controls in real time.

By utilizing the native Windows UI Automation API and global mouse hooks, FlowGuide provides a seamless onboarding and training experience directly within the target application, rather than relying on static external manuals or video recordings.

---

## 2. Key Features

* **Control Recording**: Automatically tracks click events using a low-level global Win32 mouse hook.
* **Smart UI Element Selection**: Inspects elements under the cursor using UI Automation and captures relevant properties (AutomationId, Name, ClassName, and Ancestor hierarchy).
* **Automatic Screenshot Capture**: Saves cropped screenshot regions of highlighted elements with a short delay post-click to ensure stable visuals.
* **Interactive Spotlight Overlay**: Dims the screen during playback and cuts out a clear "spotlight" region around the active control.
* **Click Guard**: Prevents users from clicking outside the active highlighted element during playback, ensuring they follow the exact workflow sequence.
* **Auto-Execution of System Actions**: Automatically executes standard Windows commands (such as opening the Start menu, Settings pages, Command Prompt, or File Explorer) on behalf of the user.
* **DPI and Resolution Independence**: Records normalized coordinates relative to the screen size and the active window to scale overlays correctly across varying display scaling settings.
* **Step Editing**: Review, reorganize, delete, and refine recorded steps, instruction texts, and execution settings.

---

## 3. Tech Stack

| Layer / Component | Technology | Details / Purpose |
| --- | --- | --- |
| Platform | .NET 8.0 | Target framework: `net8.0-windows` |
| UI Framework | WPF | Native Windows desktop UI and drawing layers |
| Automation API | UI Automation | UI Automation Client for scanning and finding window controls |
| Hooking Interface | Win32 API | Low-level mouse hook (`WH_MOUSE_LL`) for tracking clicks |
| Serialization | System.Text.Json | Storage format for guide definitions (`guide.json`) |

---

## 4. Getting Started

### Prerequisites

* Windows 10 or Windows 11 (x64)
* .NET 8.0 SDK or .NET 8.0 Desktop Runtime installed

### Building the Project

Clone the repository and compile the solution using the .NET CLI:

```powershell
dotnet restore
dotnet build FlowGuide.Desktop.sln -c Release
```

### Running the Applications

#### FlowGuide Recorder

The Recorder utility captures user actions. Note that global hooks require administrator permissions to interact with other elevated applications.

```powershell
dotnet run --project FlowGuide.Recorder/FlowGuide.Recorder.csproj
```

#### FlowGuide Player

The Player overlay guides the user through the recorded step-by-step instructions.

```powershell
dotnet run --project FlowGuide.Player/FlowGuide.Player.csproj
```

---

## 5. Running the Tests

Unit tests for Core models and serialization are located in the `TestGuide` project.

Run tests via the CLI:

```powershell
dotnet test
```

---

## 6. Deployment

To build portable, self-contained executables for both the Recorder and Player:

```powershell
powershell -ExecutionPolicy Bypass -File .\publish.ps1
```

This compiles both applications to the `.\Release` folder as single-file executables that do not require installation.

---

## 7. Contributing

Please refer to [docs/DEVELOPER_GUIDE.md](docs/DEVELOPER_GUIDE.md) and [docs/TECHNICAL_REQUIREMENTS_RU.md](docs/TECHNICAL_REQUIREMENTS_RU.md) for more details on project architecture and future work.

If you wish to contribute, please fork the repository, make your changes, and submit a pull request.

---

## 8. License

This project is licensed under the MIT License - see the `LICENSE` file for details.
