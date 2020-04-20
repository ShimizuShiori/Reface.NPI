using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    /// <summary>
    /// 通过将该特征定义在方法上，
    /// 可以跳过对方法名称分析的过程，
    /// 直接以 Sql 中指定的语句进行查询。
    /// 使用此标签定义的 Sql 语句，参数部分请以 @ 或 : 开头。
    /// 如果其它数据库类型有着不同的参数表示，可以联系作者。
    /// 也可以自己实现 <see cref="ISqlParameterFinder"/> 接口，
    /// 再使用 <see cref="NpiServicesCollection.ReplaceService{T}(Func{Type, T})"/> 方法替换已有的功能
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SqlAttribute : Attribute
    {
        /// <summary>
        /// NPI 允许开发者在一个方法上定义多个 <see cref="SqlAttribute"/>，
        /// 以应对可能存在的数据库兼容性问题。
        /// 不同的 <see cref="SqlAttribute"/> 可以通过该属性来区别。
        /// 开发者可以通过修改属性 <see cref="NpiConfig.QuerySelector"/> 来指定使用哪一个 <see cref="SqlAttribute"/>
        /// </summary>
        public string Selector { get; private set; }

        /// <summary>
        /// 通过该属性，可以指定该特征对数据库的操作类型，以应对不同的数据库执行方式。
        /// </summary>
        public SqlCommandExecuteModes Mode { get; private set; }

        /// <summary>
        /// Sql 语句，语句中的参数请使用 : 或 @ 开头，视对应的数据库类型而决定。
        /// </summary>
        public string Sql { get; private set; }

        public SqlAttribute(string selector, SqlCommandExecuteModes mode, string sql)
        {
            Selector = selector;
            this.Mode = mode;
            Sql = sql;
        }

        public SqlAttribute(SqlCommandExecuteModes mode, string sql) : this("", mode, sql)
        {

        }
    }
}
