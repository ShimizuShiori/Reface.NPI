using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface IParameterLookupFactory
    {
        void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values);
    }
}
