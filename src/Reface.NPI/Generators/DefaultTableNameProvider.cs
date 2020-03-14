using Reface.NPI.Attributes;
using System;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultTableNameProvider : ITableNameProvider
    {
        public string Provide(MethodInfo methodInfo)
        {
            Type interfaceType = methodInfo.DeclaringType;
            TableAttribute tableAttribute = interfaceType.GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
                throw new ApplicationException("未找到 TableName");
            return tableAttribute.TableName;
        }
    }
}
