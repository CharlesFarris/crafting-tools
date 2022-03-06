namespace CraftingTools.Shared;

/// <summary>
/// Extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Checks if the supplied string is null and returns the
    /// empty string if so.
    /// </summary>
    public static string ToSafeString(this string? value) => value ?? string.Empty;
}