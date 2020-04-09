using System;

namespace Reface.NPI.Errors
{
    public class CanNotGetItemTypeFromCollectionException : Exception
    {
        public Type Type { get; private set; }

        public CanNotGetItemTypeFromCollectionException(Type type)
            : base($"无法从集合类型 {type.FullName} 中分析出成员类型")
        {
            Type = type;
        }
    }
}
