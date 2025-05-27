using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of a single remediation action.
    /// </summary>
    public class RemediationActionResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the action result.
        /// </summary>
        public string ActionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the action was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the start time of the action.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end time of the action.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the action.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the error message if the action failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the output of the action.
        /// </summary>
        public string Output { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the metadata associated with the action.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation results for the action.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus? RollbackStatus { get; set; }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Gets or sets the rollback action if available.
        /// </summary>
        public RemediationAction RollbackAction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationActionResult"/> class.
        /// </summary>
        public RemediationActionResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationActionResult"/> class.
        /// </summary>
        /// <param name="isSuccessful">Whether the action was successful.</param>
        public RemediationActionResult(bool isSuccessful)
        {
            Success = isSuccessful;
        }

        /// <summary>
        /// Sets the success state of this result.
        /// </summary>
        /// <param name="isSuccessful">Whether the action was successful.</param>
        public void SetSuccess(bool isSuccessful)
        {
            Success = isSuccessful;
        }

        /// <summary>
        /// Creates a successful action result.
        /// </summary>
        /// <returns>A successful action result.</returns>
        public static RemediationActionResult CreateSuccessResult()
        {
            return new RemediationActionResult
            {
                Success = true
            };
        }

        /// <summary>
        /// Creates a failed action result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A failed action result.</returns>
        public static RemediationActionResult Failure(string errorMessage)
        {
            return new RemediationActionResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
    }
} 