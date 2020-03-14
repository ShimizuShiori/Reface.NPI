using Reface.NPI.Generators;
using System.Reflection;

namespace Reface.NPI
{
    public interface IParameterFiller
    {
        void Fill(SqlCommandDescription description, MethodInfo methodInfo, object[] values);
    }
}
