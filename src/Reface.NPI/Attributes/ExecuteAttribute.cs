using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteAttribute : SqlAttribute
    {
        public ExecuteAttribute(string sql) : base(SqlCommandExecuteModes.Execute, sql)
        {
        }

        public ExecuteAttribute(string selector, string sql) : base(selector, SqlCommandExecuteModes.Execute, sql)
        {
        }
    }
}
