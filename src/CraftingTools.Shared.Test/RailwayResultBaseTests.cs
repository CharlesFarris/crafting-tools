using System;
using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for <see cref="RailwayResultBase"/> class.
/// </summary>
internal static class RailwayResultBaseTests
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
                var _ = new MockRailwayResult(RailwayResultStatus.Unknown, Error.Empty);
            });
            Assert.That(ex?.ParamName, Is.EqualTo("status"));
        }
        
        // use case: set valid status
        {
            var result = new MockRailwayResult(RailwayResultStatus.Success, Error.Empty);
            Assert.That(result.Status, Is.EqualTo(RailwayResultStatus.Success));
            Assert.That(result.Error, Is.EqualTo(Error.Empty));
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
            var result = new MockRailwayResult(RailwayResultStatus.Failure, Error.Empty);
            Assert.That(result.IsSuccess, Is.False);
        }
        
        // use case: verify success behavior
        {
            var result = new MockRailwayResult(RailwayResultStatus.Success, Error.Empty);
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
            var result = new MockRailwayResult(RailwayResultStatus.Failure, Error.Empty);
            Assert.That(result.IsFailure, Is.True);
        }
        
        // use case: verify success behavior
        {
            var result = new MockRailwayResult(RailwayResultStatus.Success, Error.Empty);
            Assert.That(result.IsFailure, Is.False);
        }
    }

    /// <summary>
    /// Mock concrete implementation of <see cref="RailwayResultBase"/>.
    /// </summary>
    private class MockRailwayResult : RailwayResultBase
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MockRailwayResult(RailwayResultStatus status, Error error)
            : base(status, error)
        {
        }
    }
}