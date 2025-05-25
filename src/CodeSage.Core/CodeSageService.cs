using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

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
        private CodeSageOptions _options;
        private readonly ILMStudioClient _lmStudioClient;
        private readonly IMCPClient _mcpClient;

        public CodeSageService(
            ILogger<CodeSageService> logger,
            IConfiguration configuration,
            ILMStudioClient lmStudioClient,
            IMCPClient mcpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _lmStudioClient = lmStudioClient ?? throw new ArgumentNullException(nameof(lmStudioClient));
            _mcpClient = mcpClient ?? throw new ArgumentNullException(nameof(mcpClient));
            _remediationStrategies = new List<IRemediationStrategy>();
            _options = new CodeSageOptions();
        }

        public async Task<ErrorAnalysisResult> ProcessExceptionAsync(Exception exception, ErrorContext context)
        {
            try
            {
                _logger.LogInformation("Processing exception of type {ExceptionType}", exception.GetType().Name);

                // Enrich the context if enabled
                if (_options.EnableContextEnrichment)
                {
                    context = await EnrichContextAsync(context);
                }

                // Generate the prompt for the LLM
                var prompt = GeneratePrompt(exception, context);

                // Get analysis from LM Studio
                var llmResponse = await _lmStudioClient.AnalyzeErrorAsync(prompt);

                // Parse and validate the response
                var analysisResult = ParseLLMResponse(llmResponse, exception, context);

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
                throw new CodeSageException("Failed to process exception", ex);
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
                // Find an appropriate remediation strategy
                var strategy = _remediationStrategies.Find(s => s.CanHandle(analysisResult));
                if (strategy == null)
                {
                    _logger.LogWarning("No suitable remediation strategy found");
                    return new RemediationResult
                    {
                        Success = false,
                        ActionTaken = "No suitable strategy",
                        RemediationTimestamp = DateTime.UtcNow
                    };
                }

                // Apply the remediation strategy
                var result = await strategy.ApplyAsync(analysisResult);

                // Log the remediation attempt
                if (_options.EnableAuditLogging)
                {
                    _logger.LogInformation(
                        "Remediation applied using strategy {Strategy}. Success: {Success}",
                        strategy.Name,
                        result.Success);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying remediation");
                throw new CodeSageException("Failed to apply remediation", ex);
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
            // TODO: Implement proper parsing of LLM response
            // This is a placeholder implementation
            return new ErrorAnalysisResult
            {
                NaturalLanguageExplanation = llmResponse,
                SuggestedActions = new List<string> { "Implement proper error handling" },
                ContextualData = context.Metadata,
                CanAutoRemediate = false,
                RemediationStrategy = "Manual intervention required",
                Severity = DetermineSeverity(exception)
            };
        }

        private SeverityLevel DetermineSeverity(Exception exception)
        {
            // TODO: Implement proper severity determination
            // This is a placeholder implementation
            return exception switch
            {
                NullReferenceException => SeverityLevel.High,
                ArgumentException => SeverityLevel.Medium,
                TimeoutException => SeverityLevel.Medium,
                _ => SeverityLevel.Low
            };
        }
    }

    /// <summary>
    /// Custom exception type for CodeSage-specific errors.
    /// </summary>
    public class CodeSageException : Exception
    {
        public CodeSageException(string message) : base(message)
        {
        }

        public CodeSageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
} 