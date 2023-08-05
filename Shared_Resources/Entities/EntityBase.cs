using Shared_Resources.Interfaces;
using System;

namespace Shared_Resources.Entities;
public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is IEntity otherEntity)
        {
            bool isValidReference = Id != Guid.Empty || otherEntity.Id != Guid.Empty;
            if (!isValidReference) throw new ArgumentException($"entity {otherEntity} had null Id. ");

            bool isEqual = Id == otherEntity.Id;
            return isEqual;
        }
        else
        {
            return base.Equals(obj);
        }
    }
}