using System;

namespace Reface.NPI.Errors
{
    public class MustBeBaseTypeException : Exception
    {
        public Type Type { get; private set; }

        public MustBeBaseTypeException(Type type)
            : base($"指定的类型 {type.FullName} 不是基础类型")
        {
            this.Type = type;
        }
    }
}
