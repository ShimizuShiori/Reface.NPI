using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CountQueryAttribute : QueryAttribute
    {
        public CountQueryAttribute(string selector, string sql) : base(selector, SqlCommandTypes.Count, sql)
        {

        }

        public CountQueryAttribute(string sql) : this("", sql)
        {

        }
    }
}
