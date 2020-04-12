using System;

namespace Reface.NPI.Errors
{
    public class EmptyCollectionException : Exception
    {
        public string ParameterName { get; private set; }

        public EmptyCollectionException(string parameterName)
            :base($"参数 {parameterName} 是集合类型，可是没有包含任何元素")
        {
            ParameterName = parameterName;
        }
    }
}
