using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Automation;
using FlowGuide.Core.Models;

namespace FlowGuide.Core.Services;

/// <summary>
/// Calculates a match score between an AutomationElement and a TargetSelector.
/// The score is a float between 0 and 1, where higher means a better match.
/// </summary>
public class ElementMatchScorer
{
    /// <summary>
    /// Compute a weighted match score.
    /// </summary>
    public float CalculateMatchScore(AutomationElement element, TargetSelector selector)
    {
        if (element == null || selector == null) return 0f;
        float score = 0f;
        float weightSum = 0f;

        // AutomationId exact match (high weight)
        if (!string.IsNullOrEmpty(selector.AutomationId))
        {
            weightSum += 3f;
            if (string.Equals(element.Current.AutomationId, selector.AutomationId, StringComparison.OrdinalIgnoreCase))
                score += 3f;
        }

        // Name match (medium weight)
        if (!string.IsNullOrEmpty(selector.Name))
        {
            weightSum += 2f;
            if (string.Equals(element.Current.Name, selector.Name, StringComparison.OrdinalIgnoreCase))
                score += 2f;
            else if (selector.NameVariants != null)
            {
                foreach (var variant in selector.NameVariants)
                {
                    if (string.Equals(element.Current.Name, variant, StringComparison.OrdinalIgnoreCase))
                    {
                        score += 2f;
                        break;
                    }
                }
            }
        }

        // ControlType match (medium weight)
        if (!string.IsNullOrEmpty(selector.ControlType))
        {
            weightSum += 2f;
            var ctName = element.Current.ControlType.ProgrammaticName.Replace("ControlType.", "");
            if (string.Equals(ctName, selector.ControlType, StringComparison.OrdinalIgnoreCase))
                score += 2f;
        }

        // ClassName match (low weight)
        if (!string.IsNullOrEmpty(selector.ClassName))
        {
            weightSum += 1f;
            if (string.Equals(element.Current.ClassName, selector.ClassName, StringComparison.OrdinalIgnoreCase))
                score += 1f;
        }

        // Bounding rectangle proximity (low weight)
        if (selector.BoundingRectangle != Rectangle.Empty)
        {
            weightSum += 1f;
            var elemRect = element.Current.BoundingRectangle;
            var dx = Math.Abs(elemRect.Left - selector.BoundingRectangle.Left);
            var dy = Math.Abs(elemRect.Top - selector.BoundingRectangle.Top);
            // Simple proximity heuristic: closer => higher score
            var distance = Math.Sqrt(dx * dx + dy * dy);
            // Normalize assuming 200px tolerance
            var proximityScore = Math.Max(0, 1 - distance / 200.0);
            score += (float)proximityScore;
        }

        // Apply optional custom weight from selector
        if (selector.ScoreWeight.HasValue)
        {
            return (float)(score / weightSum) * selector.ScoreWeight.Value;
        }

        return weightSum > 0 ? (float)(score / weightSum) : 0f;
    }
}
