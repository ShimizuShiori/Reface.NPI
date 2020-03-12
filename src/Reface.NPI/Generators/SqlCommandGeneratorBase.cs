using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Generators
{
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly ICommandParser commandParser;

        public SqlCommandGeneratorBase()
        {
            this.commandParser = new DefaultCommandParser();
        }

        public SqlCommandDescription Generate(MethodInfo methodInfo)
        {
            string methodName = methodInfo.Name;
            ICommandInfo commandInfo = this.commandParser.Parse(methodName);
            switch (commandInfo.Type)
            {
                case CommandInfoTypes.Insert:
                    return this.Generate(commandInfo as InsertInfo);
                case CommandInfoTypes.Delete:
                    return this.Generate(commandInfo as DeleteInfo);
                case CommandInfoTypes.Update:
                    return this.Generate(commandInfo as UpdateInfo);
                case CommandInfoTypes.Select:
                    return this.Generate(commandInfo as SelectInfo);
                default:
                    throw new NotImplementedException($"不能处理的命令类型 : {commandInfo.Type.ToString()}");
            }
        }

        protected abstract SqlCommandDescription Generate(SelectInfo selectInfo);
        protected abstract SqlCommandDescription Generate(UpdateInfo updateInfo);
        protected abstract SqlCommandDescription Generate(DeleteInfo deleteInfo);
        protected abstract SqlCommandDescription Generate(InsertInfo insertInfo);
    }
}
