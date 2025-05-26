using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Metrics;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents the context of an error occurrence.
    /// </summary>
    public class ErrorContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for the error.
        /// </summary>
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the type of error.
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the source of the error.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the operation identifier.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the status of the error.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the severity of the error.
        /// </summary>
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the impact scope of the error.
        /// </summary>
        public ImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the impact severity of the error.
        /// </summary>
        public ImpactSeverity ImpactSeverity { get; set; }

        /// <summary>
        /// Gets or sets the analysis validation status.
        /// </summary>
        public AnalysisValidationStatus AnalysisValidationStatus { get; set; }

        /// <summary>
        /// Gets or sets additional metadata.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Defines the severity levels of errors.
    /// </summary>
    public enum ErrorSeverity
    {
        Critical,
        High,
        Medium,
        Low,
        Info
    }

    /// <summary>
    /// Defines the scope of error impact.
    /// </summary>
    public enum ImpactScope
    {
        Global,
        Service,
        Component,
        Operation,
        Local
    }

    /// <summary>
    /// Defines the severity of error impact.
    /// </summary>
    public enum ImpactSeverity
    {
        Critical,
        High,
        Medium,
        Low,
        None
    }

    /// <summary>
    /// Defines the validation status of error analysis.
    /// </summary>
    public enum AnalysisValidationStatus
    {
        Validated,
        Pending,
        Failed,
        Skipped,
        Unknown
    }
} 
