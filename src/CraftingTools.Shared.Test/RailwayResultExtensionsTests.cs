using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
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
    [TestCase(null, ExpectedResult = "Failure|message", TestName = "null_value")]
    [TestCase("", ExpectedResult = "Failure|message", TestName = "empty_value")]
    [TestCase(" ", ExpectedResult = "Success| ", TestName = "whitespace_value")]
    [TestCase("abc", ExpectedResult = "Success|abc", TestName = "valid_value")]
    public static string ToResultIsNotNullOrEmpty_ValidatesBehavior(string value)
    {
        var result = value.ToResultIsNotNullOrEmpty("message");
        return result.IsSuccess
            ? string.Join("|", result.Status, result.Value.ToSafeString())
            : string.Join("|", result.Status, result.Error.Message);
    }

    /// <summary>
    /// Validates the <c>ToResultIsNotNullOrEmpty</c> returns back the correct
    /// failure result if the string is null, empty, or whitespace.  
    /// </summary>
    [TestCase(null, ExpectedResult = "Failure|message", TestName = "null_value")]
    [TestCase("", ExpectedResult = "Failure|message", TestName = "empty_value")]
    [TestCase(" ", ExpectedResult = "Failure|message", TestName = "whitespace_value")]
    [TestCase("abc", ExpectedResult = "Success|abc", TestName = "valid_value")]
    public static string ToResultIsNotNullOrWhitespace_ValidatesBehavior(string value)
    {
        var result = value.ToResultIsNotNullOrWhitespace("message");
        return result.IsSuccess
            ? string.Join("|", result.Status, result.Value.ToSafeString())
            : string.Join("|", result.Status, result.Error.Message);
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
            Assert.That(ex!.ParamName, Is.EqualTo("inResult"));
        }

        // use case: null result parameter
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new object().ToResult().Check(default!, failureMessage: "message");
            });
            Assert.That(ex!.ParamName, Is.EqualTo("predicate"));
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
            Assert.That(outResult.Error.Message, Is.EqualTo("message"));
            Assert.That(outResult.Id, Is.EqualTo("id"));
        }
        
        // use case: default ID
        {
            var inResult = RailwayResult<object>.Success(new object(), id: "in_id");
            var outResult = inResult.Check(_ => true, failureMessage: "message");
            Assert.That(outResult, Is.EqualTo(inResult));
            Assert.That(outResult.Id, Is.EqualTo("in_id"));
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
            Assert.That(result.Error.Message, Is.EqualTo("message"));
            Assert.That(result.Id, Is.EqualTo("id"));
        }

        // use case: non-null value
        {
            var value = new object();
            var result = value.ToResultIsNotNull(failureMessage: "message", id: "id");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Unwrap(), Is.EqualTo(value));
            Assert.That(result.Id, Is.EqualTo("id"));
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
            Assert.That(ex!.ParamName, Is.EqualTo("result"));
        }
        
        // use case: null failures collection
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                var failures = default(ImmutableList<RailwayResultBase>);
                var _ = RailwayResult<object>.Success(new object()).UnwrapOrAddToFailuresImmutable(ref failures!);
            });
            Assert.That(ex!.ParamName, Is.EqualTo("failures"));
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
            Assert.That(failures.Count, Is.EqualTo(1));
            Assert.That(failures[0], Is.EqualTo(inResult));
        }
    }
}