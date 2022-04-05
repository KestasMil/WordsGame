using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WordsGameCore.Exceptions
{
    public class InvalidLettersUsedException : Exception
    {
        public InvalidLettersUsedException()
        {
        }

        public InvalidLettersUsedException(string message) : base(message)
        {
        }

        public InvalidLettersUsedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidLettersUsedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
