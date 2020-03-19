using System.Reflection;

namespace Reface.NPI.Generators
{
    /// <summary>
    /// 字段名供应商，从 PropertyInfo 中提供字段名的接口
    /// </summary>
    public interface IFieldNameProvider
    {
        string Provide(PropertyInfo propertyInfo);
    }
}
