using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a remediation action that can be executed to fix an error.
    /// </summary>
    public class RemediationAction : IRemediationAction
    {
        private readonly RemediationActionCore _core;
        private readonly RemediationActionValidation _validation;
        private readonly RemediationActionExecution _execution;
        private readonly RemediationActionResult _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationAction"/> class.
        /// </summary>
        public RemediationAction()
        {
            _core = new RemediationActionCore();
            _validation = new RemediationActionValidation();
            _execution = new RemediationActionExecution();
            _result = new RemediationActionResult();
        }

        /// <summary>
        /// Gets or sets the unique identifier for the action.
        /// </summary>
        public string ActionId
        {
            get => _core.ActionId;
            set => _core.ActionId = value;
        }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public string ActionType
        {
            get => _core.ActionType;
            set => _core.ActionType = value;
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name
        {
            get => _core.Name;
            set => _core.Name = value;
        }

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description
        {
            get => _core.Description;
            set => _core.Description = value;
        }

        /// <summary>
        /// Gets or sets the context in which the action should be executed.
        /// </summary>
        public ErrorContext Context
        {
            get => _core.Context;
            set => _core.Context = value;
        }

        /// <summary>
        /// Gets or sets the priority of the action.
        /// </summary>
        public int Priority
        {
            get => _core.Priority;
            set => _core.Priority = value;
        }

        /// <summary>
        /// Gets or sets the impact level of the action.
        /// </summary>
        public ImpactLevel Impact
        {
            get => _core.Impact;
            set => _core.Impact = value;
        }

        /// <summary>
        /// Gets or sets the risk level of the action.
        /// </summary>
        public RiskLevel RiskLevel
        {
            get => _core.RiskLevel;
            set => _core.RiskLevel = value;
        }

        /// <summary>
        /// Gets or sets the status of the action.
        /// </summary>
        public RemediationStatusEnum Status
        {
            get => _core.Status;
            set => _core.Status = value;
        }

        /// <summary>
        /// Gets or sets whether the action requires manual approval.
        /// </summary>
        public bool RequiresManualApproval
        {
            get => _core.RequiresManualApproval;
            set => _core.RequiresManualApproval = value;
        }

        /// <summary>
        /// Gets or sets the prerequisites for the action.
        /// </summary>
        public Collection<string> Prerequisites
        {
            get => _core.Prerequisites;
            set => _core.Prerequisites = value;
        }

        /// <summary>
        /// Gets or sets the dependencies for the action.
        /// </summary>
        public Collection<string> Dependencies
        {
            get => _core.Dependencies;
            set => _core.Dependencies = value;
        }

        /// <summary>
        /// Gets or sets the parameters for the action.
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get => _core.Parameters;
            set => _core.Parameters = value;
        }

        /// <summary>
        /// Gets or sets the metadata for the action.
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get => _core.Metadata;
            set => _core.Metadata = value;
        }

        /// <summary>
        /// Gets or sets the tags for the action.
        /// </summary>
        public Collection<string> Tags
        {
            get => _core.Tags;
            set => _core.Tags = value;
        }

        /// <summary>
        /// Gets or sets the version of the action.
        /// </summary>
        public string Version
        {
            get => _core.Version;
            set => _core.Version = value;
        }

        /// <summary>
        /// Gets or sets the author of the action.
        /// </summary>
        public string Author
        {
            get => _core.Author;
            set => _core.Author = value;
        }

        /// <summary>
        /// Gets or sets the creation date of the action.
        /// </summary>
        public DateTime CreatedDate
        {
            get => _core.CreatedDate;
            set => _core.CreatedDate = value;
        }

        /// <summary>
        /// Gets or sets the last modified date of the action.
        /// </summary>
        public DateTime LastModifiedDate
        {
            get => _core.LastModifiedDate;
            set => _core.LastModifiedDate = value;
        }

        /// <summary>
        /// Gets or sets the execution timeout in milliseconds.
        /// </summary>
        public int ExecutionTimeoutMs
        {
            get => _core.ExecutionTimeoutMs;
            set => _core.ExecutionTimeoutMs = value;
        }

        /// <summary>
        /// Gets or sets the number of retries.
        /// </summary>
        public int RetryCount
        {
            get => _core.RetryCount;
            set => _core.RetryCount = value;
        }

        /// <summary>
        /// Gets or sets the retry delay in milliseconds.
        /// </summary>
        public int RetryDelayMs
        {
            get => _core.RetryDelayMs;
            set => _core.RetryDelayMs = value;
        }

        /// <summary>
        /// Gets or sets whether the action is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => _core.IsEnabled;
            set => _core.IsEnabled = value;
        }

        /// <summary>
        /// Gets or sets whether the action is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _core.IsVisible;
            set => _core.IsVisible = value;
        }

        /// <summary>
        /// Gets or sets the category of the action.
        /// </summary>
        public string Category
        {
            get => _core.Category;
            set => _core.Category = value;
        }

        /// <summary>
        /// Gets or sets the subcategory of the action.
        /// </summary>
        public string Subcategory
        {
            get => _core.Subcategory;
            set => _core.Subcategory = value;
        }

        /// <summary>
        /// Gets or sets the severity of the action.
        /// </summary>
        public string Severity
        {
            get => _core.Severity;
            set => _core.Severity = value;
        }

        /// <summary>
        /// Gets or sets the complexity of the action.
        /// </summary>
        public string Complexity
        {
            get => _core.Complexity;
            set => _core.Complexity = value;
        }

        /// <summary>
        /// Gets or sets the estimated duration of the action.
        /// </summary>
        public TimeSpan EstimatedDuration
        {
            get => _core.EstimatedDuration;
            set => _core.EstimatedDuration = value;
        }

        /// <summary>
        /// Gets or sets the actual duration of the action.
        /// </summary>
        public TimeSpan? ActualDuration
        {
            get => _core.ActualDuration;
            set => _core.ActualDuration = value;
        }

        /// <summary>
        /// Gets or sets the start time of the action.
        /// </summary>
        public DateTime? StartTime
        {
            get => _core.StartTime;
            set => _core.StartTime = value;
        }

        /// <summary>
        /// Gets or sets the end time of the action.
        /// </summary>
        public DateTime? EndTime
        {
            get => _core.EndTime;
            set => _core.EndTime = value;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _core.ErrorMessage;
            set => _core.ErrorMessage = value;
        }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace
        {
            get => _core.StackTrace;
            set => _core.StackTrace = value;
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        public string Output
        {
            get => _core.Output;
            set => _core.Output = value;
        }

        /// <summary>
        /// Gets or sets the validation results.
        /// </summary>
        public Collection<ValidationResult> ValidationResults
        {
            get => _core.ValidationResults;
            set => _core.ValidationResults = value;
        }

        /// <summary>
        /// Gets or sets the rollback status.
        /// </summary>
        public RollbackStatus? RollbackStatus
        {
            get => _core.RollbackStatus;
            set => _core.RollbackStatus = value;
        }

        /// <summary>
        /// Gets or sets whether the action can be rolled back.
        /// </summary>
        public bool CanRollback
        {
            get => _core.CanRollback;
            set => _core.CanRollback = value;
        }

        /// <summary>
        /// Gets or sets the rollback action.
        /// </summary>
        public RemediationAction RollbackAction
        {
            get => _core.RollbackAction;
            set => _core.RollbackAction = value;
        }

        /// <summary>
        /// Validates the action.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        public async Task<ValidationResult> ValidateAsync(ErrorContext context)
        {
            return await _validation.ValidateAsync(context);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The remediation result.</returns>
        public async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            return await _execution.ExecuteAsync(context);
        }

        /// <summary>
        /// Rolls back the remediation action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a result indicating success or failure.</returns>
        public async Task<RemediationResult> RollbackAsync()
        {
            return await _execution.RollbackAsync();
        }

        /// <summary>
        /// Gets the estimated impact of this action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with the impact result.</returns>
        public async Task<RemediationImpact> GetEstimatedImpactAsync()
        {
            return await _execution.GetEstimatedImpactAsync();
        }

        /// <summary>
        /// Gets the result of the action.
        /// </summary>
        /// <returns>The action result.</returns>
        public ActionResult GetResult()
        {
            return _result.GetResult();
        }

        /// <summary>
        /// Registers a result handler.
        /// </summary>
        /// <param name="handler">The result handler.</param>
        public Action<ActionResult> handler { ArgumentNullException.ThrowIfNull(Action<ActionResult> handler); }
        {
            _result.RegisterHandler(handler);
        }

        /// <summary>
        /// Unregisters a result handler.
        /// </summary>
        /// <param name="handler">The result handler.</param>
        public Action<ActionResult> handler { ArgumentNullException.ThrowIfNull(Action<ActionResult> handler); }
        {
            _result.UnregisterHandler(handler);
        }

        /// <summary>
        /// Registers an execution handler.
        /// </summary>
        /// <param name="handler">The execution handler.</param>
        public Action<ExecutionResult> handler { ArgumentNullException.ThrowIfNull(Action<ExecutionResult> handler); }
        {
            _execution.RegisterHandler(handler);
        }

        /// <summary>
        /// Unregisters an execution handler.
        /// </summary>
        /// <param name="handler">The execution handler.</param>
        public Action<ExecutionResult> handler { ArgumentNullException.ThrowIfNull(Action<ExecutionResult> handler); }
        {
            _execution.UnregisterHandler(handler);
        }

        /// <summary>
        /// Adds a validation rule.
        /// </summary>
        /// <param name="rule">The validation rule.</param>
        public ValidationRule rule { ArgumentNullException.ThrowIfNull(ValidationRule rule); }
        {
            _validation.AddRule(rule);
        }

        /// <summary>
        /// Removes a validation rule.
        /// </summary>
        /// <param name="rule">The validation rule.</param>
        public ValidationRule rule { ArgumentNullException.ThrowIfNull(ValidationRule rule); }
        {
            _validation.RemoveRule(rule);
        }

        /// <summary>
        /// Gets the validation rules.
        /// </summary>
        /// <returns>The validation rules.</returns>
        public IReadOnlyCollection<ValidationRule> GetValidationRules()
        {
            return _validation.GetRules();
        }

        /// <summary>
        /// Clears the results.
        /// </summary>
        public void ClearResults()
        {
            _result.Clear();
        }
    }
} 







