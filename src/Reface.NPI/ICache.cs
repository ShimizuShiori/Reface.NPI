using System;

namespace Reface.NPI
{
    public interface ICache
    {
        object GetOrCreate(string key, Func<string, object> creator);
    }
}
