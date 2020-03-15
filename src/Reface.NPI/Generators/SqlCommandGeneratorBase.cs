using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly ICommandParser commandParser;
        private readonly IParameterFiller parameterFiller;

        public SqlCommandGeneratorBase()
        {
            this.commandParser = new DefaultCommandParser();
            this.parameterFiller = new DefaultParameterFiller();
        }

        public SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments)
        {
            string methodName = methodInfo.Name;

            Type daoType = methodInfo.DeclaringType;
            Type idaoType = typeof(INpiDao<>);
            Type entityType = daoType.GetInterface(idaoType.FullName)
                .GetGenericArguments()[0];

            string tableName = entityType.Name;
            TableAttribute tableAttribute = entityType.GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;

            ICommandInfo commandInfo = this.commandParser.Parse(methodName);
            var description = GetSqlCommandDescriptionFromCommandInfo(commandInfo, entityType, tableName);
            if (arguments != null && arguments.Length != 0)
                this.parameterFiller.Fill(description, methodInfo, arguments);
            return description;
        }

        private SqlCommandDescription GetSqlCommandDescriptionFromCommandInfo(ICommandInfo commandInfo, Type entityType, string tableName)
        {
            switch (commandInfo.Type)
            {
                case CommandInfoTypes.Delete:
                    return this.Generate(commandInfo as DeleteInfo, tableName);
                case CommandInfoTypes.Update:
                    return this.Generate(commandInfo as UpdateInfo, tableName);
                case CommandInfoTypes.Select:
                    return this.Generate(commandInfo as SelectInfo, tableName);
                case CommandInfoTypes.Insert:
                    return this.Generate(tableName, entityType);
                default:
                    throw new NotImplementedException($"不能处理的命令类型 : {commandInfo.Type.ToString()}");
            }
        }

        protected abstract SqlCommandDescription Generate(SelectInfo selectInfo, string tableName);
        protected abstract SqlCommandDescription Generate(UpdateInfo updateInfo, string tableName);
        protected abstract SqlCommandDescription Generate(DeleteInfo deleteInfo, string tableName);

        protected virtual SqlCommandDescription Generate(string tableName, Type entityType)
        {
            SqlCommandDescription description = new SqlCommandDescription();

            IEnumerable<ColumnAttribute> columns = entityType.GetProperties().Where(x => x.CanWrite && x.CanRead)
                .Select(x =>
                {
                    ColumnAttribute columnAttribute = x.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute != null)
                        return columnAttribute;
                    return new ColumnAttribute(x.Name);
                });
            string fields = columns.Join(",", x => $"[{x.Name}]");
            string values = columns.Join(",", x => $"@{x.Name}");
            foreach (var p in columns)
            {
                description.AddParameter(new SqlParameterInfo(p.Name, ParameterUses.ForInsert));
            }
            description.SqlCommand = $"INSERT INTO [{tableName}]({fields})VALUES({values})";
            return description;
        }
    }
}
