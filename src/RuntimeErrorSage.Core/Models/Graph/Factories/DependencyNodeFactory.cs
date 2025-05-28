using System;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Models.Graph.Factories
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
                Label = label,
                ComponentId = componentId,
                ComponentName = componentName,
                IsErrorSource = isErrorSource
            };
        }
    }
} 