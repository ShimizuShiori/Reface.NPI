using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class DefaultFieldNameProvider : IFieldNameProvider
    {
        public string Provide(PropertyInfo propertyInfo)
        {
            string result = propertyInfo.Name;

            ColumnAttribute column = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (column != null) result = column.Name;

            return result;
        }
    }
}
