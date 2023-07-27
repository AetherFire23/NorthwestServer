using System;
using System.Reflection;

namespace Shared_Resources.Utils;

public static class TypeExtensions
{
    public static bool TryGetCustomAttribute<T>(this Type type, out T attribute) where T : Attribute
    {
        attribute = CustomAttributeExtensions.GetCustomAttribute<T>(type);
        bool hasAttribute = attribute != null;
        return hasAttribute;
    }

    /// <summary> crash if not foud </summary>
    public static T GetCustomAttribute<T>(this Type type) where T : Attribute
    {
        var attribute = CustomAttributeExtensions.GetCustomAttribute<T>(type) ?? throw new Exception($"Attribute not found : {type.Name}");
        return attribute;
    }
}
