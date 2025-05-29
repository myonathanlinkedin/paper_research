using System.Collections.ObjectModel;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Graph;

namespace RuntimeErrorSage.Application.Extensions
{
    /// <summary>
    /// Extension methods for KeyValuePair objects in the RuntimeErrorSage codebase.
    /// </summary>
    public static class KeyValuePairExtensions
    {
        /// <summary>
        /// Gets the Id property from the Value of a KeyValuePair where Value is a GraphNode.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to get the Id from.</param>
        /// <returns>The Id of the GraphNode.</returns>
        public static string GetId(this KeyValuePair<string, GraphNode> kvp)
        {
            return kvp.Value.Id;
        }

        /// <summary>
        /// Gets the NodeType property from the Value of a KeyValuePair where Value is a GraphNode.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to get the NodeType from.</param>
        /// <returns>The Type of the GraphNode.</returns>
        public static string GetNodeType(this KeyValuePair<string, GraphNode> kvp)
        {
            return kvp.Value.Type;
        }

        /// <summary>
        /// Gets the ErrorProbability property from the Value of a KeyValuePair where Value is a GraphNode.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to get the ErrorProbability from.</param>
        /// <returns>The ErrorProbability of the GraphNode.</returns>
        public static double GetErrorProbability(this KeyValuePair<string, GraphNode> kvp)
        {
            return kvp.Value.ErrorProbability;
        }

        /// <summary>
        /// Gets the SourceId property from the Value of a KeyValuePair where Value is a GraphEdge.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to get the SourceId from.</param>
        /// <returns>The SourceId of the GraphEdge.</returns>
        public static string GetSourceId(this KeyValuePair<string, GraphEdge> kvp)
        {
            return kvp.Value.SourceId;
        }

        /// <summary>
        /// Gets the TargetId property from the Value of a KeyValuePair where Value is a GraphEdge.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to get the TargetId from.</param>
        /// <returns>The TargetId of the GraphEdge.</returns>
        public static string GetTargetId(this KeyValuePair<string, GraphEdge> kvp)
        {
            return kvp.Value.TargetId;
        }

        /// <summary>
        /// Converts a KeyValuePair with GraphNode Value to a DependencyNode.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to convert.</param>
        /// <returns>A new DependencyNode with data from the GraphNode.</returns>
        public static DependencyNode ToDependencyNode(this KeyValuePair<string, GraphNode> kvp)
        {
            var node = kvp.Value;
            return new DependencyNode
            {
                Id = node.Id,
                Name = node.Name,
                Type = node.Type,
                Metadata = new Dictionary<string, object>(node.Metadata)
            };
        }
    }
} 






