using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Extension methods for DependencyEdge.
    /// </summary>
    public static class DependencyEdgeExtensions
    {
        /// <summary>
        /// Gets the value of the edge (the edge itself).
        /// </summary>
        /// <param name="edge">The dependency edge.</param>
        /// <returns>The edge itself as the value.</returns>
        public static DependencyEdge Value(this DependencyEdge edge)
        {
            return edge;
        }
        
        /// <summary>
        /// Gets the source ID from the edge.
        /// </summary>
        /// <param name="edge">The dependency edge.</param>
        /// <returns>The source ID.</returns>
        public static string SourceId(this DependencyEdge edge)
        {
            return edge?.SourceId ?? string.Empty;
        }
        
        /// <summary>
        /// Gets the target ID from the edge.
        /// </summary>
        /// <param name="edge">The dependency edge.</param>
        /// <returns>The target ID.</returns>
        public static string TargetId(this DependencyEdge edge)
        {
            return edge?.TargetId ?? string.Empty;
        }
    }
} 