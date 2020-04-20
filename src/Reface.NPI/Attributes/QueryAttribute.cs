using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class QueryAttribute : SqlAttribute
    {
        public QueryAttribute(string sql) : base(SqlCommandExecuteModes.Query, sql)
        {
        }

        public QueryAttribute(string selector, string sql) : base(selector, SqlCommandExecuteModes.Execute, sql)
        {
        }
    }
}
