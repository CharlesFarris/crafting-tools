using System;
using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for <see cref="ResultBase"/> class.
/// </summary>
internal static class ResultBaseTests
{
    /// <summary>
    /// Validates the behavior of the constructor.
    /// </summary>
    [Test]
    public static void Ctor_ValidatesBehavior()
    {
        // use case: unknown status throws exception
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var _ = new MockResult(ResultStatus.Unknown);
            });
            Assert.That(ex?.ParamName, Is.EqualTo("status"));
        }
        
        // use case: set valid status
        {
            var result = new MockResult(ResultStatus.Success);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        }
    }

    /// <summary>
    /// Validates the behavior of the <c>IsSuccess</c> property.
    /// </summary>
    [Test]
    public static void IsSuccess_validatesBehavior()
    {
        // use case: verify failure behavior
        {
            var result = new MockResult(ResultStatus.Failure);
            Assert.That(result.IsSuccess, Is.False);
        }
        
        // use case: verify success behavior
        {
            var result = new MockResult(ResultStatus.Success);
            Assert.That(result.IsSuccess, Is.True);
        }
    }
    
    /// <summary>
    /// Validates the behavior fo the <c>IsFailure</c> property.
    /// </summary>
    [Test]
    public static void IsFailure_ValidatesBehavior()
    {
        // use case: verify failure behavior
        {
            var result = new MockResult(ResultStatus.Failure);
            Assert.That(result.IsFailure, Is.True);
        }
        
        // use case: verify success behavior
        {
            var result = new MockResult(ResultStatus.Success);
            Assert.That(result.IsFailure, Is.False);
        }
    }

    /// <summary>
    /// Mock concrete implementation of <see cref="ResultBase"/>.
    /// </summary>
    private class MockResult : ResultBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MockResult(ResultStatus status)
            : base(status)
        {
        }
    }
}