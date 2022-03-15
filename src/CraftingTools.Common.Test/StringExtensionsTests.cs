using NUnit.Framework;

namespace CraftingTools.Common.Test;

/// <summary>
/// Tests for <see cref="StringExtensions"/> class.
/// </summary>
internal static class StringExtensionsTests
{
    /// <summary>
    /// Validates the behavior of the <c>ToSafeString()</c> method.
    /// </summary>
    [TestCase(arguments: null, ExpectedResult = "", TestName = "null_value")]
    [TestCase(arg: "", ExpectedResult = "", TestName = "empty_value")]
    [TestCase(arg: "abc", ExpectedResult = "abc", TestName = "valid_value")]
    public static string ToSafeString_ValidatesBehavior(string? value) => value.ToSafeString();
}
