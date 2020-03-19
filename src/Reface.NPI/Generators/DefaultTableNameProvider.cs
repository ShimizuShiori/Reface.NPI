using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultTableNameProvider : ITableNameProvider
    {
        public string Provide(Type entityType)
        {
            string tableName = entityType.Name;

            TableAttribute ta = entityType.GetCustomAttribute<TableAttribute>();
            if (ta != null) tableName = ta.Name;

            return tableName;
        }
    }
}
