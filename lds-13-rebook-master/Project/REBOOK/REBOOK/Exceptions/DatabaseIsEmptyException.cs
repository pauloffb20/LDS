using System;

namespace REBOOK.Exceptions
{
    public class DatabaseIsEmptyException : Exception
    {
        public DatabaseIsEmptyException(){}
        public DatabaseIsEmptyException(string message) : base(message){}
        public DatabaseIsEmptyException(string message, Exception inner) : base(message, inner){}
    }
}