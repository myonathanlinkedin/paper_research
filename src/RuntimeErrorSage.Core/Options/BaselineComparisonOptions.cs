using System;

namespace RuntimeErrorSage.Core.Options
{
    /// <summary>
    /// Configuration options for baseline comparison.
    /// Required by research for evaluation.
    /// </summary>
    public class BaselineComparisonOptions
    {
        /// <summary>
        /// Gets or sets whether to compare with traditional error handling.
        /// Required by research (try-catch baseline).
        /// </summary>
        public bool CompareWithTraditionalHandling { get; set; }

        /// <summary>
        /// Gets or sets whether to compare with static analysis tools.
        /// Required by research (static analysis baseline).
        /// </summary>
        public bool CompareWithStaticAnalysis { get; set; }

        /// <summary>
        /// Gets or sets whether to compare with manual debugging process.
        /// Required by research (manual debugging baseline).
        /// </summary>
        public bool CompareWithManualDebugging { get; set; }

        /// <summary>
        /// Gets or sets whether to include metrics comparison.
        /// Required by research for evaluation.
        /// </summary>
        public bool IncludeMetricsComparison { get; set; }
    }
} 
