using System;

namespace Reface.NPI
{
    public static class Ext
    {
        public static T GetOrCreate<T>(this ICache cache, string key, Func<string, T> creator)
        {
            object obj = cache.GetOrCreate(key, k => creator(k));
            return (T)obj;
        }
    }
}
