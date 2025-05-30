using RuntimeErrorSage.Domain.Models.Graph;
using System;

namespace RuntimeErrorSage.Domain.Models.Graph.Factories
{
    /// <summary>
    /// Factory for creating DependencyNode instances.
    /// </summary>
    public class DependencyNodeFactory : IDependencyNodeFactory
    {
        /// <summary>
        /// Creates a new DependencyNode instance.
        /// </summary>
        /// <param name="label">The node label.</param>
        /// <param name="componentId">The component ID.</param>
        /// <param name="componentName">The component name.</param>
        /// <param name="isErrorSource">Whether this node is an error source.</param>
        /// <returns>A new DependencyNode instance.</returns>
        public DependencyNode Create(string label, string componentId, string componentName, bool isErrorSource = false)
        {
            return new DependencyNode
            {
                Id = Guid.NewGuid().ToString(),
                Name = label, // Using Name instead of Label since DependencyNode inherits from GraphNode
                ComponentId = componentId,
                ComponentName = componentName,
                IsErrorSource = isErrorSource
            };
        }
    }
} 
