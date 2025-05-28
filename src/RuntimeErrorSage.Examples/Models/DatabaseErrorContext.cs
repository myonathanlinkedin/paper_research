using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Examples.Models
{
    /// <summary>
    /// Represents the context of a database error.
    /// </summary>
    public class DatabaseErrorContext
    {
        /// <summary>
        /// Gets or sets the name of the service where the error occurred.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the operation that caused the error.
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// Gets or sets the correlation ID for tracking the error.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the exception that was thrown.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the SQL query that caused the error.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the parameters used in the query.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the timeout duration for the operation.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
} 
