
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Models
{
    public class ConditionInfo
    {
        public string Field { get; private set; }
        public ConditionOperators Operators { get; private set; }

        /// <summary>
        /// 与前一个条件的连接词
        /// </summary>
        public ConditionJoiners Joiner { get; private set; } = ConditionJoiners.Null;

        public ConditionInfo(string field, ConditionOperators operators, ConditionJoiners joiner)
        {
            Field = field;
            Operators = operators;
            Joiner = joiner;
        }

        public ConditionInfo(string field, ConditionOperators operators)
            : this(field, operators, ConditionJoiners.Null)
        {
        }
    }
}
