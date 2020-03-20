using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultFieldNameProvider : IFieldNameProvider
    {
        private readonly ICache cache;

        public DefaultFieldNameProvider()
        {
            this.cache = NpiServicesCollection.GetService<ICache>();
        }

        public string Provide(PropertyInfo propertyInfo)
        {
            string cacheKey = $"FieldName_{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}";

            return this.cache.GetOrCreate<string>(cacheKey, key =>
            {
                string result = propertyInfo.Name;

                ColumnAttribute column = propertyInfo.GetCustomAttribute<ColumnAttribute>();
                if (column != null) result = column.Name;

                return result;
            });
        }
    }
}
