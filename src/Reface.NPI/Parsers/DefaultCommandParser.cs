using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public class DefaultCommandParser : ICommandParser
    {
        private readonly ICache cache;

        public const string ACTION_GET = "Get";
        public const string ACTION_SELECT = "Select";
        public const string ACTION_FETCH = "Fetch";
        public const string ACTION_FIND = "Find";

        public const string ACTION_UPDATE = "Update";
        public const string ACTION_MODIFY = "Modify";

        public const string ACTION_REMOVE = "Remove";
        public const string ACTION_DELETE = "Delete";

        public const string ACTION_INSERT = "Insert";
        public const string ACTION_NEW = "New";
        public const string ACTION_CREATE = "Create";

        public DefaultCommandParser()
        {
            this.cache = NpiServicesCollection.GetService<ICache>();
        }

        public ICommandInfo Parse(string command)
        {
            string cacheKey = $"COMMAND_INFO_{command}";
            return this.cache.GetOrCreate<ICommandInfo>(cacheKey, key => ParseFromCommand(command));
        }

        private ICommandInfo ParseFromCommand(string command)
        {
            List<string> words = command.SplitToWords();
            string action = words.FirstOrDefault();
            string realCommand = command.Replace(action, "");
            switch (action)
            {
                case ACTION_GET:
                case ACTION_SELECT:
                case ACTION_FETCH:
                case ACTION_FIND:
                    return NpiServicesCollection.GetService<ISelectParser>().Parse(realCommand);
                case ACTION_UPDATE:
                case ACTION_MODIFY:
                    return NpiServicesCollection.GetService<IUpdateParser>().Parse(realCommand);
                case ACTION_REMOVE:
                case ACTION_DELETE:
                    return NpiServicesCollection.GetService<IDeleteParser>().Parse(realCommand);
                case ACTION_CREATE:
                case ACTION_NEW:
                case ACTION_INSERT:
                    return new InsertInfo();
                default:
                    DebugLogger.Info($"{action} 未实现，不能处理");
                    throw new NotImplementedException($"未实现的 Action : {action}");
            }
        }
    }
}
