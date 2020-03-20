using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultParameterLookupFactory : IParameterLookupFactory
    {
        private readonly IEnumerable<IParameterLookup> lookups;
        private readonly ICache cache;

        public DefaultParameterLookupFactory()
        {
            this.lookups = NpiServicesCollection.GetServices<IParameterLookup>();
            this.cache = NpiServicesCollection.GetService<ICache>();
        }

        public void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            string cacheKey = $"ParameterLookups_{methodInfo.GetFullName()}";
            IEnumerable<IParameterLookup> thisLookups
                = this.cache.GetOrCreate<IEnumerable<IParameterLookup>>(cacheKey, key => 
                {
                    return this.lookups.Where(l => l.Match(description, methodInfo));
                });
            foreach (var lookup in thisLookups)
            {
                lookup.Lookup(description, methodInfo, values);
            }
        }
    }
}
