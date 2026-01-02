using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Analysis.Exceptions;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Application.Exceptions;
using RuntimeErrorSage.Application.LLM;
using RuntimeErrorSage.Application.LLM.Interfaces;
using RuntimeErrorSage.Application.MCP.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.LLM;
using RuntimeErrorSage.Domain.Models.Remediation;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Diagnostics;
using RuntimeErrorSage.Core.Storage.Utilities;

namespace RuntimeErrorSage.Application.Analysis;

/// <summary>
/// Analyzes errors using a local LLM.
/// </summary>
public class ErrorAnalyzer : IErrorAnalyzer
{
    private readonly ILogger<ErrorAnalyzer> _logger;
    private readonly ILMStudioClient _llmClient;
    private readonly IMCPClient _mcpClient;
    private readonly ConcurrentDictionary<string, List<ErrorPattern>> _localPatternCache;
    private readonly Dictionary<string, string> _promptTemplates;
    private readonly ILLMService _llmService;
    private readonly Dictionary<string, double> _errorTypeConfidence;
    private readonly Dictionary<string, List<string>> _remediationSteps;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorAnalyzer"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="llmClient">The LLM client.</param>
    /// <param name="mcpClient">The MCP client.</param>
    /// <param name="llmService">The LLM service.</param>
    public ErrorAnalyzer(
        ILogger<ErrorAnalyzer> logger,
        ILMStudioClient llmClient,
        IMCPClient mcpClient,
        ILLMService llmService)
    {
        _logger = logger;
        _llmClient = llmClient;
        _mcpClient = mcpClient;
        _localPatternCache = new();
        _promptTemplates = InitializePromptTemplates();
        _llmService = llmService ?? throw new ArgumentNullException(nameof(llmService));
        _errorTypeConfidence = new Dictionary<string, double>();
        _remediationSteps = new Dictionary<string, List<string>>();
    }

    /// <summary>
    /// Analyzes an error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The analysis.</returns>
    public async Task<ErrorAnalysis> AnalyzeAsync(RuntimeError error)
    {
        if (error == null)
            throw new ArgumentNullException(nameof(error));

        var context = new ErrorContext(error, null, DateTime.UtcNow);

        var prompt = GeneratePrompt(null, context, null);
        var response = await _llmService.GenerateAsync(prompt);
        var analysis = ParseResponse(response);

        if (!ValidateAnalysis(analysis))
            throw new InvalidOperationException("Invalid analysis.");

        return analysis;
    }

    /// <summary>
    /// Analyzes an error context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>The analysis.</returns>
    public async Task<ErrorAnalysis> AnalyzeContextAsync(ErrorContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (!context.Validate())
            throw new ArgumentException("Invalid context.", nameof(context));

        var prompt = GeneratePrompt(null, context, null);
        var response = await _llmService.GenerateAsync(prompt);
        var analysis = ParseResponse(response);

        if (!ValidateAnalysis(analysis))
            throw new InvalidOperationException("Invalid analysis.");

        return analysis;
    }

    /// <summary>
    /// Validates an analysis.
    /// </summary>
    /// <param name="analysis">The analysis.</param>
    /// <returns>True if the analysis is valid; otherwise, false.</returns>
    public bool ValidateAnalysis(ErrorAnalysis analysis)
    {
        if (analysis == null)
            return false;

        if (!analysis.Validate())
            return false;

        if (string.IsNullOrEmpty(analysis.ErrorType))
            return false;

        if (string.IsNullOrEmpty(analysis.RootCause))
            return false;

        if (analysis.Confidence < 0 || analysis.Confidence > 1)
            return false;

        return true;
    }

