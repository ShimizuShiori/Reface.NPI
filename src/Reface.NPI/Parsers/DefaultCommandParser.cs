using Reface.NPI.Errors;
using Reface.NPI.Models;
using System.Collections.Generic;

namespace Reface.NPI.Parsers
{
    public class DefaultCommandParser : ICommandParser
    {
        private readonly ICache cache;

        public const string ACTION_GET = "Get";
        public const string ACTION_SELECT = "Select";
        public const string ACTION_FETCH = "Fetch";
        public const string ACTION_FIND = "Find";

        public const string ACTION_PAGING = "Paging";

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
            Queue<string> wordQueue = new Queue<string>(words);
            return ParseFromCommand(wordQueue);
        }

        private ICommandInfo ParseFromCommand(Queue<string> wordQueue)
        {
            string action = wordQueue.Dequeue();
            string realCommand = wordQueue.Join("", x => x);
            switch (action)
            {
                case ACTION_PAGING:
                    {
                        SelectInfo info = this.ParseFromCommand(wordQueue) as SelectInfo;
                        if (info == null)
                            throw new PagingMustBeFollowedBySelectException();
                        info.Paging = true;
                        return info;
                    }
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
                    return NpiServicesCollection.GetService<IInsertParser>().Parse(realCommand);
                default:
                    throw new NotSupportActionException(action);
            }
        }
    }
}
