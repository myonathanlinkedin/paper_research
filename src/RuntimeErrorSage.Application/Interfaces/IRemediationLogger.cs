using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for remediation logging.
    /// </summary>
    /// <typeparam name="T">The type to use for the logger category.</typeparam>
    public interface IRemediationLogger<T>
    {
        /// <summary>
        /// Logs a message at the specified log level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The log message.</param>
        void Log(LogLevel logLevel, string message);

        /// <summary>
        /// Logs a message at the specified log level with exception details.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The log message.</param>
        void Log(LogLevel logLevel, Exception exception, string message);

        /// <summary>
        /// Logs a message at the Debug level.
        /// </summary>
        /// <param name="message">The log message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs a message at the Information level.
        /// </summary>
        /// <param name="message">The log message.</param>
        void Info(string message);

        /// <summary>
        /// Logs a message at the Warning level.
        /// </summary>
        /// <param name="message">The log message.</param>
        void Warn(string message);

        /// <summary>
        /// Logs a message at the Error level.
        /// </summary>
        /// <param name="message">The log message.</param>
        void Error(string message);

        /// <summary>
        /// Logs a message at the Error level with exception details.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The log message.</param>
        void Error(Exception exception, string message);

        /// <summary>
        /// Logs a message at the Critical level.
        /// </summary>
        /// <param name="message">The log message.</param>
        void Critical(string message);

        /// <summary>
        /// Logs a message at the Critical level with exception details.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The log message.</param>
        void Critical(Exception exception, string message);
    }
} 