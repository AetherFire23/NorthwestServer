using Shared_Resources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.Extensions;
public static class EntityExtensions
{
    public static bool IsEntity(this IEntity self, IEntity otherEntity)
    {
        bool isValidReference = self.Id != Guid.Empty || otherEntity.Id != Guid.Empty;
        if (!isValidReference) throw new ArgumentException($"entity {otherEntity} had null Id. ");

        bool isEqual = self.Id == otherEntity.Id;
        return isEqual;
    }

    public static bool ContainsEntity<T>(this IEnumerable<T> self, IEntity element) where T : IEntity
    {
        bool isContained = self.Any(x => x.IsEntity(element));
        return isContained;
    }

    public static bool ContainsEntity<T>(this List<T> self, IEntity element) where T : IEntity
    {
        bool isContained = self.Any(x => x.IsEntity(element));
        return isContained;
    }

    public static T FindEntityOrDefault<T>(this List<T> self, T other) where T : IEntity
    {
        T entity = self.FirstOrDefault(x => x.IsEntity(other));
        return entity;
    }
}
