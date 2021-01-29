using System;

namespace REBOOK.Exceptions
{
    public class InvalidObjectException : Exception
    {
        public InvalidObjectException()
        {
        }
        
        public InvalidObjectException(string message) : base(message)
        {
        }
        
        public InvalidObjectException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}