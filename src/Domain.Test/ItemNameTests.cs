using NUnit.Framework;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Tests for the <see cref="ItemName"/>.
/// </summary>
internal static class ItemNameTests
{
    /// <summary>
    /// Validates the <c>FromParameters</c> method.
    /// </summary>
    [TestCase(arguments: null, ExpectedResult = "Failure|Item name cannot be empty.", TestName = "null_value")]
    [TestCase(arguments: "", ExpectedResult = "Failure|Item name cannot be empty.", TestName = "empty_value")]
    [TestCase(arguments: "   ", ExpectedResult = "Failure|Item name cannot be empty.", TestName = "whitespace_value")]
    [TestCase(arguments: "name", ExpectedResult = "Success|name", TestName = "string_value")]
    public static string FromParameters_ValidatesBehavior(object? value)
    {
        var result = ItemName.FromParameters(value);
        return result.IsSuccess
            ? string.Join(separator: "|", result.Status, result.Unwrap().Value)
            : string.Join(separator: "|", result.Status, result.Error.Message);
    }
}
