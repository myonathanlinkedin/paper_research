using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Graph;

namespace RuntimeErrorSage.Core.Interfaces
{
    /// <summary>
    /// Interface for analyzing error context graphs.
    /// </summary>
    public interface IGraphAnalyzer
    {
        /// <summary>
        /// Gets whether the analyzer is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the analyzer name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the analyzer version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Analyzes an error context graph.
        /// </summary>
        /// <param name="context">The error context.</param>
        /// <returns>The graph analysis result.</returns>
        Task<GraphAnalysis> AnalyzeContextGraphAsync(ErrorContext context);

        /// <summary>
        /// Gets the impact analysis for a component.
        /// </summary>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The impact analysis result.</returns>
        Task<ImpactAnalysisResult> GetComponentImpactAsync(string componentId, ErrorContext context);

        /// <summary>
        /// Gets the dependency analysis for a component.
        /// </summary>
        /// <param name="componentId">The component identifier.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The dependency analysis result.</returns>
        Task<DependencyAnalysisResult> GetComponentDependenciesAsync(string componentId, ErrorContext context);
    }

    /// <summary>
    /// Represents an impact analysis result.
    /// </summary>
    public class ImpactAnalysisResult
    {
        /// <summary>
        /// Gets or sets the impacted component identifier.
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the impact severity.
        /// </summary>
        public ImpactSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the impact scope.
        /// </summary>
        public ImpactScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the impact metrics.
        /// </summary>
        public Dictionary<string, double> ImpactMetrics { get; set; } = new();

        /// <summary>
        /// Gets or sets the affected nodes.
        /// </summary>
        public List<string> AffectedNodes { get; set; } = new();
    }

    /// <summary>
    /// Represents a dependency analysis result.
    /// </summary>
    public class DependencyAnalysisResult
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        public string ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the direct dependencies.
        /// </summary>
        public List<DependencyInfo> DirectDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the indirect dependencies.
        /// </summary>
        public List<DependencyInfo> IndirectDependencies { get; set; } = new();

        /// <summary>
        /// Gets or sets the dependency metrics.
        /// </summary>
        public Dictionary<string, double> DependencyMetrics { get; set; } = new();
    }

    /// <summary>
    /// Represents dependency information.
    /// </summary>
    public class DependencyInfo
    {
        /// <summary>
        /// Gets or sets the dependency identifier.
        /// </summary>
        public string DependencyId { get; set; }

        /// <summary>
        /// Gets or sets the dependency type.
        /// </summary>
        public DependencyType Type { get; set; }

        /// <summary>
        /// Gets or sets the dependency strength.
        /// </summary>
        public double Strength { get; set; }

        /// <summary>
        /// Gets or sets whether the dependency is critical.
        /// </summary>
        public bool IsCritical { get; set; }
    }

    /// <summary>
    /// Specifies the impact severity.
    /// </summary>
    public enum ImpactSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// Specifies the impact scope.
    /// </summary>
    public enum ImpactScope
    {
        Local,
        Component,
        Service,
        System
    }

    /// <summary>
    /// Specifies the dependency type.
    /// </summary>
    public enum DependencyType
    {
        Runtime,
        Compile,
        Development,
        Test
    }
} 