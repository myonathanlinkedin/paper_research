using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models.Validation
{
    /// <summary>
    /// Represents a validation warning.
    /// </summary>
    public class ValidationWarning
    {
        /// <summary>
        /// Gets or sets the warning identifier.
        /// </summary>
        public string WarningId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the warning code.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning source.
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning severity.
        /// </summary>
        public ValidationSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets additional warning details.
        /// </summary>
        public Dictionary<string, object> Details { get; set; } = new();
    }
} 
