namespace CraftingTools.Domain;

/// <summary>
/// Value object describing a range of integer values.
/// </summary>
public readonly struct Range
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <remarks>The start and end values are sorted so the start
    /// value is always less than or equal to the end value.</remarks>
    public Range(int start, int end)
    {
        if (start < end)
        {
            this.Start = start;
            this.End = end;
        }
        else
        {
            this.Start = end;
            this.End = start;
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Range(int start)
    {
        this.Start = start;
        this.End = start;
    }

    /// <summary>
    /// Start of the range.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// End of the range.
    /// </summary>
    public int End { get; }
}
