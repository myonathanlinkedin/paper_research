using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Infrastructure.Services
{
    /// <summary>
    /// Represents the context for a remediation process.
    /// </summary>
    public class RemediationContext : IRemediationContext
    {
        /// <summary>
        /// Gets the error analysis result.
        /// </summary>
        private ErrorAnalysisResult _analysisResult;
        public ErrorAnalysisResult AnalysisResult => _analysisResult;

        /// <summary>
        /// Gets the remediation plan.
        /// </summary>
        private RemediationPlan _plan;
        public RemediationPlan Plan => _plan;

        /// <summary>
        /// Gets the current remediation action.
        /// </summary>
        private IRemediationAction _currentAction;
        public IRemediationAction CurrentAction => _currentAction;

        /// <summary>
        /// Gets the remediation history.
        /// </summary>
        private List<RemediationResult> _history = new List<RemediationResult>();
        public List<RemediationResult> History => _history;

        /// <summary>
        /// Gets the context data.
        /// </summary>
        public Dictionary<string, object> ContextData { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets a value indicating whether the remediation is in rollback mode.
        /// </summary>
        public bool IsRollbackMode { get; set; }

        /// <summary>
        /// Gets the current remediation state.
        /// </summary>
        private Domain.Models.Remediation.RemediationState _state;
        public Domain.Models.Remediation.RemediationState State => _state;

        /// <summary>
        /// Gets or sets the current remediation strategy.
        /// </summary>
        public RemediationStrategyModel CurrentStrategy { get; set; }

        /// <summary>
        /// Gets or sets the current remediation step.
        /// </summary>
        public RemediationStep CurrentStep { get; set; }

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        public RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the remediation metrics.
        /// </summary>
        public RemediationMetrics Metrics { get; set; }

        /// <summary>
        /// Gets or sets the remediation validation rules.
        /// </summary>
        public List<RemediationValidationRule> ValidationRules { get; set; } = new List<RemediationValidationRule>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationContext"/> class.
        /// </summary>
        /// <param name="analysisResult">The error analysis result.</param>
        /// <param name="plan">The remediation plan.</param>
        public RemediationContext(ErrorAnalysisResult analysisResult, RemediationPlan plan)
        {
            _analysisResult = analysisResult ?? throw new ArgumentNullException(nameof(analysisResult));
            _plan = plan ?? throw new ArgumentNullException(nameof(plan));
            _history = new List<RemediationResult>();
            ContextData = new Dictionary<string, object>();
            _state = new Domain.Models.Remediation.RemediationState
            {
                CurrentState = RemediationStateEnum.NotStarted
            };
        }

        /// <summary>
        /// Updates the context with a new remediation result.
        /// </summary>
        /// <param name="result">The remediation result.</param>
        public void UpdateContext(RemediationResult result)
        {
            ArgumentNullException.ThrowIfNull(result);

            _history.Add(result);
            _state = new Domain.Models.Remediation.RemediationState
            {
                CurrentState = result.IsSuccessful ? RemediationStateEnum.Completed : RemediationStateEnum.Failed
            };
        }

        /// <summary>
        /// Clears the context data.
        /// </summary>
        public void ClearContext()
        {
            ContextData.Clear();
            _currentAction = null;
            _state = new Domain.Models.Remediation.RemediationState
            {
                CurrentState = RemediationStateEnum.NotStarted
            };
        }

        /// <summary>
        /// Sets the current action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        public void SetCurrentAction(IRemediationAction action)
        {
            ArgumentNullException.ThrowIfNull(action);
            
            _currentAction = action;
            _state = new Domain.Models.Remediation.RemediationState
            {
                CurrentState = RemediationStateEnum.InProgress
            };
        }
    }
} 
