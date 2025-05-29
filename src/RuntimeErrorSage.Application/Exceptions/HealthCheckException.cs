using System;

namespace RuntimeErrorSage.Application.Health.Exceptions
{
    public class HealthCheckException : Exception
    {
        public HealthCheckException(string message, Exception inner) : base(message, inner) { }
        public HealthCheckException()
        {
        }

        public HealthCheckException(string message) : base(message)
        {
        }
    }
} 

