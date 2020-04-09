using Reface.NPI.Attributes;
using Reface.NPI.Errors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators.ParameterLookups
{
    public class DefaultParameterLookup : IParameterLookup
    {
        private bool IsBaseType(Type type)
        {
            return type == typeof(string)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(short)
                || type == typeof(byte)
                || type == typeof(DateTime)
                || type == typeof(bool)
                || type == typeof(Guid)
                ;
        }

        private bool IsCollectionType(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private bool IsObject(Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }

        public void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            var parameterInfos = methodInfo.GetParameters();
            int length = parameterInfos.Length;
            for (int i = 0; i < length; i++)
            {
                var parameterInfo = parameterInfos[i];
                var value = values[i];
                this.Fill(description, GetParameterName(parameterInfo), parameterInfo.ParameterType, () => value);
            }
        }

        private void Fill(SqlCommandDescription description, string parameterName, Type valueType, Func<object> valueGetter)
        {
            if (IsBaseType(valueType))
                FillWithBaseType(description, parameterName, valueGetter);
            else if (IsCollectionType(valueType))
                FillWithCollectionType(description, parameterName, valueGetter);
            else if (IsObject(valueType))
                FillWithObjectType(description, valueType, valueGetter);
            else
                throw new CanNotConvertToSqlParameterException(valueType);
        }

        private string GetParameterName(ParameterInfo parameterInfo)
        {
            var attr = parameterInfo.GetCustomAttribute<ParameterAttribute>();
            if (attr != null) return attr.Name;
            return parameterInfo.Name;
        }
        private string GetParameterName(PropertyInfo proeprtyInfo)
        {
            var attr = proeprtyInfo.GetCustomAttribute<ParameterAttribute>();
            if (attr != null) return attr.Name;
            return proeprtyInfo.Name;
        }

        private void FillWithBaseType(SqlCommandDescription description, string parameterName, Func<object> valueGetter)
        {
            var parameter = description.Parameters.Values
                .Where(x => x.Name.ToLower() == parameterName.ToLower())
                .FirstOrDefault();
            if (parameter == null) return;
            parameter.Value = valueGetter();
        }

        private void FillWithCollectionType(SqlCommandDescription description, string parameterName, Func<object> valueGetter)
        {
            var parameter = description.Parameters.Values
                .Where(x => x.Name.ToLower() == parameterName.ToLower())
                .FirstOrDefault();
            if (parameter == null) return;
            IEnumerable collection = (IEnumerable)valueGetter();
            Type itemType = GetCollectionItemType(collection);
            int i = 0;
            List<string> newParameterNameList = new List<string>();
            foreach (object item in collection)
            {
                if (!IsBaseType(itemType))
                    throw new MustBeBaseTypeException(itemType);

                var itemSqlParameter = new SqlParameterInfo()
                {
                    Name = $"{parameter.Name}{i++}",
                    Use = parameter.Use,
                    Value = item
                };
                description.AddParameter(itemSqlParameter);
                newParameterNameList.Add($"@{itemSqlParameter.Name}");
            }
            string newParameterNameSql = newParameterNameList.Join(",", x => x);
            description.SqlCommand = description.SqlCommand.Replace($"@{parameter.Name}", $"({newParameterNameSql})");
            description.Parameters.Remove(parameter.Name);
        }

        private void FillWithObjectType(SqlCommandDescription description, Type valueType, Func<object> valueGetter)
        {
            var propertyInfos = valueType.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                string parameterNameOnProperty = GetParameterName(propertyInfo);
                this.Fill(description, parameterNameOnProperty, propertyInfo.PropertyType, () =>
                {
                    return propertyInfo.GetValue(valueGetter(), null);
                });
            }
        }

        private Type GetCollectionItemType(IEnumerable collection)
        {
            Type collectionType = collection.GetType();
            Type ienumerableItemType = typeof(IEnumerable<>);
            Type thisEnumerableItemType = collectionType.GetInterface(ienumerableItemType.FullName);

            if (thisEnumerableItemType != null)
                return thisEnumerableItemType.GetGenericArguments()[0];

            foreach (object obj in collection)
            {
                if (obj != null) return obj.GetType();
            }

            throw new CanNotGetItemTypeFromCollectionException(collectionType);
        }
    }
}
