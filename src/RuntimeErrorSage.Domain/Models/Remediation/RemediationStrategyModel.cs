using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents a strategy for remediating an error.
    /// </summary>
    public class RemediationStrategyModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for this strategy.
        /// </summary>
        public string StrategyId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for this strategy (alias for StrategyId).
        /// </summary>
        public string Id
        {
            get => StrategyId;
            set => StrategyId = value;
        }

        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy version.
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// Gets or sets whether the strategy is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the strategy priority (lower is higher priority).
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the strategy.
        /// </summary>
        public RiskLevel RiskLevel { get; set; } = RiskLevel.Medium;

        /// <summary>
        /// Gets or sets the error patterns this strategy can handle.
        /// </summary>
        public List<ErrorPattern> HandledPatterns { get; set; } = new();

        /// <summary>
        /// Gets or sets the remediation steps.
        /// </summary>
        public List<RemediationStep> Steps { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        public List<ValidationRule> ValidationRules { get; set; } = new();

        /// <summary>
        /// Gets or sets the rollback steps.
        /// </summary>
        public List<RemediationStep> RollbackSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets when the strategy was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the strategy was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the strategy metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the success rate (0-1).
        /// </summary>
        public double SuccessRate { get; set; }

        /// <summary>
        /// Gets or sets the average execution time in seconds.
        /// </summary>
        public double AverageExecutionTimeSeconds { get; set; }

        /// <summary>
        /// Gets or sets the complexity of the strategy (0-1).
        /// </summary>
        public double? Complexity { get; set; }

        /// <summary>
        /// Gets or sets the system impact of the strategy (0-1).
        /// </summary>
        public double? SystemImpact { get; set; }

        /// <summary>
        /// Gets or sets whether the strategy can be rolled back.
        /// </summary>
        public bool IsRollbackable { get; set; }
    }
} 
