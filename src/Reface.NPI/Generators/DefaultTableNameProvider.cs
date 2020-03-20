using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultTableNameProvider : ITableNameProvider
    {
        private readonly ICache cache;

        public DefaultTableNameProvider()
        {
            this.cache = NpiServicesCollection.GetService<ICache>();
        }
        public string Provide(Type entityType)
        {
            string cacheKey = $"TableName_{entityType.FullName}";

            return this.cache.GetOrCreate<string>(cacheKey, key =>
            {
                string tableName = entityType.Name;

                TableAttribute ta = entityType.GetCustomAttribute<TableAttribute>();
                if (ta != null) tableName = ta.Name;

                return tableName;
            });

        }
    }
}
