namespace CraftingTools.Shared;

/// <summary>
/// Value object base class derived from V. Khorikov's <c>ValueObject</c> class
/// in "Domain-Driven Design in Practice" Pluralsight course.
/// </summary>
public abstract class ValueObject<T> where T: ValueObject<T>
{
    /// <summary>
    /// Overrides the base method to use the derived class'
    /// <see cref="EqualsCore"/> method.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is T valueObject && this.EqualsCore(valueObject);
    }

    /// <summary>
    /// Equality method to be implemented in the derived class.
    /// </summary>
    protected abstract bool EqualsCore(T other);

    /// <summary>
    /// Override the base method to the the derived class'
    /// <see cref="GetHashCodeCore"/> method.
    /// </summary>
    public override int GetHashCode()
    {
        return this.GetHashCodeCore();
    }

    /// <summary>
    /// Hashcode method to be implemented in the derived class.
    /// </summary>
    /// <returns></returns>
    protected abstract int GetHashCodeCore();
    
    /// <summary>
    /// Implements the <c>==</c> operator.
    /// </summary>
    public static bool operator ==(ValueObject<T>? a, ValueObject<T>? b)
    {
        return a is null && b is null || a is not null && b is not null && a.Equals(b);
    }

    /// <summary>
    /// Implements the <c>!=</c> operator.
    /// </summary>
    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
    {
        return !(a == b);
    }
}