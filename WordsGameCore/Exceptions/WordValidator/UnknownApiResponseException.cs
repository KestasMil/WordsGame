using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WordsGameCore.Exceptions.WordValidator
{
    public class UnknownApiResponseException : Exception
    {
        public UnknownApiResponseException()
        {
        }

        public UnknownApiResponseException(string message) : base(message)
        {
        }

        public UnknownApiResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownApiResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
