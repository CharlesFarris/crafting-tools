using System;
using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for <see cref="Entity"/> class.
/// </summary>
internal static class EntityTest
{

    /// <summary>
    /// Validates the constructor behavior.
    /// </summary>
    public static void Ctor_ValidatesBehavior()
    {
        var id = Guid.Parse("C251B2AE-8927-4421-B6D3-47F6B29F2D6F");
        var entity = new MockEntityA(id);
        Assert.That(entity.Id, Is.EqualTo(id));
    }

    /// <summary>
    /// Validates the get hash code behavior.
    /// </summary>
    [Test]
    public static void GetHashCode_ValidatesBehavior()
    {
        var id = Guid.Parse("C251B2AE-8927-4421-B6D3-47F6B29F2D6F");

        // use case: check value
        {
            var entity = new MockEntityA(id);
            Assert.That(entity.GetHashCode(), Is.EqualTo((entity.GetType(), id).GetHashCode()));
        }
        
        // use case: verify same ID, different class
        {
            var entityA = new MockEntityA(id);
            var entityB = new MockEntityB(id);
            Assert.That(entityA.Id, Is.EqualTo(entityB.Id));
            Assert.That(entityA.GetHashCode(), Is.Not.EqualTo(entityB.GetHashCode()));
        }
    }

    /// <summary>
    /// Validates equality behavior.
    /// </summary>
    [Test]
    public static void Equals_ValidatesBehavior()
    {
        var leftId = Guid.Parse("C251B2AE-8927-4421-B6D3-47F6B29F2D6F");
        var rightId = Guid.Parse("E040E292-6877-44C7-8887-F138BA886C00");
        
        // use case: null comparison
        {
            var left = new MockEntityA(leftId);
            Assert.That(left.Equals(null), Is.False);
        }
        
        // use case: different type comparison
        {
            var left = new MockEntityA(leftId);
            var right = new object();
            Assert.That(left.Equals(right), Is.False);
        }
        
        // use case: different entity type, same ID comparison
        {
            var left = new MockEntityA(leftId);
            var right = new MockEntityB(leftId);
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(left.Equals(right), Is.False);
        }
        
        // use case: same entity type, different ID comparison
        {
            var left = new MockEntityA(leftId);
            var right = new MockEntityA(rightId);
            Assert.That(left.Equals(right), Is.False);
            Assert.That(left == right, Is.False);
            Assert.That(left != right, Is.True);
        }
        
        // use case: same entity type, same ID comparison
        {
            var left = new MockEntityA(leftId);
            var right = new MockEntityA(leftId);
            Assert.That(left.Equals(right), Is.True);
            Assert.That(left == right, Is.True);
            Assert.That(left != right, Is.False);
        }
        
        // use case: same entity instance
        {
            var left = new MockEntityA(leftId);
            // ReSharper disable EqualExpressionComparison
            Assert.That(left.Equals(left), Is.True);
            Assert.That(left == left, Is.True);
            Assert.That(left != left, Is.False);
            // ReSharper restore EqualExpressionComparison
        }
        
    }

    private sealed class MockEntityA : Entity
    {
        public MockEntityA(Guid id) : base(id)
        {
        }
    }

    private sealed class MockEntityB : Entity
    {
        public MockEntityB(Guid id) : base(id)
        {
        }
    }
}