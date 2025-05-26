using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.LLM;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.LLM;

/// <summary>
/// Implements integration with Qwen 2.5 7B Instruct 1M model for error analysis and remediation.
/// </summary>
public class QwenModelIntegration : ILLMIntegration
{
    private readonly IQwenClient _qwenClient;
    private readonly IModelConfigurationProvider _configProvider;
    private const string MODEL_VERSION = "qwen-2.5-7b-instruct-1m";

    public QwenModelIntegration(IQwenClient qwenClient, IModelConfigurationProvider configProvider)
    {
        _qwenClient = qwenClient ?? throw new ArgumentNullException(nameof(qwenClient));
        _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
    }

    /// <summary>
    /// Analyzes a runtime error using the Qwen model.
    /// </summary>
    public async Task<ErrorAnalysisResult> AnalyzeErrorAsync(RuntimeError error, ErrorContext context)
    {
        var config = await _configProvider.GetModelConfigurationAsync(MODEL_VERSION);
        var prompt = BuildErrorAnalysisPrompt(error, context);

        var response = await _qwenClient.GetCompletionAsync(new ModelRequest
        {
            Prompt = prompt,
            MaxTokens = config.MaxTokens,
            Temperature = config.Temperature,
            TopP = config.TopP,
            StopSequences = config.StopSequences
        });

        return ParseErrorAnalysisResponse(response);
    }

    /// <summary>
    /// Generates remediation suggestions using the Qwen model.
    /// </summary>
    public async Task<List<RemediationSuggestion>> GenerateRemediationSuggestionsAsync(RuntimeError error, ErrorAnalysisResult analysis)
    {
        var config = await _configProvider.GetModelConfigurationAsync(MODEL_VERSION);
        var prompt = BuildRemediationPrompt(error, analysis);

        var response = await _qwenClient.GetCompletionAsync(new ModelRequest
        {
            Prompt = prompt,
            MaxTokens = config.MaxTokens,
            Temperature = config.Temperature,
            TopP = config.TopP,
            StopSequences = config.StopSequences
        });

        return ParseRemediationResponse(response);
    }

    /// <summary>
    /// Validates a proposed remediation using the Qwen model.
    /// </summary>
    public async Task<RemediationValidationResult> ValidateRemediationAsync(RemediationSuggestion suggestion, ErrorContext context)
    {
        var config = await _configProvider.GetModelConfigurationAsync(MODEL_VERSION);
        var prompt = BuildValidationPrompt(suggestion, context);

        var response = await _qwenClient.GetCompletionAsync(new ModelRequest
        {
            Prompt = prompt,
            MaxTokens = config.MaxTokens,
            Temperature = config.Temperature,
            TopP = config.TopP,
            StopSequences = config.StopSequences
        });

        return ParseValidationResponse(response);
    }

    private string BuildErrorAnalysisPrompt(RuntimeError error, ErrorContext context)
    {
        return $@"Analyze the following runtime error:
Error Message: {error.Message}
Stack Trace: {error.StackTrace}
Context:
- Environment: {context.Environment}
- Component: {context.Component}
- Related Variables: {string.Join(", ", context.Variables)}

Provide a detailed analysis of:
1. Root cause
2. Error classification
3. Potential impact
4. Severity level";
    }

    private string BuildRemediationPrompt(RuntimeError error, ErrorAnalysisResult analysis)
    {
        return $@"Generate remediation suggestions for:
Error: {error.Message}
Root Cause: {analysis.RootCause}
Classification: {analysis.Classification}
Severity: {analysis.Severity}

Provide:
1. Immediate mitigation steps
2. Long-term fixes
3. Prevention measures";
    }

    private string BuildValidationPrompt(RemediationSuggestion suggestion, ErrorContext context)
    {
        return $@"Validate the following remediation suggestion:
Suggestion: {suggestion.Description}
Implementation: {suggestion.Implementation}
Context:
- Environment: {context.Environment}
- Component: {context.Component}

Evaluate:
1. Effectiveness
2. Side effects
3. Implementation risks";
    }

    private ErrorAnalysisResult ParseErrorAnalysisResponse(ModelResponse response)
    {
        // Implement response parsing logic
        return new ErrorAnalysisResult();
    }

    private List<RemediationSuggestion> ParseRemediationResponse(ModelResponse response)
    {
        // Implement response parsing logic
        return new List<RemediationSuggestion>();
    }

    private RemediationValidationResult ParseValidationResponse(ModelResponse response)
    {
        // Implement response parsing logic
        return new RemediationValidationResult();
    }
} 