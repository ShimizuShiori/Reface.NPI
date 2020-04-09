using System;

namespace Reface.NPI.Errors
{
    public class CanNotConvertToSqlParameterException : Exception
    {
        public Type Type { get; private set; }

        public CanNotConvertToSqlParameterException(Type type)
            :base($"指定的类型 {type.FullName} 无法转化执行时的 Sql 参数")
        {
            Type = type;
        }
    }
}
