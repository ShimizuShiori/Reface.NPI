using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultParameterLookupFactory : IParameterLookupFactory
    {
        private readonly IEnumerable<IParameterLookup> lookups;

        public DefaultParameterLookupFactory()
        {
            this.lookups = NpiServicesCollection.GetServices<IParameterLookup>();
        }

        public void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            foreach (var lookup in lookups)
            {
                lookup.Lookup(description, methodInfo, values);
            }
        }
    }
}
