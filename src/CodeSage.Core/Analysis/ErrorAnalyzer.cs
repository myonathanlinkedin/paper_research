using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeSage.Core.Interfaces;
using CodeSage.Core.Models;
using CodeSage.Core.Exceptions;
using System.Collections.Concurrent;
using CodeSage.Core.Models.Error;
using CodeSage.Core.Interfaces.MCP;
using CodeSage.Core.Utilities;

namespace CodeSage.Core.Analysis;

public class ErrorAnalyzer : IErrorAnalyzer
{
    private readonly ILogger<ErrorAnalyzer> _logger;
    private readonly ILMStudioClient _llmClient;
    private readonly IMCPClient _mcpClient;
    private readonly ConcurrentDictionary<string, List<ErrorPattern>> _localPatternCache;
    private readonly Dictionary<string, string> _promptTemplates;

    public ErrorAnalyzer(
        ILogger<ErrorAnalyzer> logger,
        ILMStudioClient llmClient,
        IMCPClient mcpClient)
    {
        _logger = logger;
        _llmClient = llmClient;
        _mcpClient = mcpClient;
        _localPatternCache = new();
        _promptTemplates = InitializePromptTemplates();
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

            if (matchingPattern != null)
            {
                _logger.LogInformation(
                    "Found matching error pattern for {Service}:{Operation} - {Pattern}",
                    context.ServiceName,
                    context.OperationName,
                    matchingPattern.PatternId);

                // Use the pattern's analysis if available
                if (matchingPattern.Analysis != null)
                {
                    var analysisResult = matchingPattern.Analysis;
                    // Add performance metrics to the analysis result
                    stopwatch.Stop();
                    var endMemory = process.WorkingSet64;
                    var endCpu = process.TotalProcessorTime;
                    analysisResult.Metadata["PerformanceMetrics"] = new
                    {
                        AnalysisDurationMs = stopwatch.Elapsed.TotalMilliseconds,
                        MemoryUsageMB = (endMemory - startMemory) / (1024.0 * 1024.0),
                        CpuUsageTimeMs = (endCpu - startCpu).TotalMilliseconds
                    };
                    return analysisResult;
                }
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
                await _mcpClient.UpdateErrorPatternsAsync(context.ServiceName, new List<ErrorPattern> { newPattern });
                _logger.LogInformation("New error pattern created and stored: {PatternId}", newPattern.PatternId);
            }

            // Add performance metrics to the analysis result
            stopwatch.Stop();
            var endMemory = process.WorkingSet64;
            var endCpu = process.TotalProcessorTime;
            analysisResult.Metadata["PerformanceMetrics"] = new
            {
                AnalysisDurationMs = stopwatch.Elapsed.TotalMilliseconds,
                MemoryUsageMB = (endMemory - startMemory) / (1024.0 * 1024.0),
                CpuUsageTimeMs = (endCpu - startCpu).TotalMilliseconds
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

    private string GeneratePrompt(Exception exception, ErrorContext context, ErrorPattern? matchingPattern)
    {
        var prompt = $"Analyze the following .NET runtime error:\n\n";

        if (matchingPattern != null)
        {
            prompt += $"This error matches a known pattern (ID: {matchingPattern.PatternId}).\n";
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
            CorrelationId = context.CorrelationId,
            Timestamp = DateTime.UtcNow,
            RootCause = string.Join(", ", rootCauses),
            Confidence = confidence,
            SuggestedActions = remediationActions,
            Metadata = new Dictionary<string, object>(
                context.AdditionalContext.Where(kvp => !kvp.Key.StartsWith("LLM_") && !kvp.Key.StartsWith("Pattern_")))
            {
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

    private bool IsSimilarContext(Dictionary<string, object> patternContext, Dictionary<string, object> currentContext)
    {
        // Compare key context attributes using the utility class
        return ContextComparer.CompareAdditionalContext(patternContext, currentContext);
    }

     private bool IsNewPattern(ErrorAnalysisResult analysisResult)
    {
        // Determine if the analysis result represents a new, distinct pattern
        // This could involve checking confidence score, novelty of root cause/remediation, etc.
        // For now, a simple check based on confidence
        return analysisResult.Confidence > 0.7; // Example threshold
    }

    private ErrorPattern CreateErrorPattern(
        Exception exception,
        ErrorContext context,
        ErrorAnalysisResult analysisResult)
    {
        // Create a new ErrorPattern based on the analysis result and context
        return new ErrorPattern
        {
            PatternId = Guid.NewGuid().ToString(),
            ServiceName = context.ServiceName,
            ErrorType = exception.GetType().Name,
            OperationName = context.OperationName,
            FirstOccurrence = DateTime.UtcNow,
            LastOccurrence = DateTime.UtcNow,
            OccurrenceCount = 1,
            Context = context,
            RemediationStrategies = analysisResult.SuggestedActions.Select(a => a.Name).ToList(), // Using action names as strategies for simplicity
            PatternMetadata = new Dictionary<string, object>
            {
                { "LLM_Explanation", analysisResult.Metadata.GetValueOrDefault("LLM_Explanation") ?? "" },
                { "LLM_RootCauses", analysisResult.RootCause }
            },
            IsActive = true,
            Notes = analysisResult.NaturalLanguageExplanation // Using explanation as notes for simplicity
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
            System.Data.SqlClient.SqlException => "database",
            UnauthorizedAccessException => "filesystem",
            System.Net.Http.HttpRequestException => "http",
            OutOfMemoryException => "resource",
            _ => "general"
        };
    }
} 