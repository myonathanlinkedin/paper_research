using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    /// <summary>
    /// Interface for remediation context in the Remediation namespace.
    /// </summary>
    public interface IRemediationContext
    {
        /// <summary>
        /// Gets the error analysis result.
        /// </summary>
        ErrorAnalysisResult AnalysisResult { get; }

        /// <summary>
        /// Gets the remediation plan.
        /// </summary>
        RemediationPlan Plan { get; }

        /// <summary>
        /// Gets the current remediation action.
        /// </summary>
        IRemediationAction CurrentAction { get; }

        /// <summary>
        /// Gets the remediation history.
        /// </summary>
        List<RemediationResult> History { get; }

        /// <summary>
        /// Gets the context data.
        /// </summary>
        Dictionary<string, object> ContextData { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the remediation is in rollback mode.
        /// </summary>
        bool IsRollbackMode { get; set; }

        /// <summary>
        /// Gets the current remediation state.
        /// </summary>
        RemediationState State { get; }

        /// <summary>
        /// Updates the context with a new remediation result.
        /// </summary>
        /// <param name="result">The remediation result.</param>
        void UpdateContext(RemediationResult result);

        /// <summary>
        /// Clears the context data.
        /// </summary>
        void ClearContext();
    }
} 
