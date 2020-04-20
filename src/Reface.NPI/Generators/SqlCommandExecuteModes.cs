namespace Reface.NPI.Generators
{
    /// <summary>
    /// SQL脚本执行模式
    /// </summary>
    public enum SqlCommandExecuteModes
    {
        /// <summary>
        /// 执行，一般是更新、删除操作
        /// </summary>
        Execute,
        /// <summary>
        /// 查询
        /// </summary>
        Query
    }
}
