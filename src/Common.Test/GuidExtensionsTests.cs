using NUnit.Framework;
using SleepingBearSystems.Tools.Railway;

namespace SleepingBearSystems.CraftingTools.Common.Test;

/// <summary>
/// Tests for <see cref="GuidExtensions"/>.
/// </summary>
internal static class GuidExtensionsTests
{
    [Test]
    public static void ToResultNotEmpty_ValidatesBehavior()
    {
        // use case: empty guid
        {
            var result = Guid.Empty
                .ToResultNotEmpty(resultTag: "empty");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(expected: "Guid cannot be empty."));
            Assert.That(result.Tag, Is.EqualTo(expected: "empty"));
        }

        // use case: non-empty guid
        {
            var id = Guid.Parse(input: "E15E3E15-6317-467A-868B-5A83FC1794C6");
            var result = id
                .ToResultNotEmpty(resultTag: "non-empty");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(id));
            Assert.That(result.Tag, Is.EqualTo(expected: "non-empty"));
        }
    }
}
