using System;
using System.Collections.Generic;

namespace Reface.NPI
{
    public class DefaultCache : ICache
    {
        private static readonly Dictionary<int, object> pool = new Dictionary<int, object>();

        static DefaultCache()
        {
            DebugLogger.Debug("初始化缓存池");
        }

        public object GetOrCreate(string key, Func<string, object> creator)
        {
            object result;

            int hashKey = key.GetHashCode();
            DebugLogger.Debug($"HashKey : [{key}] => [{hashKey}]");

            if (pool.TryGetValue(hashKey, out result))
            {
                DebugLogger.Debug($"发现缓存 : {hashKey}");
                return result;
            }

            result = creator(key);
            pool[hashKey] = result;
            return result;
        }
    }
}
