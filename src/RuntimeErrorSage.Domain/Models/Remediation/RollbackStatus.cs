using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents the status of a rollback operation.
    /// </summary>
    public class RollbackStatus
    {
        /// <summary>
        /// Gets or sets the action ID associated with the rollback.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the rollback.
        /// </summary>
        public RollbackStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the rollback.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the rollback.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the error message if the rollback failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets whether the rollback was successful.
        /// </summary>
        public bool Success => Status == RollbackStatusEnum.Completed;

        /// <summary>
        /// Converts a RemediationResult to a RollbackStatus
        /// </summary>
        public static implicit operator RollbackStatus(RemediationResult result)
        {
            if (result == null)
                return null;
                
            return new RollbackStatus
            {
                ActionId = result.ActionId,
                Status = MapStatus(result.Status),
                Message = result.Message,
                Timestamp = result.Timestamp,
                ErrorMessage = result.ErrorMessage
            };
        }
        
        private static RollbackStatusEnum MapStatus(RemediationStatusEnum status)
        {
            return status switch
            {
                RemediationStatusEnum.Success => RollbackStatusEnum.Completed,
                RemediationStatusEnum.Failed => RollbackStatusEnum.Failed,
                RemediationStatusEnum.InProgress => RollbackStatusEnum.InProgress,
                _ => RollbackStatusEnum.NotAttempted
            };
        }
    }
} 