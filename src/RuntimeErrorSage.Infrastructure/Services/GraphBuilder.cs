using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Graph;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Infrastructure.Services;

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
                UpdatedAt = DateTime.UtcNow,
                CorrelationId = context.CorrelationId,
                Timestamp = DateTime.UtcNow
            };

            var nodesDict = new Dictionary<string, DependencyNode>();

            // Add nodes from component graph
            if (context.ComponentGraph != null)
            {
                foreach (var (componentId, dependencies) in context.ComponentGraph)
                {
                    // Add the component node
                    var node = new DependencyNode
                    {
                        Id = componentId,
                        Name = componentId,
                        NodeType = GraphNodeType.Component,
                        Type = GraphNodeType.Component.ToString(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Metadata = new Dictionary<string, string>()
                    };

                    graph.Nodes.Add(node);
                    nodesDict[componentId] = node;

                    // Create edges for dependencies (but add them later when all nodes exist)
                    foreach (var dependencyId in dependencies)
                    {
                        // We'll add the actual edges after creating all nodes
                        if (!nodesDict.ContainsKey(dependencyId))
                        {
                            var depNode = new DependencyNode
                            {
                                Id = dependencyId,
                                Name = dependencyId,
                                NodeType = GraphNodeType.Component,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            
                            graph.Nodes.Add(depNode);
                            nodesDict[dependencyId] = depNode;
                        }
                    }
                }
                
                // Now create the edges
                foreach (var (componentId, dependencies) in context.ComponentGraph)
                {
                    var sourceNode = nodesDict[componentId];
                    
                    foreach (var dependencyId in dependencies)
                    {
                        var targetNode = nodesDict[dependencyId];
                        
                        var edge = new DependencyEdge
                        {
                            SourceId = sourceNode.Id,
                            TargetId = targetNode.Id,
                            Source = sourceNode,
                            Target = targetNode,
                            Label = "depends_on",
                            IsDirected = true
                        };

                        graph.Edges.Add(edge);
                    }
                }
            }

            // Add error source node if available
            if (!string.IsNullOrEmpty(context.ErrorSource))
            {
                var errorNodeId = context.ErrorSource;
                DependencyNode errorNode;
                
                if (nodesDict.ContainsKey(errorNodeId))
                {
                    // Update existing node
                    errorNode = nodesDict[errorNodeId];
                    errorNode.IsErrorSource = true;
                    errorNode.NodeType = GraphNodeType.Error;
                    errorNode.Type = GraphNodeType.Error.ToString();
                    
                    if (errorNode.Metadata == null)
                    {
                        errorNode.Metadata = new Dictionary<string, string>();
                    }
                    
                    errorNode.Metadata["ErrorType"] = context.ErrorType;
                    errorNode.Metadata["ErrorMessage"] = context.Message;
                }
                else
                {
                    // Create new error node
                    errorNode = new DependencyNode
                    {
                        Id = errorNodeId,
                        Name = "Error Source",
                        NodeType = GraphNodeType.Error,
                        Type = GraphNodeType.Error.ToString(),
                        IsErrorSource = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Metadata = new Dictionary<string, string>
                        {
                            { "ErrorType", context.ErrorType },
                            { "ErrorMessage", context.Message }
                        }
                    };

                    graph.Nodes.Add(errorNode);
                    nodesDict[errorNodeId] = errorNode;
                }
            }

            // Add metadata
            graph.Metadata["ErrorId"] = context.ErrorId;
            graph.Metadata["Timestamp"] = DateTime.UtcNow.ToString("o");
            graph.Metadata["ComponentCount"] = graph.Nodes.Count.ToString();
            graph.Metadata["EdgeCount"] = graph.Edges.Count.ToString();

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
