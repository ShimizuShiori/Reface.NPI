﻿using Reface.NPI.Attributes;
using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly IParameterLookupFactory parameterLookupFactory;
        private readonly ICache cache;

        public SqlCommandGeneratorBase()
        {
            this.parameterLookupFactory = NpiServicesCollection.GetService<IParameterLookupFactory>();
            this.cache = NpiServicesCollection.GetService<ICache>();
        }

        public SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments)
        {
            var context = SqlCommandGenerateContext.Create()
                .SetMethod(methodInfo)
                .Build();

            string cacheKey = $"SqlCOmmandDescription_{context.ToString()}";
            SqlCommandDescription description = cache.GetOrCreate<SqlCommandDescription>(cacheKey, key =>
            {
                if (context.HasAnyQueryAttribute)
                    return GetDescriptionByQuery(context);
                else
                    return GetSqlCommandDescriptionWithourParameterFilled(context);
            });

            if (arguments != null && arguments.Length != 0)
            {
                ParameterLookupContext lookupContet
                    = new ParameterLookupContext
                    (
                        this,
                        description,
                        methodInfo,
                        arguments
                    );
                this.parameterLookupFactory.Lookup(lookupContet);
            }
            return description;
        }

        private SqlCommandDescription GetSqlCommandDescriptionWithourParameterFilled(SqlCommandGenerateContext context)
        {
            SqlCommandDescription description;
            switch (context.CommandInfo.Type)
            {
                case CommandInfoTypes.Delete:
                    description = GenerateDelete(context);
                    description.Type = SqlCommandTypes.Delete;
                    break;
                case CommandInfoTypes.Update:
                    description = GenerateUpdate(context);
                    description.Type = SqlCommandTypes.Update;
                    break;
                case CommandInfoTypes.Select:
                    description = GenerateSelect(context);
                    description.Type = SqlCommandTypes.Select;
                    break;
                case CommandInfoTypes.Insert:
                    description = GenerateInsert(context);
                    description.Type = SqlCommandTypes.Insert;
                    break;
                case CommandInfoTypes.Count:
                    description = GenerateCount(context);
                    description.Type = SqlCommandTypes.Count;
                    break;
                default: throw new NotImplementedException($"不能处理的命令类型 : {context.CommandInfo.Type.ToString()}");
            }
            return description;
        }

        protected abstract SqlCommandDescription GenerateSelect(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateUpdate(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateDelete(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateInsert(SqlCommandGenerateContext context);

        protected abstract SqlCommandDescription GenerateCount(SqlCommandGenerateContext context);
        public abstract string GenerateParameterName(string name);

        private SqlCommandDescription GetDescriptionByQuery(SqlCommandGenerateContext context)
        {
            string querySelector = NpiConfig.QuerySelector;
            IEnumerable<QueryAttribute> queryAttributes = context.QueryAttributes.Where(x => x.Selector == querySelector);
            if (queryAttributes.Count() > 1)
                throw new ApplicationException("匹配到多个 QueryAttribute");

            QueryAttribute queryAttribute = queryAttributes.FirstOrDefault();

            SqlCommandDescription description = new SqlCommandDescription()
            {
                SqlCommand = queryAttribute.Sql,
                Type = queryAttribute.SqlCommandType
            };

            foreach (var methodParameter in context.Method.GetParameters())
            {
                description.AddParameter(new SqlParameterInfo(methodParameter.Name));
            }

            return description;
        }
    }
}
