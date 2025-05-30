using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Domain.Models.Analysis
{
    /// <summary>
    /// Represents the result of an impact analysis operation.
    /// </summary>
    public class ImpactAnalysisResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for this analysis result.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the timestamp when the analysis was performed.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the list of directly affected components.
        /// </summary>
        public List<string> DirectlyAffectedComponents { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of indirectly affected components.
        /// </summary>
        public List<string> IndirectlyAffectedComponents { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of cascading effects.
        /// </summary>
        public List<CascadingEffect> CascadingEffects { get; set; } = new();

        /// <summary>
        /// Gets or sets the severity of the impact.
        /// </summary>
        public ImpactSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the scope of the impact.
        /// </summary>
        public ImpactScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the metrics for this analysis.
        /// </summary>
        public ImpactAnalysisMetrics Metrics { get; set; } = new();
    }

    /// <summary>
    /// Represents a cascading effect in an impact analysis.
    /// </summary>
    public class CascadingEffect
    {
        /// <summary>
        /// Gets or sets the source component of the effect.
        /// </summary>
        public string SourceComponent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target component of the effect.
        /// </summary>
        public string TargetComponent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of effect.
        /// </summary>
        public string EffectType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the severity of the effect.
        /// </summary>
        public ImpactSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the description of the effect.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents metrics for an impact analysis operation.
    /// </summary>
    public class ImpactAnalysisMetrics
    {
        /// <summary>
        /// Gets or sets the total number of affected components.
        /// </summary>
        public int TotalAffectedComponents { get; set; }

        /// <summary>
        /// Gets or sets the number of direct effects.
        /// </summary>
        public int DirectEffectCount { get; set; }

        /// <summary>
        /// Gets or sets the number of indirect effects.
        /// </summary>
        public int IndirectEffectCount { get; set; }

        /// <summary>
        /// Gets or sets the execution time of the analysis in milliseconds.
        /// </summary>
        public double ExecutionTimeMs { get; set; }
    }

    /// <summary>
    /// Represents the severity of an impact.
    /// </summary>
    public enum ImpactSeverity
    {
        /// <summary>
        /// The impact is critical.
        /// </summary>
        Critical,

        /// <summary>
        /// The impact is high.
        /// </summary>
        High,

        /// <summary>
        /// The impact is medium.
        /// </summary>
        Medium,

        /// <summary>
        /// The impact is low.
        /// </summary>
        Low,

        /// <summary>
        /// The impact is minimal.
        /// </summary>
        Minimal
    }

    /// <summary>
    /// Represents the scope of an impact.
    /// </summary>
    public enum ImpactScope
    {
        /// <summary>
        /// The impact is global.
        /// </summary>
        Global,

        /// <summary>
        /// The impact is regional.
        /// </summary>
        Regional,

        /// <summary>
        /// The impact is local.
        /// </summary>
        Local,

        /// <summary>
        /// The impact is isolated.
        /// </summary>
        Isolated
    }
} 
