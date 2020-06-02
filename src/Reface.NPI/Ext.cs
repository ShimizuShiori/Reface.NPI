using System;
using System.Reflection;

namespace Reface.NPI
{
    public static class Ext
    {
        public static T GetOrCreate<T>(this ICache cache, string key, Func<string, T> creator)
        {
            object obj = cache.GetOrCreate(key, k => creator(k));
            return (T)obj;
        }

        public static string GetFullName(this MethodInfo method)
        {
            return $"{method.DeclaringType.FullName}.{method.Name}";
        }

        public static T As<T>(this object value)
        {
            return (T)value;
        }
    }
}
