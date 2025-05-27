using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Enums;
using RemediationSeverity = RuntimeErrorSage.Models.Enums.RemediationSeverity;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the context for a remediation action.
    /// </summary>
    public class RemediationActionContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for the context.
        /// </summary>
        public Guid ContextId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the context.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the context.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of context data.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the list of tags.
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the creation timestamp.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update timestamp.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the version of the context.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the source of the context.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the priority of the context.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets whether the context is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the context is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets whether the context is optional.
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Gets or sets whether the context is conditional.
        /// </summary>
        public bool IsConditional { get; set; }

        /// <summary>
        /// Gets or sets the condition for the context.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the list of dependencies.
        /// </summary>
        public List<string> Dependencies { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of prerequisites.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the severity of the context.
        /// </summary>
        public RemediationSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the risk level of the context.
        /// </summary>
        public RemediationRiskLevel RiskLevel { get; set; }

        /// <summary>
        /// Gets or sets the impact scope of the context.
        /// </summary>
        public RemediationActionImpactScope ImpactScope { get; set; }

        /// <summary>
        /// Gets or sets the list of warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the strategy name.
        /// </summary>
        public string StrategyName { get; set; }

        /// <summary>
        /// Gets or sets the order of the context.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets whether the context is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the context is parallel.
        /// </summary>
        public bool IsParallel { get; set; }

        /// <summary>
        /// Gets or sets whether the context is sequential.
        /// </summary>
        public bool IsSequential { get; set; }

        /// <summary>
        /// Gets or sets whether the context is asynchronous.
        /// </summary>
        public bool IsAsynchronous { get; set; }

        /// <summary>
        /// Gets or sets whether the context is synchronous.
        /// </summary>
        public bool IsSynchronous { get; set; }

        /// <summary>
        /// Gets or sets whether the context is idempotent.
        /// </summary>
        public bool IsIdempotent { get; set; }

        /// <summary>
        /// Gets or sets whether the context is reversible.
        /// </summary>
        public bool IsReversible { get; set; }

        /// <summary>
        /// Gets or sets whether the context is atomic.
        /// </summary>
        public bool IsAtomic { get; set; }

        /// <summary>
        /// Gets or sets whether the context is transactional.
        /// </summary>
        public bool IsTransactional { get; set; }
    }
} 