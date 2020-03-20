using System;
using System.Collections.Generic;

namespace Reface.NPI
{
    public class DefaultCache : ICache
    {
        private static readonly Dictionary<string, object> pool = new Dictionary<string, object>();

        static DefaultCache()
        {
            DebugLogger.Debug("初始化缓存池");
        }

        public object GetOrCreate(string key, Func<string, object> creator)
        {
            object result;
            if (pool.TryGetValue(key, out result))
            {
                DebugLogger.Debug($"发现缓存 : {key}");
                return result;
            }

            result = creator(key);
            pool[key] = result;
            return result;
        }
    }
}
