using System;

namespace CodeSage.Core.Health.Exceptions
{
    public class HealthCheckException : Exception
    {
        public HealthCheckException(string message, Exception inner) : base(message, inner) { }
    }
} 