using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Options
{
    /// <summary>
    /// Configuration options for the test suite.
    /// Required by research for validation.
    /// </summary>
    public class TestSuiteOptions
    {
        /// <summary>
        /// Gets or sets the number of standardized error scenarios.
        /// Required by research (100 scenarios).
        /// </summary>
        public int StandardizedScenariosCount { get; }

        /// <summary>
        /// Gets or sets the number of real-world error cases.
        /// Required by research (20 cases).
        /// </summary>
        public int RealWorldCasesCount { get; }

        /// <summary>
        /// Gets or sets whether full test coverage is required.
        /// Required by research for core components.
        /// </summary>
        public bool RequireFullCoverage { get; }

        /// <summary>
        /// Gets or sets whether to include performance benchmarks.
        /// Required by research for evaluation.
        /// </summary>
        public bool IncludePerformanceBenchmarks { get; }

        /// <summary>
        /// Gets or sets whether to include memory usage analysis.
        /// Required by research for evaluation.
        /// </summary>
        public bool IncludeMemoryAnalysis { get; }
    }
} 





