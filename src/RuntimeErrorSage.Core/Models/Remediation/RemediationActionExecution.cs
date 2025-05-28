using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Error;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the execution of a single remediation action.
    /// </summary>
    public class RemediationActionExecution
    {
        private readonly Dictionary<string, object> _metadata = new();
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
        public RemediationStatusEnum Status { get; set; }

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
        public bool IsCompleted => Status == RemediationStatusEnum.Success || Status == RemediationStatusEnum.Failed;

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
        public RemediationActionSeverity Severity { get; set; } = RemediationActionSeverity.Medium;

        /// <summary>
        /// Gets or sets any additional metadata.
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata => _metadata;

        /// <summary>
        /// Adds metadata to the action execution.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public void AddMetadata(string key, object value)
        {
            _metadata[key] = value;
        }

        /// <summary>
        /// Completes the action execution.
        /// </summary>
        /// <param name="success">Whether the action was successful.</param>
        /// <param name="result">The result of the action.</param>
        /// <param name="error">Any error that occurred.</param>
        public void Complete(bool success, Dictionary<string, object> result = null, string? error = null)
        {
            EndTime = DateTime.UtcNow;
            Status = success ? RemediationStatusEnum.Success : RemediationStatusEnum.Failed;
            Result = result ?? new Dictionary<string, object>();
            Error = error;
        }

        public RemediationActionExecution(
            string actionId,
            string contextId,
            DateTime startTime,
            RemediationActionSeverity severity,
            Dictionary<string, object> parameters)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(contextId);

            ActionId = actionId;
            ContextId = contextId;
            StartTime = startTime;
            Severity = severity;
            Parameters = parameters ?? new Dictionary<string, object>();
            Status = RemediationStatusEnum.NotStarted;
        }

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
                    NotifyExecutionHandlers(result);
                    return result;
                }

                // Execute the action
                await action.ExecuteAsync(context);

                result.Status = ExecutionStatus.Completed;
                result.EndTime = DateTime.UtcNow;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Status = ExecutionStatus.Failed;
                result.EndTime = DateTime.UtcNow;
                result.Success = false;
                result.Error = ex;
            }

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

    /// <summary>
    /// Represents the result of an action execution.
    /// </summary>
    public class ExecutionResult
    {
        /// <summary>
        /// Gets or sets the ID of the action that was executed.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the execution.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the execution.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the execution.
        /// </summary>
        public ExecutionStatus Status { get; set; }

        /// <summary>
        /// Gets or sets whether the execution was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error that occurred during execution, if any.
        /// </summary>
        public Exception Error { get; set; }
    }

    /// <summary>
    /// Represents the status of an action execution.
    /// </summary>
    public enum ExecutionStatus
    {
        /// <summary>
        /// The action is waiting to be executed.
        /// </summary>
        Pending,

        /// <summary>
        /// The action is waiting for manual approval.
        /// </summary>
        WaitingForApproval,

        /// <summary>
        /// The action is currently running.
        /// </summary>
        Running,

        /// <summary>
        /// The action has completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The action has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The action has been cancelled.
        /// </summary>
        Cancelled
    }
} 


