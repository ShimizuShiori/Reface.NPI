using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class QueryAttribute : Attribute
    {
        public string Selector { get; private set; }
        public string Sql { get; private set; }

        public QueryAttribute(string selector, string sql)
        {
            Selector = selector;
            Sql = sql;
        }

        public QueryAttribute(string sql) : this("", sql)
        {

        }
    }
}
