using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a strategy for remediating an error.
    /// </summary>
    public class RemediationStrategyModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for this strategy.
        /// </summary>
        public string StrategyId { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy description.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the strategy version.
        /// </summary>
        public string Version { get; } = "1.0.0";

        /// <summary>
        /// Gets or sets whether the strategy is enabled.
        /// </summary>
        public bool IsEnabled { get; } = true;

        /// <summary>
        /// Gets or sets the strategy priority (lower is higher priority).
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Gets or sets the error patterns this strategy can handle.
        /// </summary>
        public IReadOnlyCollection<HandledPatterns> HandledPatterns { get; } = new();

        /// <summary>
        /// Gets or sets the remediation steps.
        /// </summary>
        public IReadOnlyCollection<Steps> Steps { get; } = new();

        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        public IReadOnlyCollection<ValidationRules> ValidationRules { get; } = new();

        /// <summary>
        /// Gets or sets the rollback steps.
        /// </summary>
        public IReadOnlyCollection<RollbackSteps> RollbackSteps { get; } = new();

        /// <summary>
        /// Gets or sets when the strategy was created.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Gets or sets when the strategy was last updated.
        /// </summary>
        public DateTime LastUpdatedAt { get; }

        /// <summary>
        /// Gets or sets the strategy metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the success rate (0-1).
        /// </summary>
        public double SuccessRate { get; }

        /// <summary>
        /// Gets or sets the average execution time in seconds.
        /// </summary>
        public double AverageExecutionTimeSeconds { get; }
    }
} 





