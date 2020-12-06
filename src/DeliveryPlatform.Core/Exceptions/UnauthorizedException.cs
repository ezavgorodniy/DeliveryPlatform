using System;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryPlatform.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException) { }

    }
}
