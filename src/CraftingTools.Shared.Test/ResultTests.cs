using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for the <see cref="Result"/> class.
/// </summary>
internal static class ResultTests
{
    /// <summary>
    /// Validates the behavior of the <c>Success</c> factory
    /// method.
    /// </summary>
    [Test]
    public static void Success_ValidatesBehavior()
    {
        var value = new object();
        var result = Result<object>.Success(value);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Unwrap(), Is.EqualTo(value));
    }

    /// <summary>
    /// Validates the behavior of the <c>Failure></c> factory
    /// method.
    /// </summary>
    [Test]
    public static void Failure_ValidatesBehavior()
    {
        var result = Result<object>.Failure();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Failure));
    }
}