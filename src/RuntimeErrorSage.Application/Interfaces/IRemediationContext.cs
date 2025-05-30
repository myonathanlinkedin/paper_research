using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Interfaces
{
    /// <summary>
    /// Interface for remediation context.
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
        Domain.Enums.RemediationStateEnum State { get; }

        /// <summary>
        /// Gets or sets the current remediation strategy.
        /// </summary>
        RemediationStrategyModel CurrentStrategy { get; set; }

        /// <summary>
        /// Gets or sets the current remediation step.
        /// </summary>
        RemediationStep CurrentStep { get; set; }

        /// <summary>
        /// Gets or sets the remediation status.
        /// </summary>
        RemediationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the remediation metrics.
        /// </summary>
        RemediationMetrics Metrics { get; set; }

        /// <summary>
        /// Gets or sets the remediation validation rules.
        /// </summary>
        List<Domain.Models.Remediation.RemediationValidationRule> ValidationRules { get; set; }
    }
} 
