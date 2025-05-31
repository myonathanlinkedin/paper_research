using System;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Graph;

namespace RuntimeErrorSage.Core.Graph
{
    /// <summary>
    /// Extension methods for GraphPath.
    /// </summary>
    public static class GraphPathExtensions
    {
        /// <summary>
        /// Determines whether a graph path contains any nodes.
        /// </summary>
        /// <param name="path">The graph path.</param>
        /// <returns>true if the path has any nodes; otherwise, false.</returns>
        public static bool Any(this GraphPath path)
        {
            if (path == null)
            {
                return false;
            }
            
            return path.Nodes != null && path.Nodes.Count > 0;
        }
        
        /// <summary>
        /// Determines whether a graph path contains a specific node.
        /// </summary>
        /// <param name="path">The graph path.</param>
        /// <param name="nodeId">The node ID to search for.</param>
        /// <returns>true if the path contains the specified node; otherwise, false.</returns>
        public static bool Contains(this GraphPath path, string nodeId)
        {
            if (path == null || path.Nodes == null || string.IsNullOrEmpty(nodeId))
            {
                return false;
            }
            
            return path.Nodes.Any(n => n.Id == nodeId);
        }
    }
} 