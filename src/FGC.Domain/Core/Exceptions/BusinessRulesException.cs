﻿
using System.Runtime.Serialization;

namespace FGC.Domain.Core.Exceptions
{
    [Serializable]
    
    public class BusinessRulesException : Exception
    {
        public BusinessRulesException()
        {
        }

        public BusinessRulesException(string message)
            : base(message)
        {
        }

        public BusinessRulesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BusinessRulesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
