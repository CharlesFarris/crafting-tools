using System.Collections.Immutable;
using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for <see cref="RailwayResultFailureException"/> class.
/// </summary>
internal static class RailwayResultFailureExceptionTests
{
    /// <summary>
    /// Validates the behavior of the constructor.
    /// </summary>
    [Test]
    public static void Ctor_ValidatesBehavior()
    {
        // use case: null parameters
        {
            var exception = new RailwayResultFailureException(message: default, failures: default);
            Assert.That(exception.Message, Is.Empty);
            Assert.That(exception.Failures, Is.Not.Null);
            Assert.That(exception.Failures.IsEmpty, Is.True);
        }
        
        // use case: valid parameters
        {
            var failures = ImmutableList<RailwayResultBase>.Empty
                .Add(RailwayResult<object>.Failure(Error.Empty));
            var exception = new RailwayResultFailureException(message: "message", failures);
            Assert.That(exception.Message, Is.EqualTo("message"));
            Assert.That(exception.Failures, Is.EqualTo(failures));
        }
    }
}