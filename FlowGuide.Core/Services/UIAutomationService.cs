using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using FlowGuide.Core.Models;

namespace FlowGuide.Core.Services;

/// <summary>
/// Service for interacting with UI Automation API.
/// Provides element detection, selector extraction, and ranked search.
/// </summary>
public class UIAutomationService
{
    #region Helper Methods

    private bool IsInteractive(AutomationElement e)
    {
        try
        {
            var type = e.Current.ControlType;
            return type == ControlType.Button ||
                   type == ControlType.CheckBox ||
                   type == ControlType.RadioButton ||
                   type == ControlType.Slider ||
                   type == ControlType.ComboBox ||
                   type == ControlType.MenuItem ||
                   type == ControlType.TabItem ||
                   type == ControlType.Hyperlink ||
                   type == ControlType.ListItem ||
                   type == ControlType.TreeItem ||
                   type == ControlType.Thumb ||
                   type == ControlType.SplitButton;
        }
        catch { return false; }
    }

    private bool HasIdentification(AutomationElement e)
    {
        try { return !string.IsNullOrEmpty(e.Current.AutomationId) || !string.IsNullOrEmpty(e.Current.Name); }
        catch { return false; }
    }

    private bool ContainsPoint(AutomationElement e, System.Windows.Point point)
    {
        try
        {
            var rect = e.Current.BoundingRectangle;
            return point.X >= rect.Left && point.X <= rect.Right &&
                   point.Y >= rect.Top && point.Y <= rect.Bottom;
        }
        catch { return false; }
    }

    private int GetSystemDPI()
    {
        using var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
        return (int)graphics.DpiX;
    }

    #endregion

    #region Public API

