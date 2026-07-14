# FlowGuide - User Guide for Creating Guides

## Table of Contents

1. [Introduction](#introduction)
2. [Preparing for Recording](#preparing-for-recording)
3. [Creating a New Guide](#creating-a-new-guide)
4. [Editing a Guide](#editing-a-guide)
5. [Tips and Best Practices](#tips-and-best-practices)
6. [Troubleshooting](#troubleshooting)

---

## Introduction

**FlowGuide** helps you create interactive step-by-step instructions for any Windows application. Your guides will walk users through complex processes, highlighting the necessary buttons and fields on their screen.

### What you can do:
* Record user actions in any application
* Automatically generate textual instructions
* Edit and refine recorded steps
* Share completed guides with colleagues

### What you need:
* Windows 10 or 11
* Administrator privileges (for the first run)
* The target application for which you are creating instructions

---

## Preparing for Recording

### Step 1: Launch FlowGuide Recorder

1. Open the `Release\Recorder\` folder
2. Run **FlowGuide.Recorder.exe** as administrator
   * Right-click the file -> "Run as administrator"
   * Or: run normally once and grant the requested permissions

> [Tip]: Create a desktop shortcut for quick access.

### Step 2: Prepare the Target Application

**Before starting the recording:**

1. Open the application you are creating the guide for (Word, 1C, etc.)
2. Set the initial state (for example, open the required starting window)
3. Close unnecessary windows and notifications
4. Disable auto-save and auto-updates if possible

> [Warning]: The Recorder will track all your clicks, so prepare your workspace in advance!

---

## Creating a New Guide

### Phase 1: Configuring the Guide

1. In the main window of FlowGuide Recorder, fill in the fields:

   **Guide Title** (required)
   ```
   Example: "Creating a New Document in Word"
   ```
   
   **Author** (required, defaults to your system username)
   ```
   Example: "John Doe"
   ```
   
   **Description** (optional)
   ```
   Example: "Step-by-step instructions for creating a new document 
   from a template and saving it to the designated folder"
   ```

2. Click the **"Record New Guide"** button.

### Phase 2: Starting the Recording

**After clicking the button:**

1. The main Recorder window will minimize.
2. A floating **Control Panel** will appear containing these buttons:
   * **[Start Recording]** - start tracking
   * **[Pause]** - pause recording
   * **[Stop]** - stop and finish recording

3. Click **"Start Recording"**.

**What happens next:**
* A **red border** will highlight elements under your mouse cursor.
* This indicates that recording is active.
* The Control Panel will remain visible; you can drag it to any convenient position on your screen.

> [Tip]: Position the Control Panel so it does not block the target controls but remains accessible.

### Phase 3: Recording Actions

**Now perform the actions you want to capture:**

#### Example: Creating a Document in Word

1. **Hover the cursor** over the "File" tab.
   * You will see a red frame surrounding the tab.
   
2. **Click** on "File".
   * The step is recorded!
   * The system automatically generates the instruction: "Click the 'File' tab"
   * A cropped screenshot of the button is captured.

3. **Click** on "New".
   * Another step is recorded!
   * Instruction: "Click the 'New' button"

4. Continue performing the necessary workflow steps...

**Important Guidelines:**

* Click accurately on the elements; the system relies on exact control properties.
* Pause for 1-2 seconds between clicks to allow the system to capture a clean screenshot.
* Take your time; slow and precise recording produces better results.
* Do NOT click on blank spaces, as this generates unnecessary steps.

### Phase 4: Using Pause

**If you need to take a break or perform setup actions:**

1. Click **[Pause]** on the Control Panel.
2. Recording is suspended.
3. You can:
   * Prepare the next application state.
   * Review the currently recorded steps.
   * Take a short break.
4. Click **"Start Recording"** to resume.

> [Tip] When to use pause:
> * You need to type complex data manually without recording.
> * You are waiting for an application or data to load.
> * You want to check what has been recorded so far.

### Phase 5: Stopping the Recording

**When you have completed the sequence:**

1. Click **[Stop]** on the Control Panel.
2. The Control Panel closes.
3. The **Step Editor** window opens automatically.

---

## Editing a Guide

### Step Editor Window

After stopping the recording, you will see a list of all captured steps:

```
Step Editor - 8 steps recorded

[1] Click the "File" tab
Type: LeftClick
[Edit] [Delete]

[2] Click the "New" button
Type: LeftClick
[Edit] [Delete]

[3] Enter text in the "Name" field
Type: LeftClick
[Edit] [Delete]

...
```

### Editing an Individual Step

**To modify a step:**

1. Click the **[Edit]** button next to the target step.
2. The step editing dialog opens.

**What you can customize:**

#### 1. Instruction Text
```
Before: "Click the 'New' button"

Modified:
"Click the 'New' button to create a new document from the template"
```

> [Tip]: Keep instructions clear, precise, and descriptive!

#### 2. Action Type

**Normal Click**
* Used for standard clicks.
* The user must perform the click action manually during playback.

**System Action**
* Used for native Windows UI elements.
* Can be executed automatically by the Player.

**When to use System Actions:**

| Action | Select Type |
|---|---|
| Open Start Menu | `OpenStartMenu` |
| Open Windows Settings | `OpenSettings` |
| Open File Explorer | `OpenFileExplorer` |
| Open Task Manager | `OpenTaskManager` |

#### 3. MS-Settings Command (for System Actions)

If you selected **System Action: OpenSettings**, you can specify a direct settings category command:

```
ms-settings:display          - Display settings
ms-settings:network          - Network & Internet
ms-settings:privacy          - Privacy settings
ms-settings:windowsupdate    - Windows Update
```

#### 4. Show Auto-Execute Button

* Enabled (default): The user will see an "Auto Execute" button during playback.
* Disabled: The user must perform the action manually.

### Deleting Steps

**If you recorded an accidental action:**

1. Locate the step in the list.
2. Click **[Delete]**.
3. Confirm deletion.

> [Warning]: Deleting a step cannot be undone! Proceed with caution.

### Reordering Steps

**Currently, reordering steps directly within the UI is not supported.**

**Workaround:**
1. Close the editor without saving.
2. Re-record the workflow in the correct sequence.

> [Tip]: Plan your actions thoroughly before recording!

### Saving the Guide

**When editing is complete:**

1. Click the green **"Save Guide"** button in the bottom right corner.
2. The guide dataset is saved to:
   ```
   C:\Users\YourName\Documents\FlowGuide\GuideName\
   ```
3. A confirmation dialog appears: "Guide successfully saved!"

**Saved Files:**
* `guide.json` - configuration and step description metadata.
* `step_1.png`, `step_2.png`, ... - cropped control screenshots.

### Cancelling

**If you want to discard the recording:**

1. Click the grey **"Cancel"** button.
2. The guide is NOT saved.
3. All recorded steps are discarded.

---

## Editing an Existing Guide

### Loading a Guide for Editing

1. In the main window of FlowGuide Recorder, click **"Edit Existing Guide"**.
2. Select the **guide.json** file from the guide's directory:
   ```
   C:\Users\YourName\Documents\FlowGuide\GuideName\guide.json
   ```
3. The Step Editor will open with the loaded steps.

### Available Operations

* Edit instruction text.
* Change action type and settings.
* Delete steps.
* Note: Adding new steps is not supported (requires a new recording).

### Saving Changes

1. Click **"Save Guide"**.
2. The changes will overwrite the existing files.
3. The previous version is permanently replaced.

> [Tip]: Back up the guide folder before performing edits!

---

## Tips and Best Practices

### Creating High-Quality Guides

#### 1. Planning

**Draft a plan before recording:**

```
Example workflow plan for "Saving a Document in Word":

1. Open File menu
2. Select Save As
3. Click Browse to select path
4. Input name into file name box
5. Click Save button
```

* Write the plan down.
* Check if the steps are logical.
* Verify no steps are missing.

#### 2. Workspace Preparation

**Ensure ideal conditions for recording:**

* Close all unrelated windows.
* Clean up your desktop and hide personal information.
* Turn on "Do Not Disturb" to disable notifications.
* Prepare test data in advance.
* Confirm that the target application runs stably.

#### 3. Recording Process

**Gold rules for recording:**

1. **Slow and Precise**
   * Pause for 2-3 seconds between clicks.
   * Allow the system enough time to process and crop screenshots.

2. **Click the Center of Controls**
   * Improves UI Automation element discovery.
   * Avoid clicking near borders.

3. **Use Standard Navigation Paths**
   * Choose the most intuitive route.
   * Example: "File -> New" is preferred over "Ctrl + N".

4. **One Action per Step**
   * Keep steps simple.
   * It is better to have many simple steps than a few convoluted ones.

#### 4. Formatting Texts

**Write clear instructions:**

Poor:
```
"Click the button"
```

Good:
```
"Click the 'New' button in the upper-left corner of the screen"
```

Better:
```
"Click the green 'New' button in the upper-left corner to create a new document from a template"
```

**Include:**
* Detailed descriptions.
* Goals ("to do X").
* Directions ("in the corner").
* Visual details ("green button").

### Special Scenarios

#### Form Fields

**When filling multiple inputs:**

1. Record a click on each text box individually.
2. Specify what text to enter in the instruction text:
   ```
   "Enter the document name in the 'File Name' field.
   Example: 'Report_2026'"
   ```

#### Dropdowns

1. Click the dropdown control.
2. Click the target item.
3. Define the value clearly:
   ```
   "Select 'Microsoft Word' from the program dropdown list"
   ```

#### Dialog Windows

**Issue:** Pop-up windows may open at varying screen coordinates.

**Solution:**
1. Do not rely on fixed screen locations.
2. Reference buttons by their visible names:
   ```
   Good: "Click the 'OK' button in the confirmation dialog"
   Poor: "Click in the bottom right corner"
   ```

#### Waiting Times

**If a step takes time to complete:**

1. Pause the recording.
2. Wait for the operation to complete.
3. Resume recording.
4. Add wait details to the instructions:
   ```
   "Wait until the document finishes loading (the progress bar disappears)"
   ```

### Optimal Guide Length

**Recommendations:**

* **Short Guides (3-7 steps)**: For basic tasks.
  * Example: "How to save a document"
  
* **Medium Guides (8-15 steps)**: Standard guidelines.
  * Example: "Creating and sending an email in Outlook"
  
* **Long Guides (16-30 steps)**: Use selectively.
  * Example: "Generating a balance report in accounting software"
  
* **Very Long Guides (over 30 steps)**: Split into multiple tutorials.
  * Example: "Part 1: Setup", "Part 2: Generation"

---

## Troubleshooting

### Red Highlight Frame Does Not Appear

**Causes and Solutions:**

1. **Recording is not active**
   * Click "Start Recording" on the Control Panel.

2. **Cursor is hovering over desktop background**
   * Move the mouse cursor over a valid control.

3. **Application is not supported**
   * Check if recording works with other windows (Word, File Explorer).
   * Note that some apps with custom drawing engines do not expose UI Automation properties.

### Step Captured Incorrectly

**What to do:**

1. Do NOT stop recording.
2. Press **[Pause]**.
3. Plan the correct action.
4. Click **"Start Recording"** to resume.
5. Record the correct step.
6. Delete the incorrect step later in the Step Editor.

### Screenshot Not Saved

**Causes:**
* Clicks were performed too rapidly.
* The system did not have enough time to capture the frame.

**Solution:**
* Pause for 2-3 seconds between mouse actions.
* Avoid rushing.

### "No screenshot available for this step"

This is expected if:
* The step is a System Action.
* The application window closed or changed too quickly.

The guide will still function correctly; screenshots are not mandatory for playback.

### Saving Fails

**Verify:**

1. You specified a guide title.
2. You specified an author name.
3. Your user account has write permissions to the Documents folder.
4. There is sufficient storage space on the drive.

### Step Editor Does Not Open

**Solution:**

1. Close FlowGuide Recorder.
2. Re-open it as administrator.
3. Try loading the recording via "Edit Existing Guide".

### Application Hangs

**Steps:**

1. Open Task Manager (Ctrl + Shift + Esc).
2. Select "FlowGuide.Recorder.exe".
3. Click "End Task".
4. Restart the program.

> [Warning]: Unsaved recordings will be lost!

---

## Guide Examples

### Example 1: Basic Guide (5 steps)

**"How to Save a Document in Word"**

1. Click the "File" tab in the top-left corner of the screen.
2. Choose "Save As" from the sidebar menu.
3. Click "Browse" to choose the storage path.
4. Input the name of the file into the "File Name" box.
5. Click the "Save" button.

### Example 2: Medium Guide (12 steps)

**"Creating a Calendar Event in Windows"**

1. Press Win (opens Start Menu) - System Action.
2. Search and click the "Calendar" application.
3. Click the "New Event" button in the upper-left corner.
4. Enter the event title in the "Event Name" text box.
5. Click the "Start Date" picker.
6. Select the date on the calendar grid.
7. Click the "Start Time" field.
8. Choose the start time.
9. Click the "End Date" picker.
10. Select the end date.
11. Add a brief description in the details field.
12. Click the "Save" button.

### Example 3: System Actions

**"Opening Display Settings in Windows"**

1. Press Win (opens Start Menu) - System Action: OpenStartMenu.
2. Open Windows Settings - System Action: OpenSettings, ms-settings:display.
3. (The display settings page will open automatically).

---

## Quality Checklist

Before saving your guide, verify the following details:

### Preparation
- [ ] Workflow plan is drafted.
- [ ] Active workspace is cleared.
- [ ] Inputs and test parameters are ready.
- [ ] Notifications are muted.

### Recording
- [ ] 2-3 second pauses were taken between actions.
- [ ] No redundant mouse clicks were captured.
- [ ] All critical steps are included.
- [ ] Sequence is logical.

### Editing
- [ ] Instructions are descriptive and detailed.
- [ ] Action goals are stated ("to do X").
- [ ] Text is free of spelling errors.
- [ ] Redundant steps are deleted.
- [ ] System Actions are configured correctly (if present).

### Verification
- [ ] Title is descriptive.
- [ ] Description is filled out.
- [ ] Author name is set.
- [ ] Dataset is saved.

### Testing
- [ ] Guide was loaded and verified in FlowGuide Player.
- [ ] All spotlight areas align correctly.
- [ ] Instructions are clear to third-party users.

---

## Contact and Support

**Need help?**

1. Review the [Troubleshooting](#troubleshooting) section.
2. Contact your system administrator.
3. Open an issue on the project repository.

---

## Conclusion

Congratulations! You are now ready to create interactive tutorials with FlowGuide!

**Remember:**
* Good preparation makes a good guide.
* Precise and slow recording ensures stable screenshots.
* Editing improves instructional clarity.
* Always test your guide before sharing.

**Good luck!**
