namespace Reface.NPI.Models
{
    public class ConditionInfo
    {
        public string Field { get; set; }
        public string Operators { get; set; }

        /// <summary>
        /// 与后一个条件的连接词
        /// </summary>
        public ConditionJoiners JoinerToNext { get; set; } = ConditionJoiners.Null;

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

        public ConditionInfo() : this("", "")
        {
        }

        public override string ToString()
        {
            return $"[{Field}] [{Operators}] [{JoinerToNext.ToString()}]";
        }
    }
}
