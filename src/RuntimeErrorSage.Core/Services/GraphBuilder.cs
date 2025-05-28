using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;
using RuntimeErrorSage.Core.Services.Interfaces;

namespace RuntimeErrorSage.Core.Services;

/// <summary>
/// Service for building dependency graphs from error contexts.
/// </summary>
public class GraphBuilder : IGraphBuilder
{
    private readonly ILogger<GraphBuilder> _logger;

    public GraphBuilder(ILogger<GraphBuilder> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<DependencyGraph> BuildGraphAsync(ErrorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            _logger.LogInformation("Building dependency graph for error {ErrorId}", context.ErrorId);

            var graph = new DependencyGraph
            {
                Name = $"Dependency Graph for Error {context.ErrorId}",
                Description = $"Dependency graph built from error context at {DateTime.UtcNow}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add nodes from component graph
            if (context.ComponentGraph != null)
            {
                foreach (var (componentId, dependencies) in context.ComponentGraph)
                {
                    // Add the component node
                    var node = new GraphNode
                    {
                        Id = componentId,
                        Name = componentId,
                        Type = GraphNodeType.Component,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Metadata = new Dictionary<string, object>()
                    };

                    graph.Nodes[componentId] = node;

                    // Add edges for dependencies
                    foreach (var dependencyId in dependencies)
                    {
                        var edge = new GraphEdge
                        {
                            Source = node,
                            Target = new GraphNode { Id = dependencyId },
                            Type = GraphEdgeType.Dependency,
                            Weight = 1.0,
                            Label = "depends_on",
                            IsDirected = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        graph.Edges.Add(edge);
                    }
                }
            }

            // Add error source node if available
            if (!string.IsNullOrEmpty(context.ErrorSource))
            {
                var errorNode = new GraphNode
                {
                    Id = context.ErrorSource,
                    Name = "Error Source",
                    Type = GraphNodeType.Error,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Metadata = new Dictionary<string, object>
                    {
                        { "ErrorType", context.ErrorType },
                        { "ErrorMessage", context.Message }
                    }
                };

                graph.Nodes[context.ErrorSource] = errorNode;
            }

            // Add metadata
            graph.Metadata["ErrorId"] = context.ErrorId;
            graph.Metadata["Timestamp"] = DateTime.UtcNow;
            graph.Metadata["ComponentCount"] = graph.Nodes.Count;
            graph.Metadata["EdgeCount"] = graph.Edges.Count;

            await Task.CompletedTask;
            return graph;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error building dependency graph for error {ErrorId}", context.ErrorId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ValidateConfigurationAsync()
    {
        try
        {
            // Add any configuration validation logic here
            await Task.CompletedTask;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating graph builder configuration");
            return false;
        }
    }
} 
