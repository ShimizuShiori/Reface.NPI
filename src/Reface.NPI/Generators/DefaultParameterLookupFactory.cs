using System.Collections.Generic;

namespace Reface.NPI.Generators
{
    public class DefaultParameterLookupFactory : IParameterLookupFactory
    {
        private readonly IEnumerable<IParameterLookup> lookups;

        public DefaultParameterLookupFactory()
        {
            this.lookups = NpiServicesCollection.GetServices<IParameterLookup>();
        }

        public void Lookup(ParameterLookupContext context)
        {
            foreach (var lookup in lookups)
            {
                lookup.Lookup(context);
            }
        }
    }
}
