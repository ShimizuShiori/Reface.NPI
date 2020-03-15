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
                Type baseType = this.context.IDaoType.GetInterface(Constant.TYPE_INPIDAO.FullName);
                if (baseType == null)
                    throw new ApplicationException("未从 INpiDao 继承"); //todo : 细化异常
                this.context.EntityType = baseType.GetGenericArguments()[0];

                this.context.TableName = this.context.EntityType.Name;
                TableAttribute ta = this.context.EntityType.GetCustomAttribute<TableAttribute>();
                if (ta != null) this.context.TableName = ta.Name;

                this.context.Properties = this.context.EntityType.GetProperties();
                return this.context;
            }
        }

    }
}
