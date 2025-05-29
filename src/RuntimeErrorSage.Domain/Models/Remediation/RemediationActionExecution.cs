using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Application.Models.Error;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents the execution of a single remediation action.
    /// </summary>
    public class RemediationActionExecution
    {
        private readonly Dictionary<string, ExecutionResult> _executionResults = new();
        private readonly List<Action<ExecutionResult>> _executionHandlers = new();

        /// <summary>
        /// Gets or sets the unique identifier of the action execution.
        /// </summary>
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the ID of the action that was executed.
        /// </summary>
        public string ActionId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string ActionName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the execution.
        /// </summary>
        public ExecutionStatus Status { get; set; } = ExecutionStatus.Pending;

        /// <summary>
        /// Gets or sets the timestamp when the action started.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the action ended.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the action execution.
        /// </summary>
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;

        /// <summary>
        /// Gets or sets any error that occurred during execution.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the result of the action execution.
        /// </summary>
        public Dictionary<string, object> Result { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the action execution was successful.
        /// </summary>
        public bool IsCompleted => Status == ExecutionStatus.Completed || Status == ExecutionStatus.Failed;

        /// <summary>
        /// Gets or sets the duration of the action execution in milliseconds.
        /// </summary>
        public double DurationMs => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMilliseconds : 0;

        /// <summary>
        /// Gets or sets the validation results for this action execution.
        /// </summary>
        public List<ValidationResult> ValidationResults { get; set; } = new();

        /// <summary>
        /// Gets or sets the metrics collected during this action execution.
        /// </summary>
        public Dictionary<string, object> Metrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the parameters for this action execution.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets any warnings that occurred during action execution.
        /// </summary>
        public List<string> Warnings { get; set; } = new();

        /// <summary>
        /// Gets or sets the severity of this action execution.
        /// </summary>
        public string Severity { get; set; } = string.Empty;

        /// <summary>
        /// Executes a remediation action.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The execution result.</returns>
        public async Task<ExecutionResult> ExecuteActionAsync(IRemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            var result = new ExecutionResult
            {
                ActionId = action.ActionId,
                StartTime = DateTime.UtcNow,
                Status = ExecutionStatus.Running
            };

            try
            {
                // Check if action requires manual approval
                if (action.RequiresManualApproval)
                {
                    result.Status = ExecutionStatus.WaitingForApproval;
                    Status = ExecutionStatus.WaitingForApproval;
                    NotifyExecutionHandlers(result);
                    return result;
                }

                // Execute the action
                await action.ExecuteAsync(context);

                result.Status = ExecutionStatus.Completed;
                result.EndTime = DateTime.UtcNow;
                result.Success = true;
                Status = ExecutionStatus.Completed;
            }
            catch (Exception ex)
            {
                result.Status = ExecutionStatus.Failed;
                result.EndTime = DateTime.UtcNow;
                result.Success = false;
                result.Error = ex;
                Status = ExecutionStatus.Failed;
                Error = ex.Message;
            }

            EndTime = DateTime.UtcNow;
            _executionResults[action.ActionId] = result;
            NotifyExecutionHandlers(result);
            return result;
        }

        /// <summary>
        /// Gets the execution result for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The execution result, or null if not found.</returns>
        public ExecutionResult GetExecutionResult(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _executionResults.TryGetValue(actionId, out var result) ? result : null;
        }

        /// <summary>
        /// Registers a handler for execution results.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        public void RegisterExecutionHandler(Action<ExecutionResult> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _executionHandlers.Add(handler);
        }

        /// <summary>
        /// Unregisters a handler for execution results.
        /// </summary>
        /// <param name="handler">The handler to unregister.</param>
        public void UnregisterExecutionHandler(Action<ExecutionResult> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _executionHandlers.Remove(handler);
        }

        /// <summary>
        /// Clears all execution results.
        /// </summary>
        public void ClearResults()
        {
            _executionResults.Clear();
            Status = ExecutionStatus.Pending;
            StartTime = DateTime.UtcNow;
            EndTime = null;
            Error = null;
            Result.Clear();
            ValidationResults.Clear();
            Metrics.Clear();
            Parameters.Clear();
            Warnings.Clear();
        }

        private void NotifyExecutionHandlers(ExecutionResult result)
        {
            foreach (var handler in _executionHandlers)
            {
                try
                {
                    handler(result);
                }
                catch
                {
                    // Ignore handler exceptions
                }
            }
        }
    }
} 





