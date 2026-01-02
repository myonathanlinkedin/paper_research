using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for tracking remediation actions.
    /// </summary>
    public interface IRemediationActionTracker
    {
        /// <summary>
        /// Tracks a remediation action execution.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        /// <param name="status">The execution status.</param>
        /// <param name="message">The message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task TrackExecutionAsync(IRemediationAction action, RemediationStatusEnum status, string message);

        /// <summary>
        /// Gets the execution history for a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The execution history.</returns>
        Task<IEnumerable<RemediationActionExecution>> GetExecutionHistoryAsync(string actionId);

        /// <summary>
        /// Gets the latest execution for a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The latest execution.</returns>
        Task<RemediationActionExecution> GetLatestExecutionAsync(string actionId);

        /// <summary>
        /// Gets all executions within a time range.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>All executions within the time range.</returns>
        Task<IEnumerable<RemediationActionExecution>> GetExecutionsInTimeRangeAsync(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Gets all executions by status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>All executions with the specified status.</returns>
        Task<IEnumerable<RemediationActionExecution>> GetExecutionsByStatusAsync(RemediationStatusEnum status);

        /// <summary>
        /// Tracks the completion of a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="isSuccessful">Whether the action was successful.</param>
        /// <param name="message">Optional message providing details about the completion.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task TrackActionCompletionAsync(string actionId, bool isSuccessful, string? message = null);
    }
} 