    /// <summary>
    /// Get UI element at a screen point (physical pixels).
    /// </summary>
    public AutomationElement? GetElementAtPoint(System.Windows.Point screenPoint)
    {
        try
        {
            var element = AutomationElement.FromPoint(screenPoint);
            if (element == null) return null;

            // 1. Interactive parent within reasonable distance
            var walker = TreeWalker.ControlViewWalker;
            var current = element;
            if (IsInteractive(current) && ContainsPoint(current, screenPoint))
                return current;

            var parent = walker.GetParent(current);
            int depth = 0;
            while (parent != null && parent != AutomationElement.RootElement && depth < 3)
            {
                if (IsInteractive(parent) && ContainsPoint(parent, screenPoint))
                    return parent;
                parent = walker.GetParent(parent);
                depth++;
            }

            // 2. Identification on the element itself
            if (HasIdentification(element)) return element;

            // 3. Walk up to first identified parent (max depth 5)
            parent = walker.GetParent(element);
            depth = 0;
            while (parent != null && parent != AutomationElement.RootElement && depth < 5)
            {
                if (HasIdentification(parent) && ContainsPoint(parent, screenPoint))
                    return parent;
                parent = walker.GetParent(parent);
                depth++;
            }

            // 4. Fallback to original element
            return element;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting element at point: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extract a TargetSelector from an AutomationElement, capturing normalized coordinates and DPI.
    /// </summary>
    public TargetSelector ExtractSelector(AutomationElement element)
    {
        var selector = new TargetSelector();
        try
        {
            selector.AutomationId = element.Current.AutomationId;
            selector.Name = element.Current.Name;
            selector.NameVariants = new[] { selector.Name };
            selector.ControlType = element.Current.ControlType.ProgrammaticName;
            selector.ClassName = element.Current.ClassName;

            // Physical bounds
            var rect = element.Current.BoundingRectangle;
            selector.BoundingRectangle = new Rectangle(
                (int)rect.Left,
                (int)rect.Top,
                (int)rect.Width,
                (int)rect.Height);

            // Normalized bounds relative to virtual screen
            double screenW = SystemParameters.VirtualScreenWidth;
            double screenH = SystemParameters.VirtualScreenHeight;
            selector.NormalizedBoundingRect = new NormalizedRect
            {
                X = rect.Left / screenW,
                Y = rect.Top / screenH,
                Width = rect.Width / screenW,
                Height = rect.Height / screenH
            };

            // Placeholder for window‑relative normalized rect (filled later by Player)
            selector.NormalizedRectInWindow = new NormalizedRect();

            selector.RecordedDPI = GetSystemDPI();
            selector.RecordedScreenSize = new System.Drawing.Size((int)screenW, (int)screenH);

            selector.HierarchyPath = BuildHierarchyPath(element);
            selector.IsSystemElement = IsSystemElement(selector.ClassName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting selector: {ex.Message}");
        }
        return selector;
    }

    private bool IsSystemElement(string? className)
    {
        if (string.IsNullOrEmpty(className)) return false;
        string[] systemClasses = {
            "Shell_TrayWnd",
            "Shell_SecondaryTrayWnd",
            "ReBarWindow32",
            "MSTaskSwWClass",
            "Start",
            "TrayNotifyWnd",
            "SysPager",
            "ToolbarWindow32",
            "TrayClockWClass",
            "TrayButton",
            "Windows.UI.Input.InputSite.WindowClass"
        };
        return systemClasses.Any(sc => className.Contains(sc, StringComparison.OrdinalIgnoreCase));
    }

    private string BuildHierarchyPath(AutomationElement element)
    {
        var path = new List<string>();
        var current = element;
        try
        {
            while (current != null && current != AutomationElement.RootElement)
            {
                var ct = current.Current.ControlType.ProgrammaticName.Replace("ControlType.", "");
                var name = current.Current.Name;
                var parent = TreeWalker.RawViewWalker.GetParent(current);
                int index = 0;
                if (parent != null)
                {
                    var sibling = TreeWalker.RawViewWalker.GetFirstChild(parent);
                    while (sibling != null && !Automation.Compare(sibling, current))
                    {
                        if (sibling.Current.ControlType == current.Current.ControlType)
                            index++;
                        sibling = TreeWalker.RawViewWalker.GetNextSibling(sibling);
                    }
                }
                var segment = string.IsNullOrEmpty(name) ? $"{ct}[{index}]" : $"{ct}[{name}]";
                path.Insert(0, segment);
                current = TreeWalker.RawViewWalker.GetParent(current);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error building hierarchy path: {ex.Message}");
        }
        return "/" + string.Join("/", path);
    }

    /// <summary>
    /// Find element by selector, using prioritized exact matches and a ranked fallback.
    /// </summary>
    public AutomationElement? FindElement(TargetSelector selector, AutomationElement? root = null)
    {
        root ??= AutomationElement.RootElement;
        try
        {
            // Exact match priority chain (unchanged)
            if (!string.IsNullOrEmpty(selector.AutomationId) &&
                !string.IsNullOrEmpty(selector.ControlType) &&
                !string.IsNullOrEmpty(selector.Name))
            {
                var cond = new AndCondition(
                    new PropertyCondition(AutomationElement.AutomationIdProperty, selector.AutomationId),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.LookupById(ConvertControlTypeName(selector.ControlType))),
                    new PropertyCondition(AutomationElement.NameProperty, selector.Name));
                var el = root.FindFirst(TreeScope.Descendants, cond);
                if (el != null) return el;
            }

            if (!string.IsNullOrEmpty(selector.AutomationId) && !string.IsNullOrEmpty(selector.ControlType))
            {
                var cond = new AndCondition(
                    new PropertyCondition(AutomationElement.AutomationIdProperty, selector.AutomationId),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.LookupById(ConvertControlTypeName(selector.ControlType))));
                var el = root.FindFirst(TreeScope.Descendants, cond);
                if (el != null) return el;
            }

            if (!string.IsNullOrEmpty(selector.AutomationId))
            {
                var cond = new PropertyCondition(AutomationElement.AutomationIdProperty, selector.AutomationId);
                var el = root.FindFirst(TreeScope.Descendants, cond);
                if (el != null) return el;
            }

            if (!string.IsNullOrEmpty(selector.Name) && !string.IsNullOrEmpty(selector.ControlType))
            {
                var cond = new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, selector.Name),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.LookupById(ConvertControlTypeName(selector.ControlType))));
                var el = root.FindFirst(TreeScope.Descendants, cond);
                if (el != null) return el;
            }

            if (!string.IsNullOrEmpty(selector.Name))
            {
                var cond = new PropertyCondition(AutomationElement.NameProperty, selector.Name);
                var el = root.FindFirst(TreeScope.Descendants, cond);
                if (el != null) return el;
            }

            // Ranked search fallback
            var scorer = new ElementMatchScorer();
            var candidates = FindAllCandidates(selector, root);
            if (candidates.Any())
            {
                var best = candidates
                    .Select(e => new { Element = e, Score = scorer.CalculateMatchScore(e, selector) })
                    .OrderByDescending(x => x.Score)
                    .First();
                if (best.Score >= 0.7f) // threshold
                    return best.Element;
            }

            // Final fallback – null (Player may use coordinates)
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finding element: {ex.Message}");
            return null;
        }
    }

    private List<AutomationElement> FindAllCandidates(TargetSelector selector, AutomationElement root)
    {
        var list = new List<AutomationElement>();
        if (!string.IsNullOrEmpty(selector.AutomationId))
        {
            var cond = new PropertyCondition(AutomationElement.AutomationIdProperty, selector.AutomationId);
            var el = root.FindFirst(TreeScope.Descendants, cond);
            if (el != null) list.Add(el);
        }
        if (!string.IsNullOrEmpty(selector.Name))
        {
            var cond = new PropertyCondition(AutomationElement.NameProperty, selector.Name);
            var el = root.FindFirst(TreeScope.Descendants, cond);
            if (el != null) list.Add(el);
        }
        if (!string.IsNullOrEmpty(selector.ClassName))
        {
            var cond = new PropertyCondition(AutomationElement.ClassNameProperty, selector.ClassName);
            var el = root.FindFirst(TreeScope.Descendants, cond);
            if (el != null) list.Add(el);
        }
        return list.Distinct().ToList();
    }

    private int ConvertControlTypeName(string controlTypeName)
    {
        controlTypeName = controlTypeName.Replace("ControlType.", "");
        return controlTypeName switch
        {
            "Button" => 50000,
            "Calendar" => 50001,
            "CheckBox" => 50002,
            "ComboBox" => 50003,
            "Edit" => 50004,
            "Hyperlink" => 50005,
            "Image" => 50006,
            "ListItem" => 50007,
            "List" => 50008,
            "Menu" => 50009,
            "MenuBar" => 50010,
            "MenuItem" => 50011,
            "ProgressBar" => 50012,
            "RadioButton" => 50013,
            "ScrollBar" => 50014,
            "Slider" => 50015,
            "Spinner" => 50016,
            "StatusBar" => 50017,
            "Tab" => 50018,
            "TabItem" => 50019,
            "Text" => 50020,
            "ToolBar" => 50021,
            "ToolTip" => 50022,
            "Tree" => 50023,
            "TreeItem" => 50024,
            "Custom" => 50025,
            "Group" => 50026,
            "Thumb" => 50027,
            "DataGrid" => 50028,
            "DataItem" => 50029,
            "Document" => 50030,
            "SplitButton" => 50031,
            "Window" => 50032,
            "Pane" => 50033,
            "Header" => 50034,
            "HeaderItem" => 50035,
            "Table" => 50036,
            "TitleBar" => 50037,
            "Separator" => 50038,
            _ => 50025 // default custom
        };
    }

    /// <summary>
    /// Scroll element into view if possible.
    /// </summary>
    public void ScrollIntoView(AutomationElement element)
    {
        try
        {
            if (element.TryGetCurrentPattern(ScrollItemPattern.Pattern, out var pattern))
            {
                ((ScrollItemPattern)pattern).ScrollIntoView();
                Thread.Sleep(100);
            }
        }
        catch { }
    }

    /// <summary>
    /// Determine if element is off‑screen.
    /// </summary>
    public bool IsElementOffscreen(AutomationElement element)
    {
        try { return element.Current.IsOffscreen; }
        catch { return true; }
    }

    /// <summary>
    /// Get bounding rectangle, optionally applying DPI scaling.
    /// </summary>
    public Rectangle GetBoundingRectangle(AutomationElement element)
    {
        try
        {
            var rect = element.Current.BoundingRectangle;
            return new Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
        }
        catch { return Rectangle.Empty; }
    }

    #endregion
}
