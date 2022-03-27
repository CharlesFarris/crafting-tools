using NUnit.Framework;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Domain.Test;

/// <summary>
/// Tests for the <see cref="ItemName"/>.
/// </summary>
internal static class ItemNameTests
{
    /// <summary>
    /// Validates the <c>FromParameters</c> method.
    /// </summary>
    [TestCase(arg1: null, arg2: "id", ExpectedResult = "Failure|id|Item name cannot be empty.", TestName = "null_value")]
    [TestCase(arg1: "", arg2: "id", ExpectedResult = "Failure|id|Item name cannot be empty.", TestName = "empty_value")]
    [TestCase(arg1: "   ", arg2: "id", ExpectedResult = "Failure|id|Item name cannot be empty.", TestName = "whitespace_value")]
    [TestCase(arg1: "name", arg2: "id", ExpectedResult = "Success|id|name", TestName = "string_value")]
    public static string FromParameters_ValidatesBehavior(object? value, string? resultId)
    {
        var result = ItemName.FromParameters(value,resultId);
        return result.IsSuccess
            ? string.Join(separator: "|", result.Status, result.Id, result.Unwrap().Value)
            : string.Join(separator: "|", result.Status, result.Id, result.Error.Message);
    }

    /// <summary>
    /// Validates the <c>FromParameters</c> method length checking.
    /// </summary>
    [Test]
    public static void FromParameters_ValidatesLengthCheck()
    {
        var value = new string(c: 'X', count: 129);
        var result = ItemName.FromParameters(value, resultId: "long_name");
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
        Assert.That(result.Error.Message, Is.EqualTo(expected: "Item name cannot exceed 128 characters."));
        Assert.That(result.Id, Is.EqualTo(expected: "long_name"));
    }
}
