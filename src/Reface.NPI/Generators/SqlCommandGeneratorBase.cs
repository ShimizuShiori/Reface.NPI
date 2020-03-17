﻿using Reface.NPI.Models;
using System;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly IParameterLookupFactory parameterLookupFactory;

        public SqlCommandGeneratorBase()
        {
            this.parameterLookupFactory = NpiServicesCollection.GetService<IParameterLookupFactory>();
        }

        public SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments)
        {
            var context = SqlCommandGenerateContext.Create()
                .SetMethod(methodInfo)
                .Build();

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
                default: throw new NotImplementedException($"不能处理的命令类型 : {context.CommandInfo.Type.ToString()}");
            }

            if (arguments != null && arguments.Length != 0)
                this.parameterLookupFactory.Lookup(description, methodInfo, arguments);
            return description;
        }

        protected abstract SqlCommandDescription GenerateSelect(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateUpdate(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateDelete(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateInsert(SqlCommandGenerateContext context);
    }
}
