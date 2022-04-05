using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WordsGameCore.Exceptions
{
    public class GameIsNotInProgressException : Exception
    {
        public GameIsNotInProgressException()
        {
        }

        public GameIsNotInProgressException(string message) : base(message)
        {
        }

        public GameIsNotInProgressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameIsNotInProgressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
