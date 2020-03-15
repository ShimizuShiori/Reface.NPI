using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly IParameterFiller parameterFiller;

        public SqlCommandGeneratorBase()
        {
            this.parameterFiller = new DefaultParameterFiller();
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
                    break;
                case CommandInfoTypes.Update:
                    description = GenerateUpdate(context);
                    break;
                case CommandInfoTypes.Select:
                    description = GenerateSelect(context);
                    break;
                case CommandInfoTypes.Insert:
                    description = GenerateInsert(context);
                    break;
                default: throw new NotImplementedException($"不能处理的命令类型 : {context.CommandInfo.Type.ToString()}");
            }

            if (arguments != null && arguments.Length != 0)
                this.parameterFiller.Fill(description, methodInfo, arguments);
            return description;
        }

        protected abstract SqlCommandDescription GenerateSelect(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateUpdate(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateDelete(SqlCommandGenerateContext context);
        protected abstract SqlCommandDescription GenerateInsert(SqlCommandGenerateContext context);
        //{
        //    SqlCommandDescription description = new SqlCommandDescription();

        //    IEnumerable<ColumnAttribute> columns = entityType.GetProperties().Where(x => x.CanWrite && x.CanRead)
        //        .Select(x =>
        //        {
        //            ColumnAttribute columnAttribute = x.GetCustomAttribute<ColumnAttribute>();
        //            if (columnAttribute != null)
        //                return columnAttribute;
        //            return new ColumnAttribute(x.Name);
        //        });
        //    string fields = columns.Join(",", x => $"[{x.Name}]");
        //    string values = columns.Join(",", x => $"@{x.Name}");
        //    foreach (var p in columns)
        //    {
        //        description.AddParameter(new SqlParameterInfo(p.Name, ParameterUses.ForInsert));
        //    }
        //    description.SqlCommand = $"INSERT INTO [{tableName}]({fields})VALUES({values})";
        //    return description;
        //}
    }
}
