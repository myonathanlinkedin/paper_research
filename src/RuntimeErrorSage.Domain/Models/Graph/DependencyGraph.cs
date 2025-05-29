using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Models.Graph
{
    /// <summary>
    /// Represents a dependency graph for error analysis.
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
        public string Name { get; set; } = "Dependency Graph";

        /// <summary>
        /// Gets or sets the description of the graph.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the graph.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update time of the graph.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the nodes in the graph.
        /// </summary>
        public List<GraphNode> Nodes { get; set; } = new List<GraphNode>();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public List<GraphEdge> Edges { get; set; } = new List<GraphEdge>();

        /// <summary>
        /// Gets or sets the root node of the graph.
        /// </summary>
        public GraphNode RootNode { get; set; }

        /// <summary>
        /// Gets or sets metadata associated with the graph.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(GraphNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!Nodes.Any(n => n.Id == node.Id))
            {
                Nodes.Add(node);
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
            var sourceNode = Nodes.FirstOrDefault(n => n.Id == sourceNodeId);
            var targetNode = Nodes.FirstOrDefault(n => n.Id == targetNodeId);

            if (sourceNode == null)
                throw new ArgumentException($"Source node with ID {sourceNodeId} not found.");
            if (targetNode == null)
                throw new ArgumentException($"Target node with ID {targetNodeId} not found.");

            var edge = new GraphEdge
            {
                SourceNodeId = sourceNodeId,
                TargetNodeId = targetNodeId,
                Label = label
            };

            Edges.Add(edge);
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
                neighborIds = Edges.Where(e => e.TargetNodeId == nodeId).Select(e => e.SourceNodeId).ToList();
            }
            else if (direction == EdgeDirection.Outgoing)
            {
                neighborIds = Edges.Where(e => e.SourceNodeId == nodeId).Select(e => e.TargetNodeId).ToList();
            }
            else // Both
            {
                neighborIds = Edges.Where(e => e.SourceNodeId == nodeId || e.TargetNodeId == nodeId)
                    .Select(e => e.SourceNodeId == nodeId ? e.TargetNodeId : e.SourceNodeId)
                    .ToList();
            }

            return Nodes.Where(n => neighborIds.Contains(n.Id)).ToList();
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
            var currentNode = Nodes.FirstOrDefault(n => n.Id == currentNodeId);
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
