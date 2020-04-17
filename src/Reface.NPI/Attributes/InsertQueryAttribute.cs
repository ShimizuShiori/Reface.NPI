using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InsertQueryAttribute : QueryAttribute
    {
        public InsertQueryAttribute(string selector, string sql) : base(selector, SqlCommandTypes.Insert, sql)
        {

        }

        public InsertQueryAttribute(string sql) : this("", sql)
        {

        }
    }
}
