using System.ComponentModel.DataAnnotations;
using WebAPI.Interfaces;

namespace WebAPI.Entities;
public abstract class EntityBase : IEquatable<EntityBase>, IEntity
{
    [Key]
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool Equals(EntityBase? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return this.Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        if (obj is not EntityBase) return false;

        return Equals((EntityBase)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityBase base1, EntityBase base2)
    {
        // Defer to Equals method for comparison
        return base1.Equals(base2);
    }

    public static bool operator !=(EntityBase base1, EntityBase base2)
    {
        return !(base1 == base2);
    }
}
