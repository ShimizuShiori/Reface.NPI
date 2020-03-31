using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface IParameterLookup
    {
        void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values);
    }
}
