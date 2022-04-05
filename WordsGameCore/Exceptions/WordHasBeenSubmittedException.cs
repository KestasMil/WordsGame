using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WordsGameCore.Exceptions
{
    public class WordHasBeenSubmittedException : Exception
    {
        public WordHasBeenSubmittedException()
        {
        }

        public WordHasBeenSubmittedException(string message) : base(message)
        {
        }

        public WordHasBeenSubmittedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WordHasBeenSubmittedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
