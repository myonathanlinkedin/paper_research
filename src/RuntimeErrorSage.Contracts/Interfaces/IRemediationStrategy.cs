using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Contracts.Interfaces
{
    /// <summary>
    /// Interface for remediation strategies.
    /// </summary>
    public interface IRemediationStrategy
    {
        /// <summary>
        /// Gets the unique identifier for this strategy.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the name of the strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the strategy.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the version of the strategy.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets whether the strategy is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the priority of the strategy.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets the strategy parameters.
        /// </summary>
        IReadOnlyDictionary<string, object> Parameters { get; }

        /// <summary>
        /// Gets the supported error types.
        /// </summary>
        ISet<string> SupportedErrorTypes { get; }

        /// <summary>
        /// Gets the creation timestamp.
        /// </summary>
        DateTime CreatedAt { get; }
    }
} 