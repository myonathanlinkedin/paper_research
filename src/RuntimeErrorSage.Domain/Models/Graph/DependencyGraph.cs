using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Represents a graph of component dependencies.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Gets or sets the unique identifier for this graph.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the graph.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the nodes in the graph as a dictionary.
        /// </summary>
        private Dictionary<string, GraphNode> NodesDict { get; set; } = new();

        /// <summary>
        /// Gets or sets the nodes in the graph.
        /// </summary>
        public List<DependencyNode> Nodes { get; set; } = new();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public List<DependencyEdge> Edges { get; set; } = new();

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the graph was created.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the dependencies in the graph.
        /// </summary>
        public Dictionary<string, ComponentDependency> Dependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the graph was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the graph was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the version of the graph.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the graph.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the graph is active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the status of the graph.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional metadata about the graph.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the graph edges.
        /// </summary>
        public List<GraphEdge> GraphEdges { get; set; } = new();

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(GraphNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!NodesDict.ContainsKey(node.Id))
            {
                NodesDict.Add(node.Id, node);
            }
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="sourceNodeId">The ID of the source node.</param>
        /// <param name="targetNodeId">The ID of the target node.</param>
        /// <param name="label">The edge label.</param>
        /// <returns>The created edge.</returns>
        public GraphEdge AddEdge(string sourceNodeId, string targetNodeId, string label = "depends_on")
        {
            var sourceNode = NodesDict.Values.FirstOrDefault(n => n.Id == sourceNodeId);
            var targetNode = NodesDict.Values.FirstOrDefault(n => n.Id == targetNodeId);

            if (sourceNode == null)
                throw new ArgumentException($"Source node with ID {sourceNodeId} not found.");
            if (targetNode == null)
                throw new ArgumentException($"Target node with ID {targetNodeId} not found.");

            var edge = new GraphEdge(sourceNode, targetNode, label);
            GraphEdges.Add(edge);
            return edge;
        }

        /// <summary>
        /// Gets the neighbors of a node.
        /// </summary>
        /// <param name="nodeId">The ID of the node.</param>
        /// <param name="direction">The direction of the relationships to consider.</param>
        /// <returns>The neighboring nodes.</returns>
        public List<GraphNode> GetNeighbors(string nodeId, EdgeDirection direction = EdgeDirection.Outgoing)
        {
            List<string> neighborIds;

            if (direction == EdgeDirection.Incoming)
            {
                neighborIds = GraphEdges.Where(e => e.TargetId == nodeId).Select(e => e.SourceId).ToList();
            }
            else if (direction == EdgeDirection.Outgoing)
            {
                neighborIds = GraphEdges.Where(e => e.SourceId == nodeId).Select(e => e.TargetId).ToList();
            }
            else // Both
            {
                neighborIds = GraphEdges.Where(e => e.SourceId == nodeId || e.TargetId == nodeId)
                    .Select(e => e.SourceId == nodeId ? e.TargetId : e.SourceId)
                    .ToList();
            }

            return NodesDict.Values.Where(n => neighborIds.Contains(n.Id)).ToList();
        }

        /// <summary>
        /// Gets the path between two nodes.
        /// </summary>
        /// <param name="sourceNodeId">The ID of the source node.</param>
        /// <param name="targetNodeId">The ID of the target node.</param>
        /// <returns>The path as a list of nodes.</returns>
        public GraphPath FindPath(string sourceNodeId, string targetNodeId)
        {
            var visited = new HashSet<string>();
            var path = new List<GraphNode>();
            var pathFound = FindPathDFS(sourceNodeId, targetNodeId, visited, path);

            return new GraphPath
            {
                Nodes = path,
                SourceNodeId = sourceNodeId,
                TargetNodeId = targetNodeId,
                IsComplete = pathFound
            };
        }

        private bool FindPathDFS(string currentNodeId, string targetNodeId, HashSet<string> visited, List<GraphNode> path)
        {
            visited.Add(currentNodeId);
            var currentNode = NodesDict.Values.FirstOrDefault(n => n.Id == currentNodeId);
            if (currentNode != null)
            {
                path.Add(currentNode);
            }

            if (currentNodeId == targetNodeId)
            {
                return true;
            }

            var neighbors = GetNeighbors(currentNodeId, EdgeDirection.Outgoing);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor.Id))
                {
                    if (FindPathDFS(neighbor.Id, targetNodeId, visited, path))
                    {
                        return true;
                    }
                }
            }

            // If we get here, this node doesn't lead to the target
            path.RemoveAt(path.Count - 1);
            return false;
        }
    }
}
