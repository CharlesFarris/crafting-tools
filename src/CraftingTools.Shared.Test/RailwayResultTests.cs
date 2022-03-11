using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for the <see cref="Result"/> class.
/// </summary>
internal static class RailwayResultTests
{
    /// <summary>
    /// Validates the behavior of the <c>Success</c> factory
    /// method.
    /// </summary>
    [Test]
    public static void Success_ValidatesBehavior()
    {
        var value = new object();
        var result = RailwayResult<object>.Success(value);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
        Assert.That(result.Unwrap(), Is.EqualTo(value));
        Assert.That(result.Error, Is.EqualTo(Error.Empty));
    }

    /// <summary>
    /// Validates the behavior of the <c>Failure></c> factory
    /// method.
    /// </summary>
    [Test]
    public static void Failure_ValidatesBehavior()
    {
        var error = "error".ToError();
        var result = RailwayResult<object>.Failure(error);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Failure));
        Assert.That(result.Error, Is.EqualTo(error));
    }
}