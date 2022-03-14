using System;
using System.Collections.Immutable;
using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for <see cref="RailwayResultExtensions"/> class.
/// </summary>
internal static class RailwayResultExtensionsTests
{
    /// <summary>
    /// Validates the <c>ToResult()</c> methods wraps the value
    /// in a success result.
    /// </summary>
    [Test]
    public static void ToResult_ValidatesBehavior()
    {
        var value = new object();
        var result = value.ToResult();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
        Assert.That(result.Unwrap(), Is.EqualTo(value));
    }

    /// <summary>
    /// Validates the <c>ToResultIsNotNullOrEmpty</c> returns back the correct
    /// failure result if the string is null or empty.  
    /// </summary>
    [TestCase(arguments: null, ExpectedResult = "Failure|message", TestName = "null_value")]
    [TestCase(arg: "", ExpectedResult = "Failure|message", TestName = "empty_value")]
    [TestCase(arg: " ", ExpectedResult = "Success| ", TestName = "whitespace_value")]
    [TestCase(arg: "abc", ExpectedResult = "Success|abc", TestName = "valid_value")]
    public static string ToResultIsNotNullOrEmpty_ValidatesBehavior(string value)
    {
        var result = value.ToResultIsNotNullOrEmpty(failureMessage: "message");
        return result.IsSuccess
            ? string.Join(separator: "|", result.Status, result.Unwrap().ToSafeString())
            : string.Join(separator: "|", result.Status, result.Error.Message);
    }

    /// <summary>
    /// Validates the <c>ToResultIsNotNullOrEmpty</c> returns back the correct
    /// failure result if the string is null, empty, or whitespace.  
    /// </summary>
    [TestCase(arguments: null, ExpectedResult = "Failure|message", TestName = "null_value")]
    [TestCase(arg: "", ExpectedResult = "Failure|message", TestName = "empty_value")]
    [TestCase(arg: " ", ExpectedResult = "Failure|message", TestName = "whitespace_value")]
    [TestCase(arg: "abc", ExpectedResult = "Success|abc", TestName = "valid_value")]
    public static string ToResultIsNotNullOrWhitespace_ValidatesBehavior(string value)
    {
        var result = value.ToResultIsNotNullOrWhitespace(failureMessage: "message");
        return result.IsSuccess
            ? string.Join(separator: "|", result.Status, result.Unwrap().ToSafeString())
            : string.Join(separator: "|", result.Status, result.Error.Message);
    }

