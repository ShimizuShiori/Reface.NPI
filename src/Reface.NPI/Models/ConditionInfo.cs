namespace Reface.NPI.Models
{
    public class ConditionInfo
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public string Operators { get; set; } = "";
        /// <summary>
        /// 参数名
        /// </summary>
        public string Parameter { get; set; }
        /// <summary>
        /// 与后一个条件的连接词
        /// </summary>
        public ConditionJoiners JoinerToNext { get; set; } = ConditionJoiners.Null;

        public override string ToString()
        {
            return $"[{Field}] [{Operators}] [{JoinerToNext.ToString()}]";
        }
    }
}
