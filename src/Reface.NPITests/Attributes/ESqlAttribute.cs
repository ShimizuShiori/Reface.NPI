using System;

namespace Reface.NPITests.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ESqlAttribute : Attribute
    {
        public string Sql { get; private set; }

        public ESqlAttribute(string sql)
        {
            Sql = sql;
        }
    }
}