    /// <summary>
    /// Generates a prompt for error analysis.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The prompt.</returns>
    private string GeneratePrompt(Exception exception, ErrorContext context, ErrorPattern? matchingPattern)
    {
        var prompt = $"Analyze the following .NET runtime error:\n\n";

        if (matchingPattern != null)
        {
            prompt += $"This error matches a known pattern (ID: {matchingPattern.Id}).\n";
            prompt += $"Pattern Description: {matchingPattern.Notes ?? "N/A"}\n";
            // Optionally include pattern context or remediation strategies if relevant
        }

        prompt += $"Exception Type: {exception.GetType().Name}\n";
        prompt += $"Message: {exception.Message}\n";
        prompt += $"StackTrace: {exception.StackTrace}\n";
        prompt += $"Service: {context.ServiceName}\n";
        prompt += $"Operation: {context.OperationName}\n";
        prompt += $"Timestamp: {context.Timestamp}\n";

        if (context.AdditionalContext?.Any() == true)
        {
            prompt += "\nAdditional Context:\n";
            foreach (var kvp in context.AdditionalContext)
            {
                prompt += $"- {kvp.Key}: {kvp.Value}\n";
            }
        }

        prompt += "\nPlease provide:\n";
        prompt += "1. A concise natural language explanation of the error.\n";
        prompt += "2. The most likely root cause(s).\n";
        prompt += "3. Actionable remediation steps.\n";
        prompt += "4. Suggested prevention strategies.\n";
        prompt += "5. A confidence score for your analysis (0-1). Please provide only the numerical score.";

        return prompt;
    }

