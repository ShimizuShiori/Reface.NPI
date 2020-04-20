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
                if (result is ICopy c)
                    return c.Copy();

                return result;
            }

            result = creator(key);

            if (result is ICopy c2)
            {
                var copied = c2.Copy();
                pool[hashKey] = copied;
            }
            else
            {
                pool[hashKey] = result;
            }
            return result;
        }
    }
}