    /// <summary>
    /// Validates the <c>Check()</c> method returns back the correct result
    /// based on result and predicate parameters.
    /// </summary>
    [Test]
    public static void Check_ValidatesBehavior()
    {
        // use case: null result parameter
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = default(RailwayResult<object>)!.Check(_ => false, failureMessage: "message");
            });
            Assert.That(ex!.ParamName, Is.EqualTo(expected: "inResult"));
        }

        // use case: null result parameter
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new object().ToResult().Check(default!, failureMessage: "message");
            });
            Assert.That(ex!.ParamName, Is.EqualTo(expected: "predicate"));
        }

        // use case: failure result
        {
            var inResult = RailwayResult<object>.Failure(Error.Empty, id: "in_id");
            var outResult = inResult.Check(_ => true, failureMessage: "message", id: "id");
            Assert.That(outResult, Is.EqualTo(inResult));
        }

        // use case: success case
        {
            var inResult = RailwayResult<object>.Success(new object(), id: "in_id");
            var outResult = inResult.Check(_ => true, failureMessage: "message", id: "id");
            Assert.That(outResult, Is.EqualTo(inResult));
        }

        // use case: failure case
        {
            var inResult = RailwayResult<object>.Success(new object(), id: "in_id");
            var outResult = inResult.ToResult().Check(_ => false, failureMessage: "message", id: "id");
            Assert.That(outResult, Is.Not.Null);
            Assert.That(outResult.Status, Is.EqualTo(RailwayResultStatus.Failure));
            Assert.That(outResult.Error.Message, Is.EqualTo(expected: "message"));
            Assert.That(outResult.Id, Is.EqualTo(expected: "id"));
        }

        // use case: default ID
        {
            var inResult = RailwayResult<object>.Success(new object(), id: "in_id");
            var outResult = inResult.Check(_ => true, failureMessage: "message");
            Assert.That(outResult, Is.EqualTo(inResult));
            Assert.That(outResult.Id, Is.EqualTo(expected: "in_id"));
        }
    }

    /// <summary>
    /// Validates the <c>ToResultIsNotNull</c> method return back the failure result
    /// if the value is null.
    /// </summary>
    [Test]
    public static void ToResultIsNotNull_ValidatesBehavior()
    {
        // use case: null value
        {
            var result = default(object).ToResultIsNotNull(failureMessage: "message", id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(expected: "message"));
            Assert.That(result.Id, Is.EqualTo(expected: "id"));
        }

        // use case: non-null value
        {
            var value = new object();
            var result = value.ToResultIsNotNull(failureMessage: "message", id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(value));
            Assert.That(result.Id, Is.EqualTo(expected: "id"));
        }
    }

    /// <summary>
    /// Validates the behavior of the <c>UnwrapOrAddToFailuresImmutable</c> method.
    /// </summary>
    [Test]
    public static void UnwrapOrAddToFailuresImmutable_ValidatesBehavior()
    {
        // use case: null result
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var failures = ImmutableList<RailwayResultBase>.Empty;
                var _ = default(RailwayResult<object>)!.UnwrapOrAddToFailuresImmutable(ref failures);
            });
            Assert.That(ex!.ParamName, Is.EqualTo(expected: "result"));
        }

        // use case: null failures collection
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var failures = default(ImmutableList<RailwayResultBase>);
                var _ = RailwayResult<object>.Success(new object()).UnwrapOrAddToFailuresImmutable(ref failures!);
            });
            Assert.That(ex!.ParamName, Is.EqualTo(expected: "failures"));
        }

        // use case: success result
        {
            var value = new object();
            var failures = ImmutableList<RailwayResultBase>.Empty;
            var unwrapped = value.ToResult().UnwrapOrAddToFailuresImmutable(ref failures);
            Assert.That(unwrapped, Is.EqualTo(value));
        }

        // use case: failure result
        {
            var failures = ImmutableList<RailwayResultBase>.Empty;
            var inResult = RailwayResult<object>.Failure(Error.Empty);
            var unwrapped = inResult.UnwrapOrAddToFailuresImmutable(ref failures);
            Assert.That(unwrapped, Is.Null);
            Assert.That(failures.Count, Is.EqualTo(expected: 1));
            Assert.That(failures[index: 0], Is.EqualTo(inResult));
        }
    }

    /// <summary>
    /// Validates the behavior of the <c>As()</c> method for decimal values.
    /// </summary>
    [Test]
    public static void As_Decimal_ValidatesBehavior()
    {
        // use case: null value
        {
            var result = default(object).As<decimal>(id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(expected: 0M));
            Assert.That(result.Id, Is.EqualTo(expected: "id"));
        }

        // use case: valid string value
        {
            var result = "12.34".As<decimal>(id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(expected: 12.34M));
            Assert.That(result.Id, Is.EqualTo(expected: "id"));
        }

        // use case: invalid string value
        {
            var result = "abc".As<decimal>(id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(expected: "Unable to parse decimal."));
        }

        // use case: valid decimal value
        {
            var result = 12.34M.As<decimal>(id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(expected: 12.34M));
            Assert.That(result.Id, Is.EqualTo(expected: "id"));
        }

        // use case: valid integer value
        {
            var result = 1234.As<decimal>(id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(expected: 1234M));
            Assert.That(result.Id, Is.EqualTo(expected: "id"));
        }

        // use case: invalid value type
        {
            var result = new Exception().As<decimal>(id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Failure));
            Assert.That(result.Error.Message, Is.EqualTo(expected: "Unable to convert to decimal."));
        }
    }
}