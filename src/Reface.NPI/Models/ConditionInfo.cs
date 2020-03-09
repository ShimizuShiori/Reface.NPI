
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
        public string Operators { get; private set; }

        /// <summary>
        /// 与后一个条件的连接词
        /// </summary>
        public ConditionJoiners JoinerToNext { get; private set; } = ConditionJoiners.Null;

        public ConditionInfo(string field, string operators, ConditionJoiners joinerToNext)
        {
            Field = field;
            Operators = operators;
            JoinerToNext = joinerToNext;
        }

        public ConditionInfo(string field, string operators)
            : this(field, operators, ConditionJoiners.Null)
        {
        }
    }
}
