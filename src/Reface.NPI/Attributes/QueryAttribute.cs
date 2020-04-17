using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class QueryAttribute : Attribute
    {
        public string Selector { get; private set; }
        public SqlCommandTypes SqlCommandType { get; private set; }
        public string Sql { get; private set; }

        public QueryAttribute(string selector, SqlCommandTypes sqlCommandType, string sql)
        {
            Selector = selector;
            this.SqlCommandType = sqlCommandType;
            Sql = sql;
        }

        public QueryAttribute(SqlCommandTypes sqlCommandType, string sql) : this("", sqlCommandType, sql)
        {

        }
    }
}
