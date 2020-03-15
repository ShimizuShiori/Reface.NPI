using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly ICommandParser commandParser;
        private readonly ITableNameProvider tableNameProvider;
        private readonly IParameterFiller parameterFiller;

        public SqlCommandGeneratorBase()
        {
            this.commandParser = new DefaultCommandParser();
            this.tableNameProvider = new DefaultTableNameProvider();
            this.parameterFiller = new DefaultParameterFiller();
        }

        public SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments)
        {
            string tableName = this.tableNameProvider.Provide(methodInfo);
            string methodName = methodInfo.Name;
            ICommandInfo commandInfo = this.commandParser.Parse(methodName);
            var description = GetSqlCommandDescriptionFromCommandInfo(commandInfo, tableName);
            if (arguments != null && arguments.Length != 0)
                this.parameterFiller.Fill(description, methodInfo, arguments);
            return description;
        }

        private SqlCommandDescription GetSqlCommandDescriptionFromCommandInfo(ICommandInfo commandInfo, string tableName)
        {
            switch (commandInfo.Type)
            {
                case CommandInfoTypes.Delete:
                    return this.Generate(commandInfo as DeleteInfo, tableName);
                case CommandInfoTypes.Update:
                    return this.Generate(commandInfo as UpdateInfo, tableName);
                case CommandInfoTypes.Select:
                    return this.Generate(commandInfo as SelectInfo, tableName);
                default:
                    throw new NotImplementedException($"不能处理的命令类型 : {commandInfo.Type.ToString()}");
            }
        }

        protected abstract SqlCommandDescription Generate(SelectInfo selectInfo, string tableName);
        protected abstract SqlCommandDescription Generate(UpdateInfo updateInfo, string tableName);
        protected abstract SqlCommandDescription Generate(DeleteInfo deleteInfo, string tableName);
    }
}
