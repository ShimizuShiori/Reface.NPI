using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DeleteQueryAttribute : QueryAttribute
    {
        public DeleteQueryAttribute(string selector, string sql) : base(selector, SqlCommandTypes.Delete, sql)
        {

        }

        public DeleteQueryAttribute(string sql) : this("", sql)
        {

        }
    }
}
