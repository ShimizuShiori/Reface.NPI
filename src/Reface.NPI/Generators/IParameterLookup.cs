using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface IParameterLookup
    {
        void Lookup(ParameterLookupContext context);
    }
}
