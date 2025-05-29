using System;

namespace RuntimeErrorSage.Model.Health.Exceptions
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

