namespace CraftingTools.Shared;

/// <summary>
/// Entity base class derived from V. Khorikov's <c>Entity</c> class
/// in "Domain-Driven Design in Practice" Pluralsight course.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Constructor.
    /// </summary>
    protected Entity(Guid id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Overrides the base method to
    /// check if two instances are the same <see cref="Entity"/>
    /// implementation and have the same ID.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return obj is Entity other
               && (ReferenceEquals(this, other)
                   || GetType() == other.GetType()
                   && this.Id == other.Id);
    }

    /// <summary>
    /// Overrides the base method to return
    /// a hashcode from the derived type and
    /// ID.
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType(), this.Id).GetHashCode();
    }

    /// <summary>
    /// Implements the <c>==</c> operator.
    /// </summary>
    public static bool operator ==(Entity? a, Entity? b)
    {
        return a is null && b is null || a is not null && b is not null && a.Equals(b);
    }

    /// <summary>
    /// Implements the <c>!=</c> operator.
    /// </summary>
    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }
    
    /// <summary>
    /// ID of the entity.
    /// </summary>
    public Guid Id { get; }
}