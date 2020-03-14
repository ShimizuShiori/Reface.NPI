using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface ITableNameProvider
    {
        string Provide(MethodInfo methodInfo);
    }
}
