using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SelectQueryAttribute : QueryAttribute
    {
        public SelectQueryAttribute(string sql) : this("", sql)
        {
        }

        public SelectQueryAttribute(string selector, string sql) : base(selector, SqlCommandTypes.Select, sql)
        {
        }
    }
}
