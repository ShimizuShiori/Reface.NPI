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
                || type == typeof(uint)
                || type == typeof(ulong)
                || type == typeof(ushort)
                ;
        }

        private bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        private bool IsCollectionType(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        private bool IsObject(Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }


        public void Lookup(ParameterLookupContext context)
        {
            SqlCommandDescription description = context.Description;
            MethodInfo methodInfo = context.MethodInfo;
            object[] values = context.Values;
            var parameterInfos = methodInfo.GetParameters();
            int length = parameterInfos.Length;
            for (int i = 0; i < length; i++)
            {
                var parameterInfo = parameterInfos[i];
                var value = values[i];
                this.Fill(context.SqlCommandGenerator, description, GetParameterName(parameterInfo), parameterInfo.ParameterType, () => value);
            }
        }

        private void Fill(ISqlCommandGenerator generator, SqlCommandDescription description, string parameterName, Type valueType, Func<object> valueGetter)
        {
            if (IsBaseType(valueType))
                FillWithBaseType(description, parameterName, valueGetter);
            else if (IsCollectionType(valueType))
                FillWithCollectionType(generator, description, parameterName, valueGetter);
            else if (IsObject(valueType))
                FillWithObjectType(generator, description, valueType, valueGetter);
            else if (IsEnum(valueType))
                FillWithBaseType(description, parameterName, () => Convert.ToInt32(valueGetter()));
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

        private void FillWithCollectionType(ISqlCommandGenerator generator, SqlCommandDescription description, string parameterName, Func<object> valueGetter)
        {
            var parameter = description.Parameters.Values
                .Where(x => x.Name.ToLower() == parameterName.ToLower())
                .FirstOrDefault();
            if (parameter == null) return;
            IEnumerable collection = (IEnumerable)valueGetter();
            Type itemType = GetCollectionItemType(collection);
            int i = 0;
            List<string> newParameterNameList = new List<string>();
            bool isCollectionEmpty = true;
            foreach (object item in collection)
            {
                isCollectionEmpty = false;
                if (!IsBaseType(itemType))
                    throw new MustBeBaseTypeException(itemType);

                var itemSqlParameter = new SqlParameterInfo()
                {
                    Name = $"{parameter.Name}{i++}",
                    Value = item
                };
                description.AddParameter(itemSqlParameter);
                newParameterNameList.Add(generator.GenerateParameterName(itemSqlParameter.Name));
            }
            if (isCollectionEmpty)
                throw new EmptyCollectionException(parameterName);
            string newParameterNameSql = newParameterNameList.Join(",", x => x);
            description.SqlCommand = description.SqlCommand.Replace(generator.GenerateParameterName(parameter.Name), $"({newParameterNameSql})");

            description.Parameters.Remove(parameter.Name);
        }

        private void FillWithObjectType(ISqlCommandGenerator generator, SqlCommandDescription description, Type valueType, Func<object> valueGetter)
        {
            var propertyInfos = valueType.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                string parameterNameOnProperty = GetParameterName(propertyInfo);
                this.Fill(generator, description, parameterNameOnProperty, propertyInfo.PropertyType, () =>
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
