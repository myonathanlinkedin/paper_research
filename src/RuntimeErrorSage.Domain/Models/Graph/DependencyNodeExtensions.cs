using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RuntimeErrorSage.Domain.Models.Graph
{
    /// <summary>
    /// Extension methods for DependencyNode.
    /// </summary>
    public static class DependencyNodeExtensions
    {
        /// <summary>
        /// Gets all nodes as values.
        /// </summary>
        /// <param name="nodes">The list of nodes.</param>
        /// <returns>The nodes as values.</returns>
        public static IEnumerable<DependencyNode> Values(this List<DependencyNode> nodes)
        {
            return nodes ?? Enumerable.Empty<DependencyNode>();
        }
    }
    
    /// <summary>
    /// Extension class to provide a Values property for List<DependencyNode>.
    /// </summary>
    public class DependencyNodeList : List<DependencyNode>
    {
        /// <summary>
        /// Gets the nodes as values.
        /// </summary>
        public IEnumerable<DependencyNode> Values => this;
    }
} 