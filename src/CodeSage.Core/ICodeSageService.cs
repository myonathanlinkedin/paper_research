using System;
using System.Threading.Tasks;

namespace CodeSage.Core
{
    /// <summary>
    /// Defines the core service interface for CodeSage runtime intelligence.
    /// </summary>
    public interface ICodeSageService
    {
        /// <summary>
        /// Processes an exception and generates an analysis result.
        /// </summary>
        /// <param name="exception">The exception to analyze</param>
        /// <param name="context">Additional context information</param>
        /// <returns>The analysis result containing explanations and suggestions</returns>
        Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context);

        /// <summary>
        /// Attempts to apply automated remediation based on the analysis result.
        /// </summary>
        /// <param name="analysisResult">The analysis result to act upon</param>
        /// <returns>The result of the remediation attempt</returns>
        Task<RemediationResult> ApplyRemediationAsync(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Enriches the error context with additional runtime information.
        /// </summary>
        /// <param name="context">The base error context</param>
        /// <returns>An enriched error context</returns>
        Task<ErrorContext> EnrichContextAsync(ErrorContext context);

        /// <summary>
        /// Registers a custom remediation strategy.
        /// </summary>
        /// <param name="strategy">The remediation strategy to register</param>
        void RegisterRemediationStrategy(IRemediationStrategy strategy);

        /// <summary>
        /// Configures the service with specific settings.
        /// </summary>
        /// <param name="options">The configuration options</param>
        void Configure(CodeSageOptions options);
    }

    /// <summary>
    /// Defines the interface for custom remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets the name of the remediation strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Determines if this strategy can handle the given analysis result.
        /// </summary>
        /// <param name="analysisResult">The analysis result to evaluate</param>
        /// <returns>True if the strategy can handle the result, false otherwise</returns>
        bool CanHandle(ErrorAnalysisResult analysisResult);

        /// <summary>
        /// Applies the remediation strategy.
        /// </summary>
        /// <param name="analysisResult">The analysis result to act upon</param>
        /// <returns>The result of the remediation attempt</returns>
        Task<RemediationResult> ApplyAsync(ErrorAnalysisResult analysisResult);
    }

    /// <summary>
    /// Configuration options for the CodeSage service.
    /// </summary>
    public class CodeSageOptions
    {
        /// <summary>
        /// Gets or sets whether to enable automatic remediation.
        /// </summary>
        public bool EnableAutoRemediation { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum number of retry attempts for failed operations.
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// Gets or sets the base delay for exponential backoff in milliseconds.
        /// </summary>
        public int RetryBaseDelayMs { get; set; } = 1000;

        /// <summary>
        /// Gets or sets whether to enable context enrichment.
        /// </summary>
        public bool EnableContextEnrichment { get; set; } = true;

        /// <summary>
        /// Gets or sets the severity level threshold for automatic remediation.
        /// </summary>
        public SeverityLevel AutoRemediationThreshold { get; set; } = SeverityLevel.Medium;

        /// <summary>
        /// Gets or sets the LM Studio API endpoint.
        /// </summary>
        public string? LMStudioEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the model name to use for inference.
        /// </summary>
        public string? ModelName { get; set; }

        /// <summary>
        /// Gets or sets the MCP endpoint for context distribution.
        /// </summary>
        public string? MCPEndpoint { get; set; }

        /// <summary>
        /// Gets or sets whether to enable audit logging.
        /// </summary>
        public bool EnableAuditLogging { get; set; } = true;

        /// <summary>
        /// Gets or sets the retention period for error context data in days.
        /// </summary>
        public int ContextRetentionDays { get; set; } = 30;
    }
} 