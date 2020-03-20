using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class SqlCommandGenerateContext
    {
        public ICommandInfo CommandInfo { get; private set; }
        public Type IDaoType { get; private set; }

        public Type EntityType { get; private set; }

        public string TableName { get; private set; }

        public IEnumerable<PropertyInfo> Properties { get; private set; }

        public MethodInfo Method { get; private set; }

        public static SqlCommandGenerateContextBuilder Create()
        {
            SqlCommandGenerateContext context = new SqlCommandGenerateContext();
            return new SqlCommandGenerateContextBuilder(context);
        }
        public class SqlCommandGenerateContextBuilder
        {
            private readonly SqlCommandGenerateContext context;
            public SqlCommandGenerateContextBuilder(SqlCommandGenerateContext context)
            {
                this.context = context;
            }

            public SqlCommandGenerateContextBuilder SetMethod(MethodInfo method)
            {
                this.context.Method = method;
                return this;
            }

            public SqlCommandGenerateContext Build()
            {
                var parser = new DefaultCommandParser();
                this.context.CommandInfo = parser.Parse(this.context.Method.Name);
                this.context.IDaoType = this.context.Method.DeclaringType;

                IEntityTypeProvider entityTypeProvider = NpiServicesCollection.GetService<IEntityTypeProvider>();
                this.context.EntityType = entityTypeProvider.Provide(this.context.Method);

                ITableNameProvider tableNameProvider = NpiServicesCollection.GetService<ITableNameProvider>();
                this.context.TableName = tableNameProvider.Provide(this.context.EntityType);

                this.context.Properties = this.context.EntityType.GetProperties();
                return this.context;
            }
        }

        public override string ToString()
        {
            return $"{IDaoType.FullName}.{Method.Name}";
        }

    }
}