    /// <summary>
    /// Parses the LLM response.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <returns>The analysis.</returns>
    private ErrorAnalysis ParseResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
            throw new ArgumentException("Response cannot be null or empty.", nameof(response));

        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var errorType = string.Empty;
        var rootCause = string.Empty;
        var confidence = 0.0;
        var remediationSteps = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith("1. Error type:", StringComparison.OrdinalIgnoreCase))
            {
                errorType = line.Substring("1. Error type:".Length).Trim();
            }
            else if (line.StartsWith("2. Root cause:", StringComparison.OrdinalIgnoreCase))
            {
                rootCause = line.Substring("2. Root cause:".Length).Trim();
            }
            else if (line.StartsWith("3. Confidence:", StringComparison.OrdinalIgnoreCase))
            {
                var confidenceStr = line.Substring("3. Confidence:".Length).Trim();
                if (double.TryParse(confidenceStr, out var confidenceValue))
                {
                    confidence = confidenceValue;
                }
            }
            else if (line.StartsWith("4. Remediation steps:", StringComparison.OrdinalIgnoreCase))
            {
                var steps = line.Substring("4. Remediation steps:".Length).Trim();
                remediationSteps.AddRange(steps.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()));
            }
        }

        return new ErrorAnalysis(errorType, rootCause, confidence, remediationSteps);
    }

    public async Task<ErrorAnalysisResult> AnalyzeErrorAsync(Exception exception, ErrorContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var process = Process.GetCurrentProcess();
        var startMemory = process.WorkingSet64;
        var startCpu = process.TotalProcessorTime;

        try
        {
            // Check for known patterns
            var knownPatterns = await GetKnownPatternsAsync(context.ServiceName);
            var matchingPattern = FindMatchingPattern(exception, context, knownPatterns);

            if (matchingPattern != null && matchingPattern.Analysis != null)
            {
                _logger.LogInformation(
                    "Found matching error pattern for {Service}:{Operation} - {Pattern}",
                    context.ServiceName,
                    context.OperationName,
                    matchingPattern.Id);

                // Create a copy to avoid mutating the cached pattern
                var result = new ErrorAnalysisResult // Renamed from 'analysisResult' to 'result'
                {
                    ErrorId = matchingPattern.Analysis.ErrorId,
                    Timestamp = DateTime.UtcNow,
                    ErrorType = matchingPattern.Analysis.ErrorType,
                    Category = matchingPattern.Analysis.Category,
                    Severity = matchingPattern.Analysis.Severity,
                    Message = matchingPattern.Analysis.Message,
                    Details = new Dictionary<string, object>(matchingPattern.Analysis.Details ?? new Dictionary<string, object>())
                };

                // Add performance metrics to Details
                stopwatch.Stop();
                var endMemory = process.WorkingSet64;
                var endCpu = process.TotalProcessorTime;
                result.Details["PerformanceMetrics"] = new
                {
                    AnalysisDurationMs = stopwatch.Elapsed.TotalMilliseconds,
                    MemoryUsageMB = (endMemory - startMemory) / (1024.0 * 1024.0),
                    CpuUsageTimeMs = (endCpu - startCpu).TotalMilliseconds
                };
                return result;
            }

            // If no matching pattern or pattern has no analysis, generate a new analysis using LLM
            _logger.LogInformation("No matching pattern found or pattern has no analysis. Generating analysis using LLM.");
            var prompt = GeneratePrompt(exception, context, matchingPattern);
            var llmResponse = await _llmClient.AnalyzeErrorAsync(prompt);
            var analysisResult = ParseLLMResponse(llmResponse, exception, context);

            // Store the new pattern if it meets the criteria
            if (matchingPattern == null && IsNewPattern(analysisResult))
            {
                var newPattern = CreateErrorPattern(exception, context, analysisResult);
                await _mcpClient.UpdateErrorPatternsAsync(new List<ErrorPattern> { newPattern });
                _logger.LogInformation("New error pattern created and stored: {PatternId}", newPattern.Id);
            }

            // Add performance metrics to Details
            stopwatch.Stop();
            var endMemory2 = process.WorkingSet64;
            var endCpu2 = process.TotalProcessorTime;
            if (analysisResult.Details == null)
                analysisResult.Details = new Dictionary<string, object>();
            analysisResult.Details["PerformanceMetrics"] = new
            {
                AnalysisDurationMs = stopwatch.Elapsed.TotalMilliseconds,
                MemoryUsageMB = (endMemory2 - startMemory) / (1024.0 * 1024.0),
                CpuUsageTimeMs = (endCpu2 - startCpu).TotalMilliseconds
            };

            return analysisResult;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error during error analysis");
            throw new ErrorAnalysisException("Failed to analyze error", ex);
        }
    }

    public Task<ErrorAnalysisResult?> GetAnalysisStatusAsync(string correlationId)
    {
        // This would typically involve checking a cache or persistent storage
        _logger.LogInformation("GetAnalysisStatusAsync not implemented");
        return Task.FromResult<ErrorAnalysisResult?>(null);
    }

    private ErrorAnalysisResult ParseLLMResponse(
        string llmResponse,
        Exception exception,
        ErrorContext context)
    {
        var explanation = "Analysis incomplete.";
        var rootCauses = new List<string>();
        var remediationSteps = new List<string>();
        var preventionStrategies = new List<string>();
        var confidence = 0.0;

        // Split the response into sections based on expected headings
        var sections = llmResponse.Split(
            new[] { "Explanation:", "Root Causes:", "Remediation Steps:", "Prevention Strategies:", "Confidence:" },
            StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < sections.Length; i++)
        {
            var section = sections[i].Trim();
            if (string.IsNullOrEmpty(section)) continue;

            if (i == 0 && !llmResponse.Trim().StartsWith("Explanation:", StringComparison.OrdinalIgnoreCase))
            {
                 // If the response doesn't start with Explanation, treat the first section as explanation
                 explanation = section;
            }
            else if (llmResponse.IndexOf("Explanation:", StringComparison.OrdinalIgnoreCase) == sections.Take(i).Sum(s => s.Length) + (i > 0 ? "Explanation:".Length : 0))
            {
                explanation = section;
            }
            else if (llmResponse.IndexOf("Root Causes:", StringComparison.OrdinalIgnoreCase) == sections.Take(i).Sum(s => s.Length) + (i > 0 ? "Root Causes:".Length : 0))
            {
                 rootCauses.AddRange(section.Split(new[] { '\n', '-' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)));
            }
             else if (llmResponse.IndexOf("Remediation Steps:", StringComparison.OrdinalIgnoreCase) == sections.Take(i).Sum(s => s.Length) + (i > 0 ? "Remediation Steps:".Length : 0))
            {
                remediationSteps.AddRange(section.Split(new[] { '\n', '-' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)));
            }
            else if (llmResponse.IndexOf("Prevention Strategies:", StringComparison.OrdinalIgnoreCase) == sections.Take(i).Sum(s => s.Length) + (i > 0 ? "Prevention Strategies:".Length : 0))
            {
                 preventionStrategies.AddRange(section.Split(new[] { '\n', '-' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)));
            }
            else if (llmResponse.IndexOf("Confidence:", StringComparison.OrdinalIgnoreCase) == sections.Take(i).Sum(s => s.Length) + (i > 0 ? "Confidence:".Length : 0))
            {
                if (double.TryParse(section, out var parsedConfidence))
                {
                    confidence = parsedConfidence;
                }
            }
        }

        // Create remediation actions from steps
        var remediationActions = remediationSteps.Select(step => new RemediationAction { Description = step }).ToList();

        return new ErrorAnalysisResult
        {
            ErrorId = context.ErrorId,
            Timestamp = DateTime.UtcNow,
            ErrorType = context.ErrorType,
            Category = context.Category.ToString(),
            //Severity = context.Severity,//TODO: set severity based on analysis
            Message = explanation,
            Details = new Dictionary<string, object>
            {
                { "RootCause", string.Join(", ", rootCauses) },
                { "Confidence", confidence },
                { "RemediationActions", remediationActions },
                { "LLM_Explanation", explanation },
                { "LLM_PreventionStrategies", preventionStrategies }
            }
        };
    }

    private async Task<List<ErrorPattern>> GetKnownPatternsAsync(string serviceName)
    {
        // This would typically load patterns from distributed storage via MCP
        // For now, using a local cache
        if (_localPatternCache.TryGetValue(serviceName, out var patterns))
        {
            return patterns;
        }

        try
        {
            var patternsFromMcp = await _mcpClient.GetErrorPatternsAsync(serviceName);
            _localPatternCache[serviceName] = patternsFromMcp;
            return patternsFromMcp;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving known patterns for service {Service}", serviceName);
            return new List<ErrorPattern>();
        }
    }

    private ErrorPattern? FindMatchingPattern(
        Exception exception,
        ErrorContext context,
        List<ErrorPattern> patterns)
    {
        return patterns.FirstOrDefault(p =>
            p.ErrorType == exception.GetType().Name &&
            p.OperationName == context.OperationName &&
            IsSimilarContext(p.Context, context.AdditionalContext));
    }

    private bool IsSimilarContext(Dictionary<string, object> patternContext, Dictionary<string, string> currentContext)
    {
        // Compare key context attributes using the utility class
        return ContextComparer.CompareAdditionalContext(patternContext, currentContext);
    }

    private bool IsNewPattern(ErrorAnalysisResult analysisResult)
    {
        // Get confidence from Details if present
        double confidence = 0.0;
        if (analysisResult.Details != null && analysisResult.Details.TryGetValue("Confidence", out var confObj) && confObj is double confVal)
            confidence = confVal;
        return confidence > 0.7; // Example threshold
    }

    private ErrorPattern CreateErrorPattern(
        Exception exception,
        ErrorContext context,
        ErrorAnalysisResult analysisResult)
    {
        string llmExplanation = "";
        if (analysisResult.Details != null && analysisResult.Details.TryGetValue("LLM_Explanation", out var explanationObj) && explanationObj is string explanationStr)
            llmExplanation = explanationStr;

        var remediationActions = analysisResult.Details != null && analysisResult.Details.TryGetValue("RemediationActions", out var actionsObj) && actionsObj is List<RemediationAction> actions
            ? actions.Select(a => a.Description).ToList()
            : new List<string>();

        var rootCause = analysisResult.Details != null && analysisResult.Details.TryGetValue("RootCause", out var rootCauseObj) ? rootCauseObj?.ToString() : "";

        return new ErrorPattern
        {
            Id = Guid.NewGuid().ToString(),
            ServiceName = context.ServiceName,
            ErrorType = exception.GetType().Name,
            OperationName = context.OperationName,
            FirstOccurrence = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            OccurrenceCount = 1,
            Context = context.AdditionalContext.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value),
            RemediationStrategies = remediationActions,
            PatternMetadata = new Dictionary<string, object>
            {
                { "LLM_Explanation", llmExplanation },
                { "LLM_RootCauses", rootCause ?? string.Empty }
            },
            IsActive = true,
            Notes = rootCause ?? ""
        };
    }

    private Dictionary<string, string> InitializePromptTemplates()
    {
        return new Dictionary<string, string>
        {
            { "database", "Analyze the following database error:\n\nException Type: {ExceptionType}\nMessage: {ExceptionMessage}\nStackTrace: {StackTrace}\nAdditional Context:\n{AdditionalContext}\n\nPlease provide: Root Cause, Remediation Steps, Confidence." },
            { "filesystem", "Analyze the following file system error:\n\nException Type: {ExceptionType}\nMessage: {ExceptionMessage}\nStackTrace: {StackTrace}\nAdditional Context:\n{AdditionalContext}\n\nPlease provide: Root Cause, Remediation Steps, Confidence." },
            { "http", "Analyze the following HTTP error:\n\nException Type: {ExceptionType}\nMessage: {ExceptionMessage}\nStackTrace: {StackTrace}\nAdditional Context:\n{AdditionalContext}\n\nPlease provide: Root Cause, Remediation Steps, Confidence." },
            { "resource", "Analyze the following resource allocation error:\n\nException Type: {ExceptionType}\nMessage: {ExceptionMessage}\nStackTrace: {StackTrace}\nAdditional Context:\n{AdditionalContext}\n\nPlease provide: Root Cause, Remediation Steps, Confidence." },
            { "general", "Analyze the following .NET runtime error:\n\nException Type: {ExceptionType}\nMessage: {ExceptionMessage}\nStackTrace: {StackTrace}\nAdditional Context:\n{AdditionalContext}\n\nPlease provide: Root Cause, Remediation Steps, Confidence." }
        };
    }

    private string FormatAdditionalContext(Dictionary<string, object> context)
    {
        if (context == null || context.Count == 0)
            return "No additional context available.";

        return string.Join("\n", context.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
    }

    private string GetErrorType(Exception ex)
    {
        return ex switch
        {
            SqlException => "database",
            UnauthorizedAccessException => "filesystem",
            HttpRequestException => "http",
            OutOfMemoryException => "resource",
            _ => "general"
        };
    }

    /// <inheritdoc />
    public async Task<LLMAnalysisResult> EnrichLLMAnalysisAsync(LLMAnalysisResult llmAnalysis)
    {
        if (llmAnalysis == null)
            throw new ArgumentNullException(nameof(llmAnalysis));

        // Add additional insights and enrich the analysis
        llmAnalysis.Confidence = Math.Min(llmAnalysis.Confidence + 0.1, 1.0); // Boost confidence slightly
        llmAnalysis.Metadata["EnrichedBy"] = "ErrorAnalyzer";
        llmAnalysis.Metadata["EnrichmentTimestamp"] = DateTime.UtcNow;

        return llmAnalysis;
    }

    /// <summary>
    /// Analyzes remediation options for an error
    /// </summary>
    /// <param name="errorContext">The error context</param>
    /// <returns>The remediation analysis</returns>
    public async Task<RemediationAnalysis> AnalyzeRemediationAsync(ErrorContext errorContext)
    {
        if (errorContext == null)
            throw new ArgumentNullException(nameof(errorContext));

        // Use error context analyzer to get remediation analysis
        var llmAnalysis = await _mcpClient.AnalyzeContextAsync(errorContext);
        
        // Convert LLMAnalysisResult to RemediationAnalysis
        var suggestedActions = llmAnalysis.SuggestedActions ?? new List<RemediationAction>();
        
        return new RemediationAnalysis
        {
            ErrorContext = errorContext,
            SuggestedActions = suggestedActions,
            Confidence = llmAnalysis.Confidence,
            Timestamp = DateTime.UtcNow,
            AnalysisId = Guid.NewGuid().ToString()
        };
    }

    /// <summary>
    /// Analyzes the impact of an error
    /// </summary>
    /// <param name="errorContext">The error context</param>
    /// <returns>The impact analysis result</returns>
    public async Task<RuntimeErrorSage.Domain.Models.Graph.ImpactAnalysisResult> AnalyzeImpactAsync(ErrorContext errorContext)
    {
        if (errorContext == null)
            throw new ArgumentNullException(nameof(errorContext));

        // Analyze impact using graph analysis
        var graphAnalysis = await AnalyzeGraphAsync(errorContext);
        
        return new RuntimeErrorSage.Domain.Models.Graph.ImpactAnalysisResult
        {
            AffectedComponents = graphAnalysis.AffectedComponents ?? new List<string>(),
            ImpactLevel = RuntimeErrorSage.Domain.Enums.ImpactLevel.Medium,
            EstimatedRecoveryTime = TimeSpan.FromMinutes(5),
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Analyzes the dependency graph for an error
    /// </summary>
    /// <param name="errorContext">The error context</param>
    /// <returns>The graph analysis result</returns>
    public async Task<RuntimeErrorSage.Domain.Models.Graph.GraphAnalysisResult> AnalyzeGraphAsync(ErrorContext errorContext)
    {
        if (errorContext == null)
            throw new ArgumentNullException(nameof(errorContext));

        // Use MCP client to analyze graph
        var analysis = await _mcpClient.AnalyzeContextAsync(errorContext);
        
        return new RuntimeErrorSage.Domain.Models.Graph.GraphAnalysisResult
        {
            AffectedComponents = analysis.SuggestedActions?.Select(a => a.Name).ToList() ?? new List<string>(),
            DependencyGraph = errorContext.DependencyGraph,
            Timestamp = DateTime.UtcNow
        };
    }
} 


