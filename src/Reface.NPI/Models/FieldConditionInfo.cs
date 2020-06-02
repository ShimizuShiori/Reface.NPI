namespace Reface.NPI.Models
{
    /// <summary>
    /// 条件信息
    /// </summary>
    public class FieldConditionInfo : IConditionInfo
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

        public bool IsNot { get; set; } = false;

        public override string ToString()
        {
            if (!IsNot)
                return $"[{Field}] [{Operators}] @{Parameter}";
            return $"NOT ( [{Field}] [{Operators}] @{Parameter} )";
        }
    }
}
