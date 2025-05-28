using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the context for a remediation process.
    /// </summary>
    public class RemediationContext : IRemediationContext
    {
        /// <summary>
        /// Gets the error analysis result.
        /// </summary>
        public ErrorAnalysisResult AnalysisResult { get; }

        /// <summary>
        /// Gets the remediation plan.
        /// </summary>
        public RemediationPlan Plan { get; }

        /// <summary>
        /// Gets the current remediation action.
        /// </summary>
        public IRemediationAction CurrentAction { get; private set; }

        /// <summary>
        /// Gets the remediation history.
        /// </summary>
        public List<RemediationResult> History { get; }

        /// <summary>
        /// Gets the context data.
        /// </summary>
        public Dictionary<string, object> ContextData { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the remediation is in rollback mode.
        /// </summary>
        public bool IsRollbackMode { get; set; }

        /// <summary>
        /// Gets the current remediation state.
        /// </summary>
        public RemediationState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationContext"/> class.
        /// </summary>
        /// <param name="analysisResult">The error analysis result.</param>
        /// <param name="plan">The remediation plan.</param>
        public RemediationContext(ErrorAnalysisResult analysisResult, RemediationPlan plan)
        {
            AnalysisResult = analysisResult ?? throw new ArgumentNullException(nameof(analysisResult));
            Plan = plan ?? throw new ArgumentNullException(nameof(plan));
            History = new List<RemediationResult>();
            ContextData = new Dictionary<string, object>();
            State = RemediationState.NotStarted;
        }

        /// <summary>
        /// Updates the context with a new remediation result.
        /// </summary>
        /// <param name="result">The remediation result.</param>
        public void UpdateContext(RemediationResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            History.Add(result);
            State = result.IsSuccessful ? RemediationState.Completed : RemediationState.Failed;
        }

        /// <summary>
        /// Clears the context data.
        /// </summary>
        public void ClearContext()
        {
            ContextData.Clear();
            CurrentAction = null;
            State = RemediationState.NotStarted;
        }

        /// <summary>
        /// Sets the current action.
        /// </summary>
        /// <param name="action">The remediation action.</param>
        public void SetCurrentAction(IRemediationAction action)
        {
            CurrentAction = action ?? throw new ArgumentNullException(nameof(action));
            State = RemediationState.InProgress;
        }
    }
} 