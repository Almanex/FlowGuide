# FlowGuide

**Ein interaktives System zur Aufzeichnung und Wiedergabe von Schritt-für-Schritt-Anleitungen für Windows-Anwendungen.**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform: Windows](https://img.shields.io/badge/Platform-Windows%2010%20%2F%2011-blue.svg)](https://www.microsoft.com/windows)
[![Framework: .NET 8.0 WPF](https://img.shields.io/badge/Framework-.NET%208.0%20WPF-purple.svg)](https://dotnet.microsoft.com/en-us/apps/desktop/wpf)
[![Share](https://img.shields.io/twitter/url?style=social&url=https%3A%2F%2Fgithub.com%2FAlmanex%2FFlowGuide)](https://twitter.com/intent/tweet?text=Check%20out%20FlowGuide%20-%20an%20interactive%20guide%20recorder%20and%20player%20for%20Windows&url=https%3A%2F%2Fgithub.com%2FAlmanex%2FFlowGuide)

## 1. Übersicht

FlowGuide ist ein Desktop-Automatisierungs- und Schulungswerkzeug für Windows 10/11. Es ermöglicht Entwicklern, technischen Redakteuren und Support-Teams, interaktive Schritt-für-Schritt-Anleitungen (Guides) in jeder Standard-Windows-Anwendung aufzuzeichnen. Diese Anleitungen können dann als Overlay über dem Bildschirm des Benutzers abgespielt werden, um ihn durch komplexe Arbeitsabläufe zu führen, indem die Zielschaltflächen, Eingaben und Steuerelemente in Echtzeit hervorgehoben werden.

Durch die Nutzung der nativen Windows-UI-Automation-API und globaler Maus-Hooks bietet FlowGuide ein nahtloses Onboarding- und Schulungserlebnis direkt in der Zielanwendung, anstatt auf statische externe Handbücher oder Videoaufzeichnungen angewiesen zu sein.

---

## 2. Hauptfunktionen

* **Aktionsaufzeichnung**: Automatische Verfolgung von Klickereignissen über einen globalen, systemnahen Win32-Maus-Hook.
* **Intelligente UI-Elementauswahl**: Analyse der Elemente unter dem Cursor mittels UI Automation und Erfassung der relevanten Eigenschaften (AutomationId, Name, ClassName und Ahnenhierarchie).
* **Automatische Screenshot-Erstellung**: Speichern von zugeschnittenen Screenshot-Bereichen der hervorgehobenen Elemente mit einer kurzen Verzögerung nach dem Klick, um stabile Bilder zu gewährleisten.
* **Interaktives Spotlight-Overlay**: Dimmen des Bildschirms während der Wiedergabe mit einem transparenten Ausschnitt um das aktive Steuerelement.
* **Klickschutz (Click Guard)**: Blockiert Klicks des Benutzers außerhalb des aktiven hervorgehobenen Elements während der Wiedergabe, um die Einhaltung des Arbeitsablaufs sicherzustellen.
* **Automatische Ausführung von Systemaktionen**: Führt standardmäßige Windows-Befehle (wie das Öffnen des Startmenüs, der Einstellungsseiten, der Eingabeaufforderung oder des Datei-Explorers) automatisch im Namen des Benutzers aus.
* **DPI- und Auflösungsunabhängigkeit**: Aufzeichnung relativer Koordinaten (normalisiert bezüglich Bildschirmgröße und aktivem Fenster) zur korrekten Skalierung des Overlays bei allen Skalierungseinstellungen.
* **Schritt-Editor**: Überprüfen, Sortieren, Löschen und Verfeinern von aufgezeichneten Schritten, Anweisungstexten und Ausführungseinstellungen.

---

## 3. Technologie-Stack

| Schicht / Komponente | Technologie | Details / Zweck |
| --- | --- | --- |
| Plattform | .NET 8.0 | Ziel-Framework: `net8.0-windows` |
| UI-Framework | WPF | Native Windows-Desktop-Benutzeroberfläche und Grafikelemente |
| Automatisierungs-API | UI Automation | UI-Automation-Client zum Scannen und Auffinden von Steuerelementen |
| Hooking-Schnittstelle | Win32-API | Low-Level-Maus-Hook (`WH_MOUSE_LL`) zur Verfolgung von Klicks |
| Serialisierung | System.Text.Json | Speicherformat für Guide-Definitionen (`guide.json`) |

---

## 4. Erste Schritte

### Voraussetzungen

* Windows 10 oder Windows 11 (x64)
* Installiertes .NET 8.0 SDK oder .NET 8.0 Desktop Runtime

### Projekt erstellen

Klonen Sie das Repository und kompilieren Sie die Projektmappe über die .NET CLI:

```powershell
dotnet restore
dotnet build FlowGuide.Desktop.sln -c Release
```

### Anwendungen ausführen

#### FlowGuide Recorder (Aufnahme)

Das Recorder-Dienstprogramm erfasst Benutzeraktionen. Bitte beachten Sie, dass globale Hooks Administratorrechte erfordern, um Ereignisse von Anwendungen abzufangen, die mit erhöhten Rechten ausgeführt werden.

```powershell
dotnet run --project FlowGuide.Recorder/FlowGuide.Recorder.csproj
```

#### FlowGuide Player (Wiedergabe)

Das Player-Overlay führt den Benutzer durch die aufgezeichneten Schritt-für-Schritt-Anweisungen.

```powershell
dotnet run --project FlowGuide.Player/FlowGuide.Player.csproj
```

---

## 5. Tests ausführen

Unit-Tests für die Core-Modelle und die Serialisierungslogik befinden sich im Projekt `TestGuide`.

Führen Sie die Tests über die CLI aus:

```powershell
dotnet test
```

---

## 6. Bereitstellung

Führen Sie folgendes Skript aus, um portable, eigenständige ausführbare Dateien für den Recorder und den Player zu erstellen:

```powershell
powershell -ExecutionPolicy Bypass -File .\publish.ps1
```

Nach Abschluss der Kompilierung werden im Verzeichnis `.\Release` fertige ausführbare Dateien erstellt, die ohne Installation gestartet werden können.

---

## 7. Mitwirken

Weitere Informationen zur Projektarchitektur und zukünftigen Aufgaben finden Sie in den Dateien [docs/DEVELOPER_GUIDE.md](DEVELOPER_GUIDE.md) (in englischer Sprache) und [docs/TECHNICAL_REQUIREMENTS_RU.md](TECHNICAL_REQUIREMENTS_RU.md).

Wenn Sie zum Projekt beitragen möchten, erstellen Sie einen Fork des Repositories, nehmen Sie Änderungen vor und senden Sie einen Pull Request.

---

## 8. Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert – siehe die `LICENSE`-Datei für Details.
