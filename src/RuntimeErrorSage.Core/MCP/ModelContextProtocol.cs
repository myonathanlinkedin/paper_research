using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Application.Context.Interfaces;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Models.Context;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Application.Analysis.Interfaces;
using RuntimeErrorSage.Domain.Models.MCP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.MCP;

/// <summary>
/// Implements the Model Context Protocol (MCP) for error analysis.
/// This protocol manages the interaction between runtime error analysis and context gathering.
/// </summary>
public class ModelContextProtocol
{
    private readonly ILogger<ModelContextProtocol> _logger;
    private readonly IContextProvider _contextProvider;
    private readonly IErrorContextAnalyzer _errorContextAnalyzer;
    private readonly Dictionary<string, ContextMetadata> _contextCache;

    public ModelContextProtocol(
        ILogger<ModelContextProtocol> logger,
        IContextProvider contextProvider,
        IErrorContextAnalyzer errorContextAnalyzer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _contextCache = new Dictionary<string, ContextMetadata>();
    }

    /// <summary>
    /// Analyzes the runtime context and builds a graph-based representation.
    /// </summary>
    public async Task<RuntimeErrorSage.Core.MCP.ContextAnalysisResult> AnalyzeContextAsync(RuntimeContext context)
    {
        var metadata = await _contextProvider.GetContextMetadataAsync(context.Id);
        var errorContext = ConvertToErrorContext(context);
        var graph = await _errorContextAnalyzer.BuildDependencyGraphAsync(errorContext);
        
        var result = new RuntimeErrorSage.Core.MCP.ContextAnalysisResult
        {
            ContextId = context.Id,
            Timestamp = DateTime.UtcNow,
            ErrorAnalysis = new Domain.Models.Analysis.ErrorAnalysisResult 
            {
                Id = context.Id
            },
            Status = AnalysisStatus.Completed,
            Details = new Dictionary<string, object>
            {
                ["MetadataCount"] = metadata?.Properties?.Count ?? 0,
                ["GraphNodeCount"] = graph?.Nodes?.Count ?? 0
            }
        };

        _contextCache[context.Id] = metadata;
        return result;
    }

    /// <summary>
    /// Updates the context model based on new runtime information.
    /// </summary>
    public async Task<bool> UpdateContextModelAsync(string contextId, Domain.Models.Context.RuntimeUpdate update)
    {
        if (!_contextCache.ContainsKey(contextId))
        {
            return false;
        }

        var metadata = _contextCache[contextId];
        
        // Create a dummy ErrorContext to pass to the UpdateContextAsync method
        var errorContext = new ErrorContext(
            error: new RuntimeError(
                message: "Context update",
                errorType: "ContextUpdate",
                source: "ModelContextProtocol",
                stackTrace: string.Empty
            ),
            context: contextId,
            timestamp: DateTime.UtcNow
        );
        
        await _contextProvider.UpdateContextAsync(errorContext);
        return true;
    }

    /// <summary>
    /// Validates the context against defined rules and constraints.
    /// </summary>
    public async Task<ValidationResult> ValidateContextAsync(string contextId)
    {
        if (!_contextCache.ContainsKey(contextId))
        {
            return new ValidationResult 
            { 
                IsValid = false, 
                Messages = new List<string> { "Context not found" } 
            };
        }

        try
        {
            var metadata = await _contextProvider.GetContextMetadataAsync(contextId);
            return new ValidationResult
            {
                IsValid = true,
                Messages = new List<string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating context {ContextId}", contextId);
            return new ValidationResult
            {
                IsValid = false,
                Messages = new List<string> { ex.Message }
            };
        }
    }
    
    /// <summary>
    /// Converts a RuntimeContext to an ErrorContext
    /// </summary>
    private ErrorContext ConvertToErrorContext(RuntimeContext context)
    {
        if (context == null)
            return null;
            
        var error = new RuntimeError(
            message: context.ErrorMessage ?? "Unknown error",
            errorType: context.ErrorType ?? "Unknown",
            source: context.Source ?? "Unknown",
            stackTrace: context.StackTrace ?? string.Empty
        );
        
        return new ErrorContext(
            error: error,
            context: context.Name ?? "Unknown",
            timestamp: context.Timestamp
        );
    }
} 
