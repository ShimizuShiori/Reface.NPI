using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface ISqlCommandGenerator
    {
        SqlCommandDescription Generate(MethodInfo methodInfo);
    }
}
