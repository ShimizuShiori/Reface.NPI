namespace Reface.NPI.Generators
{
    /// <summary>
    /// 参数用途
    /// </summary>
    public enum ParameterUses
    {
        /// <summary>
        /// 用来 Set（在Update的Set子句中）
        /// </summary>
        ForSet,
        /// <summary>
        /// 用来作为条件
        /// </summary>
        ForCondition,
        ForInsert
    }
}
