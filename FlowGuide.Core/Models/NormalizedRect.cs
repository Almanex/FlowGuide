using System;

namespace FlowGuide.Core.Models;

/// <summary>
/// Represents a rectangle with coordinates normalized to the range 0‑1.
/// X and Y are the top‑left corner relative to the virtual screen.
/// Width and Height are also expressed as fractions of the screen size.
/// </summary>
public class NormalizedRect
{
    /// <summary>Normalized X coordinate (0‑1).</summary>
    public double X { get; set; }
    /// <summary>Normalized Y coordinate (0‑1).</summary>
    public double Y { get; set; }
    /// <summary>Normalized width (0‑1).</summary>
    public double Width { get; set; }
    /// <summary>Normalized height (0‑1).</summary>
    public double Height { get; set; }
}
