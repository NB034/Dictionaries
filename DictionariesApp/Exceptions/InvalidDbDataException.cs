using System;

namespace DictionariesApp.Exceptions
{
    internal class InvalidDbDataException : Exception
    {
        public InvalidDbDataException(string message) : base(message)
        {
        }
    }
}
