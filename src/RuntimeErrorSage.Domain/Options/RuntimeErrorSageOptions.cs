using System;
using RuntimeErrorSage.Application.LLM.Options;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Options
{
    /// <summary>
    /// Configuration options for the RuntimeErrorSage runtime intelligence service.
    /// Implements minimum research scope requirements with additional features.
    /// </summary>
    public class RuntimeErrorSageOptions
    {
        /// <summary>
        /// Gets or sets the LM Studio configuration options.
        /// Required for core research scope.
        /// </summary>
        public LMStudioOptions LMStudio { get; set; } = new LMStudioOptions();

        /// <summary>
        /// Gets or sets whether error analysis is enabled.
        /// Required for core research scope.
        /// </summary>
        public bool EnableErrorAnalysis { get; set; } = true;

        /// <summary>
        /// Gets or sets whether pattern recognition is enabled.
        /// Additional feature beyond research scope.
        /// </summary>
        public bool EnablePatternRecognition { get; set; } = true;

        /// <summary>
        /// Gets or sets whether automated remediation is enabled.
        /// Required for core research scope (simple remediation).
        /// </summary>
        public bool EnableRemediation { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum number of concurrent error analyses.
        /// Additional feature for performance optimization.
        /// </summary>
        public int MaxConcurrentAnalysis { get; set; } = 10;

        /// <summary>
        /// Gets or sets the timeout for error analysis operations.
        /// Required for research performance criteria (500ms for 95% of requests).
        /// </summary>
        public TimeSpan AnalysisTimeout { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Gets or sets whether context enrichment is enabled.
        /// Required for basic error context collection in research scope.
        /// </summary>
        public bool EnableContextEnrichment { get; set; } = true;

        /// <summary>
        /// Gets or sets whether audit logging is enabled.
        /// Required for research validation and metrics collection.
        /// </summary>
        public bool EnableAuditLogging { get; set; } = true;

        /// <summary>
        /// Gets or sets whether performance monitoring is enabled.
        /// Required for research success criteria validation.
        /// </summary>
        public bool EnablePerformanceMonitoring { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum memory usage for the LLM component in megabytes.
        /// Required for research success criteria (under 100MB).
        /// </summary>
        public int MaxLLMMemoryMB { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum CPU usage percentage during error analysis.
        /// Required for research success criteria (under 10%).
        /// </summary>
        public double MaxCPUUsagePercent { get; set; } = 10.0;

        /// <summary>
        /// Gets or sets the minimum accuracy threshold for error root cause identification.
        /// Required for research success criteria (at least 80%).
        /// </summary>
        public double MinRootCauseAccuracy { get; set; } = 0.8;

        /// <summary>
        /// Gets or sets the minimum accuracy threshold for remediation suggestions.
        /// Required for research success criteria (at least 70%).
        /// </summary>
        public double MinRemediationAccuracy { get; set; } = 0.7;

        /// <summary>
        /// Gets or sets the minimum severity level required for automatic remediation.
        /// Additional feature for remediation control.
        /// </summary>
        public ErrorSeverity AutoRemediationThreshold { get; set; } = ErrorSeverity.Error;

        /// <summary>
        /// Gets or sets the MCP endpoint URL for pattern sharing and collaboration.
        /// Additional feature for distributed context sharing.
        /// </summary>
        public string? MCPEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the supported error types.
        /// Required types by research: Database, FileSystem, HttpClient, Resource.
        /// Additional types may be supported beyond research scope.
        /// </summary>
        public string[] SupportedErrorTypes { get; set; } = new[]
        {
            // Required by research scope
            "Database",
            "FileSystem",
            "HttpClient",
            "Resource",
            // Additional types beyond research scope
            "Validation",
            "Authentication",
            "Authorization",
            "Configuration"
        };

        /// <summary>
        /// Gets or sets the test suite configuration.
        /// Required by research for validation.
        /// </summary>
        public TestSuiteOptions TestSuite { get; set; } = new()
        {
            // Required by research: 100 standardized scenarios
            StandardizedScenariosCount = 100,
            // Required by research: 20 real-world cases
            RealWorldCasesCount = 20,
            // Required by research: Complete test coverage
            RequireFullCoverage = true,
            // Required by research: Performance benchmark suite
            IncludePerformanceBenchmarks = true,
            // Required by research: Memory usage analysis
            IncludeMemoryAnalysis = true
        };

        /// <summary>
        /// Gets or sets the baseline comparison configuration.
        /// Required by research for evaluation.
        /// </summary>
        public BaselineComparisonOptions BaselineComparison { get; set; } = new()
        {
            // Required by research: Compare with traditional error handling
            CompareWithTraditionalHandling = true,
            // Required by research: Compare with static analysis
            CompareWithStaticAnalysis = true,
            // Required by research: Compare with manual debugging
            CompareWithManualDebugging = true,
            // Required by research: Include metrics comparison
            IncludeMetricsComparison = true
        };

        /// <summary>
        /// Gets or sets the API documentation configuration.
        /// Required by research for implementation completeness.
        /// </summary>
        public ApiDocumentationOptions ApiDocumentation { get; set; } = new()
        {
            // Required by research: Documented API and integration patterns
            GenerateApiDocs = true,
            GenerateIntegrationPatterns = true,
            IncludeCodeExamples = true,
            IncludePerformanceGuidelines = true
        };

        /// <summary>
        /// Gets or sets the severity level threshold for remediation.
        /// </summary>
        public ErrorSeverity RemediationSeverityThreshold { get; set; } = ErrorSeverity.Error;

        public bool EnableLLMAnalysis { get; set; } = true;
        public bool EnableGraphAnalysis { get; set; } = true;
        public bool EnableValidation { get; set; } = true;
        public bool EnableMetrics { get; set; } = true;
        public bool EnableCaching { get; set; } = true;
        public int CacheExpirationMinutes { get; set; } = 30;
        public int LLMTimeoutSeconds { get; set; } = 30;
        public int ValidationTimeoutSeconds { get; set; } = 10;
        public int GraphAnalysisTimeoutSeconds { get; set; } = 30;
        public string LLMModel { get; set; } = "Qwen-2.5-7B-Instruct-1M";
        public string LLMEndpoint { get; set; } = "https://api.example.com/llm";
        public string ValidationEndpoint { get; set; } = "https://api.example.com/validation";
        public string GraphAnalysisEndpoint { get; set; } = "https://api.example.com/graph";
        public string RemediationEndpoint { get; set; } = "https://api.example.com/remediation";
        public string MetricsEndpoint { get; set; } = "https://api.example.com/metrics";
        public Dictionary<string, string> AdditionalSettings { get; set; } = new();

        /// <summary>
        /// Gets or sets whether auto-remediation is enabled.
        /// </summary>
        public bool EnableAutoRemediation { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of remediation attempts.
        /// </summary>
        public int MaxRemediationAttempts { get; set; } = 3;

        /// <summary>
        /// Gets or sets whether to enable verbose logging.
        /// </summary>
        public bool EnableVerboseLogging { get; set; }

        /// <summary>
        /// Gets or sets whether to enable telemetry.
        /// </summary>
        public bool EnableTelemetry { get; set; } = true;

        /// <summary>
        /// Gets or sets the timeout for remediation operations in seconds.
        /// </summary>
        public int RemediationTimeoutSeconds { get; set; } = 300;
    }
} 

