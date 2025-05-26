using System;

namespace RuntimeErrorSage.Core.Models
{
    /// <summary>
    /// Represents the results of baseline comparison.
    /// Required by research for evaluation.
    /// </summary>
    public class BaselineComparisonResults
    {
        /// <summary>
        /// Gets or sets the traditional error handling metrics.
        /// Required by research (try-catch baseline).
        /// </summary>
        public ErrorHandlingMetrics TraditionalHandling { get; set; } = new();

        /// <summary>
        /// Gets or sets the static analysis metrics.
        /// Required by research (static analysis baseline).
        /// </summary>
        public ErrorHandlingMetrics StaticAnalysis { get; set; } = new();

        /// <summary>
        /// Gets or sets the manual debugging metrics.
        /// Required by research (manual debugging baseline).
        /// </summary>
        public ErrorHandlingMetrics ManualDebugging { get; set; } = new();
    }
} 
