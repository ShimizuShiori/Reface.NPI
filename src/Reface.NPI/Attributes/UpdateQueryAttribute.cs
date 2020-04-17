using Reface.NPI.Generators;
using System;

namespace Reface.NPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UpdateQueryAttribute : QueryAttribute
    {
        public UpdateQueryAttribute(string selector, string sql) : base(selector, SqlCommandTypes.Update, sql)
        {

        }

        public UpdateQueryAttribute(string sql) : this("", sql)
        {

        }
    }
}
