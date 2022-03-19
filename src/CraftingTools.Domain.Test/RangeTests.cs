using NUnit.Framework;

namespace CraftingTools.Domain.Test;

/// <summary>
/// Tests for <see cref="Range"/>.
/// </summary>
internal static class RangeTests
{
    /// <summary>
    /// Validates the behavior of the factory method.
    /// </summary>
    /// <returns></returns>
    [TestCase(arg1: 1, arg2: 5, ExpectedResult = "1,5", TestName = "start_less_end")]
    [TestCase(arg1: 5, arg2: 1, ExpectedResult = "1,5", TestName = "start_greater_end")]
    [TestCase(arg1: 1, arg2: 1, ExpectedResult = "1,1", TestName = "start_equals_end")]
    public static string Ctor_ValidatesBehavior(int start, int end)
    {
        var range = new Range(start, end);
        return string.Join(separator: ",", range.Start, range.End);
    }
}
