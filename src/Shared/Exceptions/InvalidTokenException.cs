using System;
using System.Diagnostics.CodeAnalysis;

namespace Shared.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message) : base(message) { }

        public InvalidTokenException(string message, Exception innerException): base(message, innerException) { }
    }
}
