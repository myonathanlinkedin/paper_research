using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Options;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Remediation.Interfaces;
using CodeSage.Core.Interfaces.MCP;

namespace CodeSage.Core
{
    /// <summary>
    /// Implements the core CodeSage service for runtime intelligence.
    /// </summary>
    public class CodeSageService : ICodeSageService
    {
        private readonly ILogger<CodeSageService> _logger;
        private readonly IConfiguration _configuration;
        private readonly List<IRemediationStrategy> _remediationStrategies;
        private readonly CodeSageOptions _options;
        private readonly ILMStudioClient _lmStudioClient;
        private readonly IMCPClient _mcpClient;
        private readonly IErrorAnalyzer _errorAnalyzer;
        private readonly IRemediationExecutor _remediationExecutor;
        private readonly IRemediationTracker _remediationTracker;
        private readonly ConcurrentDictionary<string, ErrorAnalysisResult> _analysisCache;
        private readonly ConcurrentDictionary<string, RemediationResult> _remediationCache;

        public CodeSageService(
            ILogger<CodeSageService> logger,
            IConfiguration configuration,
            IOptions<CodeSageOptions> options,
            ILMStudioClient lmStudioClient,
            IMCPClient mcpClient,
            IErrorAnalyzer errorAnalyzer,
            IRemediationExecutor remediationExecutor,
            IRemediationTracker remediationTracker)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _options = options.Value;
            _lmStudioClient = lmStudioClient ?? throw new ArgumentNullException(nameof(lmStudioClient));
            _mcpClient = mcpClient ?? throw new ArgumentNullException(nameof(mcpClient));
            _errorAnalyzer = errorAnalyzer;
            _remediationExecutor = remediationExecutor;
            _remediationTracker = remediationTracker;
            _remediationStrategies = new List<IRemediationStrategy>();
            _analysisCache = new ConcurrentDictionary<string, ErrorAnalysisResult>();
            _remediationCache = new ConcurrentDictionary<string, RemediationResult>();
        }

