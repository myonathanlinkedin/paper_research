using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Context.Interfaces;
using RuntimeErrorSage.Core.Interfaces;
using RuntimeErrorSage.Core.Models.Context;
using RuntimeErrorSage.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using RuntimeErrorSage.Core.Analysis.Interfaces;

namespace RuntimeErrorSage.Core.Models.MCP;

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
        IContextProvider contextProvider,
        IErrorContextAnalyzer errorContextAnalyzer)
    {
        _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
        _errorContextAnalyzer = errorContextAnalyzer ?? throw new ArgumentNullException(nameof(errorContextAnalyzer));
        _contextCache = new Dictionary<string, ContextMetadata>();
    }

    /// <summary>
    /// Analyzes the runtime context and builds a graph-based representation.
    /// </summary>
    public async Task<ContextAnalysisResult> AnalyzeContextAsync(RuntimeContext context)
    {
        var metadata = await _contextProvider.GetContextMetadataAsync(context);
        var graph = await _errorContextAnalyzer.BuildDependencyGraphAsync(context);
        
        var result = new ContextAnalysisResult
        {
            ContextId = context.Id,
            Timestamp = DateTime.UtcNow,
            Metadata = metadata,
            DependencyGraph = graph,
            Status = AnalysisStatus.Completed
        };

        _contextCache[context.Id] = metadata;
        return result;
    }

    /// <summary>
    /// Updates the context model based on new runtime information.
    /// </summary>
    public async Task<bool> UpdateContextModelAsync(string contextId, RuntimeUpdate update)
    {
        if (!_contextCache.ContainsKey(contextId))
        {
            return false;
        }

        var metadata = _contextCache[contextId];
        await _contextProvider.UpdateContextAsync(contextId, update, metadata);
        return true;
    }

    /// <summary>
    /// Validates the context against defined rules and constraints.
    /// </summary>
    public async Task<ValidationResult> ValidateContextAsync(string contextId)
    {
        if (!_contextCache.ContainsKey(contextId))
        {
            return new ValidationResult { IsValid = false, ErrorMessage = "Context not found" };
        }

        var metadata = _contextCache[contextId];
        return await _contextProvider.ValidateContextAsync(contextId, metadata);
    }
} 
