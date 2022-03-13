using NUnit.Framework;

namespace CraftingTools.Shared.Test;

/// <summary>
/// Tests for <see cref="ValueObject{T}"/> class.
/// </summary>
internal static class ValueObjectTests
{
    /// <summary>
    /// Validates the behavior of the <c>Equals</c> method,
    /// <c>== operator</c>, and <c>!= operator</c>.
    /// </summary>
    [Test]
    public static void Equals_ValidatesBehavior()
    {
        // use case: null comparison
        {
            var left = new MockValueObjectA(value: "left");
            Assert.That(left.Equals(null), Is.False);
        }

        // use case: not a value object type comparison
        {
            var left = new MockValueObjectA(value: "left");
            var right = new object();
            Assert.That(left.Equals(right), Is.False);
        }
        
        // use case: different value object type comparison
        {
            var left = new MockValueObjectA(value: "left");
            var right = new MockValueObjectB(value: 1234);
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(left.Equals(right), Is.False);
        }
        
        // use case: same value object type, different value comparison
        {
            var left = new MockValueObjectA(value: "left");
            var right = new MockValueObjectA(value: "right");
            Assert.That(left.Equals(right), Is.False);
            Assert.That(left == right, Is.False);
            Assert.That(left != right, Is.True);
        }
        
        // use case: same value object type, same value comparison
        {
            var left = new MockValueObjectA(value: "left");
            var right = new MockValueObjectA(value: "left");
            Assert.That(left.Equals(right), Is.True);
            Assert.That(left == right, Is.True);
            Assert.That(left != right, Is.False);
        }
        
        // use case: same instance comparison
        {
            var left = new MockValueObjectA(value: "left");
            // ReSharper disable EqualExpressionComparison
            Assert.That(left.Equals(left), Is.True);
            Assert.That(left == left, Is.True);
            Assert.That(left != left, Is.False);
            // ReSharper restore EqualExpressionComparison
        }
    }

    /// <summary>
    /// Validates the behaviors of the <c>GetHashCode</c> method.
    /// </summary>
    [Test]
    public static void GetHashCode_ValidatesBehavior()
    {
        const string value = "1234_test";
        var valueObject = new MockValueObjectA(value);
        Assert.That(valueObject.GetHashCode(), Is.EqualTo(value.GetHashCode()));
    }

    private sealed class MockValueObjectA : ValueObject<MockValueObjectA>
    {
        public MockValueObjectA(string value)
        {
            this.Value = value;
        }
        protected override bool EqualsCore(MockValueObjectA other)
        {
            return this.Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return this.Value.GetHashCode();
        }

        private string Value
        {
            get;
        }
        
    }

    private sealed class MockValueObjectB : ValueObject<MockValueObjectB>
    {
        public MockValueObjectB(int value)
        {
            this._value = value;
        }
        
        protected override bool EqualsCore(MockValueObjectB other)
        {
            return this._value == other._value;
        }
        
        protected override int GetHashCodeCore()
        {
            return this._value.GetHashCode();
        }

        private readonly int _value;
    }
}