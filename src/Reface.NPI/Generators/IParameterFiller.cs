using System.Reflection;

namespace Reface.NPI.Generators
{
    public interface IParameterFiller
    {
        void Fill(SqlCommandDescription description, MethodInfo methodInfo, object[] values);
    }
}
