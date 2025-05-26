using System;

namespace RuntimeErrorSage.Core.Health.Exceptions
{
    public class HealthCheckException : Exception
    {
        public HealthCheckException(string message, Exception inner) : base(message, inner) { }
    }
} 
