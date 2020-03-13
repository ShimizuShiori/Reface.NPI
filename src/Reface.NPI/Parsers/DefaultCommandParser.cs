﻿using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI.Parsers
{
    public class DefaultCommandParser : ICommandParser
    {
        public const string ACTION_GET = "Get";
        public const string ACTION_SELECT = "Select";
        public const string ACTION_FETCH = "Fetch";

        public const string ACTION_CREATE = "Create";
        public const string ACTION_INSERT = "Insert";
        public const string ACTION_NEW = "New";

        public const string ACTION_UPDATE = "Update";
        public const string ACTION_MODIFY = "Modify";

        public const string ACTION_REMOVE = "Remove";
        public const string ACTION_DELETE = "Delete";

        public ICommandInfo Parse(string command)
        {
            List<string> words = command.SplitToWords();
            string action = words.FirstOrDefault();
            string realCommand = command.Replace(action, "");
            switch (action)
            {
                case ACTION_GET:
                case ACTION_SELECT:
                case ACTION_FETCH:
                    return new DefaultSelectParser().Parse(realCommand);
                case ACTION_CREATE:
                case ACTION_INSERT:
                case ACTION_NEW:
                    return new InsertInfo();
                case ACTION_UPDATE:
                case ACTION_MODIFY:
                    return new DefaultUpdateParser().Parse(realCommand);
                case ACTION_REMOVE:
                case ACTION_DELETE:
                    return new DefaultDeleteParser().Parse(realCommand);
                default:
                    DebugLogger.Info($"{action} 未实现，不能处理");
                    throw new NotImplementedException($"未实现的 Action : {action}");
            }
        }
    }
}