using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Core.Models.Common;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Represents a dependency graph for analyzing component relationships.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Gets or sets the unique identifier of the graph.
        /// </summary>
        public string GraphId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the graph.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the graph.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the nodes in the graph.
        /// </summary>
        public Dictionary<string, GraphNode> Nodes { get; set; } = new();

        /// <summary>
        /// Gets or sets the edges in the graph.
        /// </summary>
        public List<GraphEdge> Edges { get; set; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the graph was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp when the graph was last updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the metadata of the graph.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(GraphNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            Nodes[node.Id] = node;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds an edge to the graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddEdge(GraphEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            if (!Nodes.ContainsKey(edge.Source.Id))
            {
                throw new ArgumentException($"Source node {edge.Source.Id} does not exist in the graph.");
            }

            if (!Nodes.ContainsKey(edge.Target.Id))
            {
                throw new ArgumentException($"Target node {edge.Target.Id} does not exist in the graph.");
            }

            Edges.Add(edge);
            Nodes[edge.Source.Id].AddOutgoingEdge(edge);
            Nodes[edge.Target.Id].AddIncomingEdge(edge);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the outgoing edges from a node.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The outgoing edges.</returns>
        public IEnumerable<GraphEdge> GetOutgoingEdges(string nodeId)
        {
            if (!Nodes.ContainsKey(nodeId))
            {
                throw new ArgumentException($"Node {nodeId} does not exist in the graph.");
            }

            return Nodes[nodeId].OutgoingEdges;
        }

        /// <summary>
        /// Gets the incoming edges to a node.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns>The incoming edges.</returns>
        public IEnumerable<GraphEdge> GetIncomingEdges(string nodeId)
        {
            if (!Nodes.ContainsKey(nodeId))
            {
                throw new ArgumentException($"Node {nodeId} does not exist in the graph.");
            }

            return Nodes[nodeId].IncomingEdges;
        }

        /// <summary>
        /// Removes a node from the graph.
        /// </summary>
        /// <param name="nodeId">The ID of the node to remove.</param>
        public void RemoveNode(string nodeId)
        {
            if (!Nodes.TryGetValue(nodeId, out var node))
            {
                throw new ArgumentException($"Node with ID {nodeId} not found in the graph.");
            }

            // Remove all edges connected to this node
            var edgesToRemove = node.IncomingEdges.Concat(node.OutgoingEdges).ToList();
            foreach (var edge in edgesToRemove)
            {
                RemoveEdge(edge.Id);
            }

            Nodes.Remove(nodeId);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an edge from the graph.
        /// </summary>
        /// <param name="edgeId">The ID of the edge to remove.</param>
        public void RemoveEdge(string edgeId)
        {
            if (!Edges.Contains(Edges.FirstOrDefault(e => e.Id == edgeId)))
            {
                throw new ArgumentException($"Edge with ID {edgeId} not found in the graph.");
            }

            var edge = Edges.First(e => e.Id == edgeId);
            edge.Remove();
            Edges.Remove(edge);
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets all nodes that depend on the specified node.
        /// </summary>
        /// <param name="nodeId">The ID of the node.</param>
        /// <returns>A list of nodes that depend on the specified node.</returns>
        public List<GraphNode> GetDependentNodes(string nodeId)
        {
            if (!Nodes.TryGetValue(nodeId, out var node))
            {
                throw new ArgumentException($"Node with ID {nodeId} not found in the graph.");
            }

            return node.OutgoingEdges.Select(e => e.Target).ToList();
        }

        /// <summary>
        /// Gets all nodes that the specified node depends on.
        /// </summary>
        /// <param name="nodeId">The ID of the node.</param>
        /// <returns>A list of nodes that the specified node depends on.</returns>
        public List<GraphNode> GetDependencyNodes(string nodeId)
        {
            if (!Nodes.TryGetValue(nodeId, out var node))
            {
                throw new ArgumentException($"Node with ID {nodeId} not found in the graph.");
            }

            return node.IncomingEdges.Select(e => e.Source).ToList();
        }

        /// <summary>
        /// Gets all nodes in the graph that have no incoming edges.
        /// </summary>
        /// <returns>A list of root nodes.</returns>
        public List<GraphNode> GetRootNodes()
        {
            return Nodes.Values.Where(n => n.IncomingEdges.Count == 0).ToList();
        }

        /// <summary>
        /// Gets all nodes in the graph that have no outgoing edges.
        /// </summary>
        /// <returns>A list of leaf nodes.</returns>
        public List<GraphNode> GetLeafNodes()
        {
            return Nodes.Values.Where(n => n.OutgoingEdges.Count == 0).ToList();
        }

        /// <summary>
        /// Gets the neighboring nodes for a given node.
        /// </summary>
        /// <param name="nodeId">The ID of the node.</param>
        /// <returns>A list of neighboring nodes.</returns>
        public List<GraphNode> GetNeighbors(string nodeId)
        {
            if (!Nodes.TryGetValue(nodeId, out var node))
            {
                throw new ArgumentException($"Node with ID {nodeId} not found in the graph.");
            }

            var dependencyNodes = node.IncomingEdges.Select(e => e.Source);
            var dependentNodes = node.OutgoingEdges.Select(e => e.Target);
            
            return dependencyNodes.Concat(dependentNodes).Distinct().ToList();
        }

        /// <summary>
        /// Gets the shortest path between two nodes.
        /// </summary>
        /// <param name="sourceId">The source node ID.</param>
        /// <param name="targetId">The target node ID.</param>
        /// <returns>A list of node IDs representing the path, or an empty list if no path exists.</returns>
        public List<string> GetPath(string sourceId, string targetId)
        {
            if (!Nodes.ContainsKey(sourceId))
            {
                throw new ArgumentException($"Source node with ID {sourceId} not found in the graph.");
            }

            if (!Nodes.ContainsKey(targetId))
            {
                throw new ArgumentException($"Target node with ID {targetId} not found in the graph.");
            }

            var visited = new HashSet<string>();
            var queue = new Queue<(string NodeId, List<string> Path)>();
            queue.Enqueue((sourceId, new List<string> { sourceId }));

            while (queue.Count > 0)
            {
                var (currentId, path) = queue.Dequeue();

                if (currentId == targetId)
                {
                    return path;
                }

                if (visited.Contains(currentId))
                {
                    continue;
                }

                visited.Add(currentId);

                if (Nodes.TryGetValue(currentId, out var node))
                {
                    foreach (var edge in node.OutgoingEdges)
                    {
                        if (!visited.Contains(edge.Target.Id))
                        {
                            var newPath = new List<string>(path) { edge.Target.Id };
                            queue.Enqueue((edge.Target.Id, newPath));
                        }
                    }
                }
            }

            return new List<string>(); // No path found
        }
    }
}