        public async Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context)
        {
            try
            {
                _logger.LogInformation("Processing exception: {ExceptionType}", exception.GetType().Name);

                // Enrich the context if enabled
                if (_options.EnableContextEnrichment)
                {
                    context = await EnrichContextAsync(context);
                }

                // Analyze error using the injected ErrorAnalyzer
                var analysisResult = await _errorAnalyzer.AnalyzeErrorAsync(exception, context);

                // Cache analysis result
                _analysisCache[analysisResult.CorrelationId] = analysisResult;

                // Publish context to MCP if configured
                if (!string.IsNullOrEmpty(_options.MCPEndpoint))
                {
                    await _mcpClient.PublishContextAsync(context, analysisResult);
                }

                // Log the analysis result
                if (_options.EnableAuditLogging)
                {
                    _logger.LogInformation(
                        "Error analysis completed. Severity: {Severity}, CanAutoRemediate: {CanAutoRemediate}",
                        analysisResult.Severity,
                        analysisResult.CanAutoRemediate);
                }

                return analysisResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing exception");
                // Return a basic analysis result in case of failure
                return new ErrorAnalysisResult
                {
                    CorrelationId = context.CorrelationId,
                    Timestamp = DateTime.UtcNow,
                    RootCause = "Error during processing",
                    Confidence = 0,
                    SuggestedActions = new List<RemediationAction> { new RemediationAction { Description = "Review logs for details." } },
                    AnalysisError = ex.Message,
                    IsComplete = true
                };
            }
        }

        public async Task<RemediationResult> ApplyRemediationAsync(ErrorAnalysisResult analysisResult)
        {
            if (!_options.EnableAutoRemediation)
            {
                _logger.LogInformation("Automatic remediation is disabled");
                return new RemediationResult
                {
                    Success = false,
                    ActionTaken = "Automatic remediation disabled",
                    RemediationTimestamp = DateTime.UtcNow
                };
            }

            if (analysisResult.Severity < _options.AutoRemediationThreshold)
            {
                _logger.LogInformation(
                    "Skipping remediation due to severity threshold. Current: {Current}, Threshold: {Threshold}",
                    analysisResult.Severity,
                    _options.AutoRemediationThreshold);
                return new RemediationResult
                {
                    Success = false,
                    ActionTaken = "Below severity threshold",
                    RemediationTimestamp = DateTime.UtcNow
                };
            }

            try
            {
                _logger.LogInformation("Attempting to apply remediation for correlation ID: {CorrelationId}", analysisResult.CorrelationId);

                // Get the original context from cache or storage
                // For now, assuming it's in the analysis result or can be retrieved
                var context = analysisResult.Metadata.GetValueOrDefault("OriginalContext") as ErrorContext; // Placeholder
                if (context == null)
                {
                    // Attempt to retrieve context if not in metadata (e.g., from MCP)
                    // This part would need implementation to retrieve context based on correlation ID
                    _logger.LogWarning("Original context not found in analysis metadata for {CorrelationId}. Cannot apply remediation.", analysisResult.CorrelationId);
                    return new RemediationResult
                    {
                        RemediationId = Guid.NewGuid().ToString(),
                        Context = new ErrorContext { CorrelationId = analysisResult.CorrelationId }, // Create a minimal context
                        Status = Models.RemediationStatus.Failed,
                        Message = "Original error context not available for remediation.",
                        Validation = new ValidationResult { IsSuccessful = false, Message = "Missing original context." }
                    };
                }

                // Execute remediation using the injected RemediationExecutor
                var remediationExecution = await _remediationExecutor.ExecuteRemediationAsync(analysisResult, context);

                // Track remediation execution
                await _remediationTracker.TrackRemediationAsync(remediationExecution);

                // Cache remediation result
                var remediationResult = new RemediationResult // Map RemediationExecution to RemediationResult
                {
                    RemediationId = remediationExecution.ExecutionId,
                    Context = context,
                    Plan = new RemediationPlan(), // Populate with plan details from execution if available
                    StartTime = remediationExecution.StartTime,
                    EndTime = remediationExecution.EndTime,
                    Status = remediationExecution.Status == RemediationExecutionStatus.Completed ? Models.RemediationStatus.Completed :
                             remediationExecution.Status == RemediationExecutionStatus.Failed ? Models.RemediationStatus.Failed :
                             Models.RemediationStatus.InProgress, // Map other statuses as needed
                    Message = remediationExecution.Error ?? "Remediation completed.",
                    CompletedSteps = remediationExecution.ExecutedActions.Where(a => a.IsSuccessful).Select(a => a.ActionName).ToList(),
                    FailedSteps = remediationExecution.ExecutedActions.Where(a => !a.IsSuccessful).Select(a => a.ActionName).ToList(),
                    Metrics = remediationExecution.Metrics?.CustomMetrics ?? new Dictionary<string, object>(), // Use custom metrics from execution
                    Validation = remediationExecution.Validation // Use validation result from execution
                };

                _remediationCache[remediationResult.RemediationId] = remediationResult;

                return remediationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation");
                // Return a failed remediation result in case of failure
                return new RemediationResult
                {
                    RemediationId = Guid.NewGuid().ToString(),
                    Context = analysisResult.Metadata.GetValueOrDefault("OriginalContext") as ErrorContext ?? new ErrorContext { CorrelationId = analysisResult.CorrelationId }, // Use original context if available
                    Status = Models.RemediationStatus.Failed,
                    Message = "Error during remediation application.",
                    ErrorMessage = ex.Message,
                    Validation = new ValidationResult { IsSuccessful = false, Message = "Exception during remediation application." }
                };
            }
        }

        public async Task<ErrorContext> EnrichContextAsync(ErrorContext context)
        {
            try
            {
                // Add system information
                context.Metadata ??= new Dictionary<string, object>();
                context.Metadata["MachineName"] = Environment.MachineName;
                context.Metadata["OSVersion"] = Environment.OSVersion.ToString();
                context.Metadata["ProcessId"] = Environment.ProcessId;
                context.Metadata["ThreadId"] = Environment.CurrentManagedThreadId;

                // Add application information
                context.Metadata["AppDomain"] = AppDomain.CurrentDomain.FriendlyName;
                context.Metadata["RuntimeVersion"] = Environment.Version.ToString();

                // Add timestamp if not set
                if (context.Timestamp == default)
                {
                    context.Timestamp = DateTime.UtcNow;
                }

                // Generate correlation ID if not set
                if (string.IsNullOrEmpty(context.CorrelationId))
                {
                    context.CorrelationId = Guid.NewGuid().ToString();
                }

                // Get MCP connection status if available
                if (_mcpClient != null)
                {
                    try
                    {
                        var status = await _mcpClient.GetConnectionStatusAsync();
                        context.Metadata["MCPConnectionStatus"] = status.IsConnected;
                        context.Metadata["MCPEndpoint"] = status.Endpoint;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to get MCP connection status");
                    }
                }

                return context;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enriching context");
                throw new CodeSageException("Failed to enrich context", ex);
            }
        }

        public void RegisterRemediationStrategy(IRemediationStrategy strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            _remediationStrategies.Add(strategy);
            _logger.LogInformation("Registered remediation strategy: {Strategy}", strategy.Name);
        }

        public void Configure(CodeSageOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger.LogInformation("CodeSage service configured with options: {@Options}", options);
        }

        private string GeneratePrompt(Exception exception, ErrorContext context)
        {
          return $@"System: You are an expert .NET runtime error analyzer. Analyze the following error and provide:
            1. A clear explanation of the error
            2. Possible root causes
            3. Recommended remediation steps
            4. Prevention strategies

            Error Context:
            Service: {context.ServiceName}
            Operation: {context.OperationName}
            Environment: {context.Environment}
            Timestamp: {context.Timestamp}
            CorrelationId: {context.CorrelationId}

            Exception:
            Type: {exception.GetType().Name}
            Message: {exception.Message}
            StackTrace: {exception.StackTrace}
            Source: {exception.Source}

            Runtime State:
            {string.Join("\n", context.Metadata?.Select(kv => $"{kv.Key}: {kv.Value}") ?? Array.Empty<string>())}";
        }

        private ErrorAnalysisResult ParseLLMResponse(string llmResponse, Exception exception, ErrorContext context)
        {
            try
            {
                // Split the response into sections
                var sections = llmResponse.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
                var explanation = "";
                var rootCauses = new List<string>();
                var remediationSteps = new List<string>();
                var preventionStrategies = new List<string>();
                var severity = DetermineSeverity(exception);
                var canAutoRemediate = false;
                var remediationStrategy = "Manual intervention required";

                // Parse each section
                foreach (var section in sections)
                {
                    if (section.StartsWith("Explanation:", StringComparison.OrdinalIgnoreCase))
                    {
                        explanation = section.Substring("Explanation:".Length).Trim();
                    }
                    else if (section.StartsWith("Root Causes:", StringComparison.OrdinalIgnoreCase))
                    {
                        var causes = section.Substring("Root Causes:".Length).Trim()
                            .Split(new[] { '\n', '-' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => c.Trim())
                            .Where(c => !string.IsNullOrEmpty(c));
                        rootCauses.AddRange(causes);
                    }
                    else if (section.StartsWith("Remediation Steps:", StringComparison.OrdinalIgnoreCase))
                    {
                        var steps = section.Substring("Remediation Steps:".Length).Trim()
                            .Split(new[] { '\n', '-' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrEmpty(s));
                        remediationSteps.AddRange(steps);

                        // Determine if auto-remediation is possible
                        canAutoRemediate = DetermineAutoRemediationPossibility(steps, severity);
                        if (canAutoRemediate)
                        {
                            remediationStrategy = "Automatic remediation available";
                        }
                    }
                    else if (section.StartsWith("Prevention Strategies:", StringComparison.OrdinalIgnoreCase))
                    {
                        var strategies = section.Substring("Prevention Strategies:".Length).Trim()
                            .Split(new[] { '\n', '-' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Where(s => !string.IsNullOrEmpty(s));
                        preventionStrategies.AddRange(strategies);
                    }
                }

                // Create the analysis result
                return new ErrorAnalysisResult
                {
                    NaturalLanguageExplanation = explanation,
                    RootCauses = rootCauses,
                    SuggestedActions = remediationSteps,
                    PreventionStrategies = preventionStrategies,
                    ContextualData = context.Metadata,
                    CanAutoRemediate = canAutoRemediate,
                    RemediationStrategy = remediationStrategy,
                    Severity = severity,
                    Timestamp = DateTime.UtcNow,
                    CorrelationId = context.CorrelationId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing LLM response");
                throw new CodeSageException("Failed to parse LLM response", ex);
            }
        }

        private SeverityLevel DetermineSeverity(Exception exception)
        {
            // Base severity on exception type and message
            var severity = exception switch
            {
                // Critical system-level exceptions
                OutOfMemoryException => SeverityLevel.Critical,
                StackOverflowException => SeverityLevel.Critical,
                ThreadAbortException => SeverityLevel.Critical,
                AccessViolationException => SeverityLevel.Critical,

                // High-severity application exceptions
                NullReferenceException => SeverityLevel.High,
                ArgumentNullException => SeverityLevel.High,
                InvalidOperationException => SeverityLevel.High,
                NotSupportedException => SeverityLevel.High,
                NotImplementedException => SeverityLevel.High,

                // Medium-severity exceptions
                ArgumentException => SeverityLevel.Medium,
                FormatException => SeverityLevel.Medium,
                TimeoutException => SeverityLevel.Medium,
                IOException => SeverityLevel.Medium,
                UnauthorizedAccessException => SeverityLevel.Medium,

                // Low-severity exceptions
                _ => SeverityLevel.Low
            };

            // Adjust severity based on exception message keywords
            var message = exception.Message.ToLowerInvariant();
            if (message.Contains("critical") || message.Contains("fatal") || message.Contains("severe"))
            {
                severity = (SeverityLevel)Math.Min((int)SeverityLevel.Critical, (int)severity + 1);
            }
            else if (message.Contains("warning") || message.Contains("minor") || message.Contains("non-critical"))
            {
                severity = (SeverityLevel)Math.Max((int)SeverityLevel.Low, (int)severity - 1);
            }

            // Check for inner exceptions
            if (exception.InnerException != null)
            {
                var innerSeverity = DetermineSeverity(exception.InnerException);
                severity = (SeverityLevel)Math.Max((int)severity, (int)innerSeverity);
            }

            return severity;
        }

        private bool DetermineAutoRemediationPossibility(IEnumerable<string> remediationSteps, SeverityLevel severity)
        {
            // Don't auto-remediate critical issues
            if (severity == SeverityLevel.Critical)
            {
                return false;
            }

            // Check if steps contain keywords indicating manual intervention
            var manualKeywords = new[]
            {
                "manual", "human", "review", "verify", "check", "confirm",
                "approve", "authorize", "validate", "inspect", "audit"
            };

            var steps = remediationSteps.Select(s => s.ToLowerInvariant());
            var hasManualSteps = steps.Any(s => manualKeywords.Any(k => s.Contains(k)));

            // Check if steps contain keywords indicating automatic remediation
            var autoKeywords = new[]
            {
                "automatically", "auto", "retry", "restart", "reload",
                "refresh", "clear", "reset", "reinitialize", "reconnect"
            };

            var hasAutoSteps = steps.Any(s => autoKeywords.Any(k => s.Contains(k)));

            // Auto-remediation is possible if:
            // 1. There are no manual steps required
            // 2. There are automatic steps available
            // 3. The severity is not critical
            return !hasManualSteps && hasAutoSteps;
        }
    }
}