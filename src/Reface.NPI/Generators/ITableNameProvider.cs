using System;

namespace Reface.NPI.Generators
{
    /// <summary>
    /// 表名供应商接口，通过实体类型，向外部提供表名
    /// </summary>
    public interface ITableNameProvider
    {
        string Provide(Type entityType);
    }
}
