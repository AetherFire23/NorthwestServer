using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared_Resources.Utils;

public static class ReflectionHelper
{
    // should make func variant
    //public static void IdentifyAttributeThenExecuteAction<TAttribute>(Assembly assembly, Action<Type> action) where TAttribute : Attribute
    //{
    //    var types = assembly.GetTypes()
    //        .Where(x => x.IsClass && !x.IsAbstract
    //        && CustomAttributeExtensions.GetCustomAttribute<TAttribute>(x) != null).ToList();

    //    foreach (var type in types)
    //    {
    //        action(type);
    //    }
    //}

    //public static List<TReturn> IdentifyAttributeThenFunc<TAttribute, TReturn>(Assembly assembly, Func<Type, TReturn> func) where TAttribute : Attribute
    //{
    //    var types = assembly.GetTypes()
    //        .Where(x => x.IsClass && !x.IsAbstract
    //        && CustomAttributeExtensions.GetCustomAttribute<TAttribute>(x) != null).ToList();


    //    var values = types.Select(x => func(x)).ToList();
    //    return values;
    //}

    //// IdentifyAttributeInheritingFrom

    //public static void IdentifyAttributeThenExecuteAction<TAttribute, T>(Assembly assembly, Action<Type> action) where TAttribute : Attribute
    //{
    //    var types = assembly.GetTypes()
    //        .Where(x => x.IsClass && !x.IsAbstract
    //        && CustomAttributeExtensions.GetCustomAttribute<TAttribute>(x) != null).ToList();

    //    foreach (var type in types)
    //    {
    //        action(type);
    //    }
    //}

    // seems to exclude static classes in dotnet 2.1
    public static List<Type> GetNonAbstractClassTypes(Assembly assembly) // stati classed are implicitly sealed
    {
        List<Type> types = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract).ToList();
        return types;
    }


    public static bool InheritanceFilter<TBase>(Type type) // use in .where
    {
        bool inheritsFrom = typeof(TBase).IsAssignableFrom(type);
        return inheritsFrom;
    }

    public static bool HasCustomAttributeFilter<TAttribute>(Type attributeType) where TAttribute : Attribute
    {
        bool hasCustomAttribute = CustomAttributeExtensions.GetCustomAttribute<TAttribute>(attributeType) != null;
        return hasCustomAttribute;
    }
}
