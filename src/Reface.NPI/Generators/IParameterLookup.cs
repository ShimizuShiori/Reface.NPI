using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface IParameterLookup
    {
        bool Match(SqlCommandDescription description, MethodInfo methodInfo);
        void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values);
    }
}
