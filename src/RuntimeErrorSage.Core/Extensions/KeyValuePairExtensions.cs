using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Graph;

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
            return kvp.Value.Source?.Id ?? string.Empty;
        }

        /// <summary>
        /// Gets the TargetId property from the Value of a KeyValuePair where Value is a GraphEdge.
        /// </summary>
        /// <param name="kvp">The KeyValuePair to get the TargetId from.</param>
        /// <returns>The TargetId of the GraphEdge.</returns>
        public static string GetTargetId(this KeyValuePair<string, GraphEdge> kvp)
        {
            return kvp.Value.Target?.Id ?? string.Empty;
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
                NodeType = node.NodeType,
                // GraphNode and DependencyNode both have Dictionary<string, string> Metadata
                Metadata = node.Metadata
            };
        }

        /// <summary>
        /// Converts a Dictionary with string values to a Dictionary with object values.
        /// </summary>
        /// <param name="source">The source dictionary with string values.</param>
        /// <returns>A new Dictionary with object values.</returns>
        public static Dictionary<string, object> ToObjectDictionary(this Dictionary<string, string> source)
        {
            if (source == null)
                return new Dictionary<string, object>();
                
            var result = new Dictionary<string, object>();
            foreach (var item in source)
            {
                result[item.Key] = item.Value;
            }
            
            return result;
        }
        
        /// <summary>
        /// Converts a Dictionary with object values to a Dictionary with string values.
        /// </summary>
        /// <param name="source">The source dictionary with object values.</param>
        /// <returns>A new Dictionary with string values.</returns>
        public static Dictionary<string, string> ToStringDictionary(this Dictionary<string, object> source)
        {
            if (source == null)
                return new Dictionary<string, string>();
                
            var result = new Dictionary<string, string>();
            foreach (var item in source)
            {
                result[item.Key] = item.Value?.ToString();
            }
            
            return result;
        }
    }
} 
