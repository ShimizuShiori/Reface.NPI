using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface IParameterLookupFactory
    {
        void Lookup(ParameterLookupContext context);
    }
}
