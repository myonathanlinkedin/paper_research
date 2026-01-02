using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Flow;

namespace RuntimeErrorSage.Domain.Models.Error
{
    /// <summary>
    /// Represents a database error context.
    /// </summary>
    public class DatabaseErrorContext : ErrorContext
    {
        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database server.
        /// </summary>
        public string Server { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database command.
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the database error code.
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database state.
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database connection string (masked).
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database version.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the database transaction ID.
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database operation type.
        /// </summary>
        public new string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database operation start time.
        /// </summary>
        public new DateTime OperationStartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the database operation duration.
        /// </summary>
        public new TimeSpan OperationDuration { get; set; }

        /// <summary>
        /// Gets or sets the database operation status.
        /// </summary>
        public new string OperationStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database operation result.
        /// </summary>
        public new string OperationResult { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database operation target.
        /// </summary>
        public new string OperationTarget { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database operation metrics.
        /// </summary>
        public new Dictionary<string, double> OperationMetrics { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// Gets or sets the database operation dependencies.
        /// </summary>
        public new List<string> OperationDependencies { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the database operation tags.
        /// </summary>
        public new List<string> OperationTags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the database service calls.
        /// </summary>
        public new List<ServiceCall> ServiceCalls { get; set; } = new List<ServiceCall>();

        /// <summary>
        /// Gets or sets the database data flows.
        /// </summary>
        public new List<DataFlow> DataFlows { get; set; } = new List<DataFlow>();

        /// <summary>
        /// Gets or sets the database component metrics.
        /// </summary>
        public new Dictionary<string, Dictionary<string, double>> ComponentMetrics { get; set; } = new Dictionary<string, Dictionary<string, double>>();

        /// <summary>
        /// Gets or sets the database component dependencies.
        /// </summary>
        public new List<ComponentDependency> ComponentDependencies { get; set; } = new List<ComponentDependency>();

        /// <summary>
        /// Gets or sets the database context data.
        /// </summary>
        public new Dictionary<string, object> ContextData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the database inner error.
        /// </summary>
        public new DatabaseErrorContext? InnerError { get; set; }

        /// <summary>
        /// Gets or sets the database service name.
        /// </summary>
        public new string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        public new string Context { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the database error is actionable.
        /// </summary>
        public new bool IsActionable { get; set; }

        /// <summary>
        /// Gets or sets whether the database error is transient.
        /// </summary>
        public new bool IsTransient { get; set; }

        /// <summary>
        /// Gets or sets whether the database error is resolved.
        /// </summary>
        public new bool IsResolved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseErrorContext"/> class.
        /// </summary>
        /// <param name="error">The runtime error.</param>
        /// <param name="context">The context information.</param>
        /// <param name="timestamp">The timestamp when the error occurred.</param>
        public DatabaseErrorContext(RuntimeError error, string context, DateTime timestamp) 
            : base(error, context, timestamp)
        {
            Category = Domain.Enums.ErrorCategory.Database;
        }

        /// <summary>
        /// Validates the database error context.
        /// </summary>
        /// <returns>True if the database error context is valid; otherwise, false.</returns>
        public new bool Validate()
        {
            if (!base.Validate())
                return false;

            if (string.IsNullOrEmpty(DatabaseName))
                return false;

            if (string.IsNullOrEmpty(Server))
                return false;

            if (string.IsNullOrEmpty(Command))
                return false;

            if (string.IsNullOrEmpty(ErrorCode))
                return false;

            if (string.IsNullOrEmpty(ErrorMessage))
                return false;

            if (string.IsNullOrEmpty(State))
                return false;

            if (string.IsNullOrEmpty(ConnectionString))
                return false;

            if (string.IsNullOrEmpty(Provider))
                return false;

            if (string.IsNullOrEmpty(Version))
                return false;

            if (string.IsNullOrEmpty(TransactionId))
                return false;

            if (string.IsNullOrEmpty(OperationType))
                return false;

            if (string.IsNullOrEmpty(OperationStatus))
                return false;

            if (string.IsNullOrEmpty(OperationResult))
                return false;

            if (string.IsNullOrEmpty(OperationTarget))
                return false;

            if (string.IsNullOrEmpty(ServiceName))
                return false;

            if (string.IsNullOrEmpty(Context))
                return false;

            return true;
        }

        /// <summary>
        /// Converts the database error context to a dictionary.
        /// </summary>
        /// <returns>The dictionary representation of the database error context.</returns>
        public new Dictionary<string, object> ToDictionary()
        {
            var dict = base.ToDictionary();

            dict.Add("DatabaseName", DatabaseName);
            dict.Add("Server", Server);
            dict.Add("Command", Command);
            dict.Add("Parameters", Parameters);
            dict.Add("ErrorCode", ErrorCode);
            dict.Add("ErrorMessage", ErrorMessage);
            dict.Add("State", State);
            dict.Add("ConnectionString", ConnectionString);
            dict.Add("Provider", Provider);
            dict.Add("Version", Version);
            dict.Add("Timeout", Timeout);
            dict.Add("TransactionId", TransactionId);
            dict.Add("OperationType", OperationType);
            dict.Add("OperationStartTime", OperationStartTime);
            dict.Add("OperationDuration", OperationDuration);
            dict.Add("OperationStatus", OperationStatus);
            dict.Add("OperationResult", OperationResult);
            dict.Add("OperationTarget", OperationTarget);
            dict.Add("OperationMetrics", OperationMetrics);
            dict.Add("OperationDependencies", OperationDependencies);
            dict.Add("OperationTags", OperationTags);
            dict.Add("ServiceCalls", ServiceCalls);
            dict.Add("DataFlows", DataFlows);
            dict.Add("ComponentMetrics", ComponentMetrics);
            dict.Add("ComponentDependencies", ComponentDependencies);
            dict.Add("ContextData", ContextData);
            dict.Add("ServiceName", ServiceName);
            dict.Add("Context", Context);
            dict.Add("IsActionable", IsActionable);
            dict.Add("IsTransient", IsTransient);
            dict.Add("IsResolved", IsResolved);

            if (InnerError != null)
            {
                dict.Add("InnerError", InnerError);
            }

            return dict;
        }
    }
} 
