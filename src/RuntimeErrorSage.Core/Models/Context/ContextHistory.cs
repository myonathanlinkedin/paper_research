using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;

namespace RuntimeErrorSage.Core.Models.Context
{
    /// <summary>
    /// Represents the history of error contexts over time.
    /// </summary>
    public class ContextHistory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the context history.
        /// </summary>
        public string HistoryId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the correlation identifier for tracking related operations.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service name associated with this history.
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the operation name associated with this history.
        /// </summary>
        public string OperationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time range covered by this history.
        /// </summary>
        public TimeRange TimeRange { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of error contexts in chronological order.
        /// </summary>
        public List<ErrorContext> Contexts { get; set; } = new();

        /// <summary>
        /// Gets or sets the connection status history.
        /// </summary>
        public List<ConnectionStatus> ConnectionStatusHistory { get; set; } = new();

        /// <summary>
        /// Gets or sets any additional metadata about the history.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the total number of errors in the history.
        /// </summary>
        public int TotalErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the number of critical errors in the history.
        /// </summary>
        public int CriticalErrorCount { get; set; }

        /// <summary>
        /// Gets or sets the average error severity in the history.
        /// </summary>
        public double AverageErrorSeverity { get; set; }

        /// <summary>
        /// Gets or sets the most common error type in the history.
        /// </summary>
        public string MostCommonErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the most recent error context.
        /// </summary>
        public ErrorContext? MostRecentContext { get; set; }

        /// <summary>
        /// Gets or sets whether the history has been analyzed.
        /// </summary>
        public bool IsAnalyzed { get; set; }

        /// <summary>
        /// Gets or sets the analysis timestamp.
        /// </summary>
        public DateTime? AnalysisTimestamp { get; set; }

        /// <summary>
        /// Gets or sets any analysis insights.
        /// </summary>
        public Dictionary<string, object> AnalysisInsights { get; set; } = new();

        /// <summary>
        /// Adds a new error context to the history.
        /// </summary>
        /// <param name="context">The error context to add.</param>
        public void AddContext(ErrorContext context)
        {
            Contexts.Add(context);
            UpdateStatistics();
        }

        /// <summary>
        /// Updates the connection status in the history.
        /// </summary>
        /// <param name="status">The new connection status.</param>
        public void UpdateConnectionStatus(ConnectionStatus status)
        {
            ConnectionStatusHistory.Add(status);
        }

        private void UpdateStatistics()
        {
            if (Contexts.Count == 0)
                return;

            TotalErrorCount = Contexts.Count;
            CriticalErrorCount = Contexts.Count(c => c.Severity == ErrorSeverity.Critical || c.Severity == ErrorSeverity.Fatal);
            AverageErrorSeverity = Contexts.Average(c => (int)c.Severity);
            MostCommonErrorType = Contexts
                .GroupBy(c => c.ErrorType)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? string.Empty;
            MostRecentContext = Contexts.OrderByDescending(c => c.Timestamp).FirstOrDefault();
        }
    }
} 