using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Models.Graph.Factories
{
    /// <summary>
    /// Interface for creating DependencyNode instances.
    /// </summary>
    public interface IDependencyNodeFactory
    {
        /// <summary>
        /// Creates a new DependencyNode instance.
        /// </summary>
        /// <param name="label">The node label.</param>
        /// <param name="componentId">The component ID.</param>
        /// <param name="componentName">The component name.</param>
        /// <param name="isErrorSource">Whether this node is an error source.</param>
        /// <returns>A new DependencyNode instance.</returns>
        DependencyNode Create(string label, string componentId, string componentName, bool isErrorSource = false);
    }
} 