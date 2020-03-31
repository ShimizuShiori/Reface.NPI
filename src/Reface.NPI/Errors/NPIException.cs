using System;
using System.Runtime.Serialization;

namespace Reface.NPI.Errors
{
    public class NPIException : Exception
    {
        public NPIException()
        {
        }

        public NPIException(string message) : base(message)
        {
        }

        public NPIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NPIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
