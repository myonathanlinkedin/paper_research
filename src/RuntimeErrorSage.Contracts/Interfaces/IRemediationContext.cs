using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Contracts.Interfaces
{
    /// <summary>
    /// Interface for remediation context.
    /// </summary>
    public interface IRemediationContext
    {
        /// <summary>
        /// Gets the error analysis result.
        /// </summary>
        object AnalysisResult { get; }

        /// <summary>
        /// Gets the remediation plan.
        /// </summary>
        object Plan { get; }

        /// <summary>
        /// Gets the current remediation action.
        /// </summary>
        IRemediationAction CurrentAction { get; }

        /// <summary>
        /// Gets the remediation history.
        /// </summary>
        List<object> History { get; }

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
        int State { get; }
    }
} 