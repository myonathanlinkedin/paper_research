using System;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Application.Remediation.Interfaces;
using RuntimeErrorSage.Core.Remediation.Base;
using DomainStrategy = RuntimeErrorSage.Domain.Interfaces.IRemediationStrategy;

namespace RuntimeErrorSage.Core.Remediation
{
    /// <summary>
    /// Extension methods for remediation strategies.
    /// </summary>
    public static class RemediationStrategyExtensions
    {
        /// <summary>
        /// Converts a domain strategy model to an application strategy.
        /// </summary>
        /// <param name="strategy">The domain strategy model to convert.</param>
        /// <returns>The application strategy.</returns>
        public static IRemediationStrategy ToApplicationStrategy(this RemediationStrategyModel strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            return new RemediationStrategyAdapter(strategy);
        }

        /// <summary>
        /// Converts a core strategy implementation to an application strategy interface.
        /// </summary>
        /// <param name="strategy">The core strategy to convert.</param>
        /// <returns>The application strategy interface.</returns>
        public static IRemediationStrategy ToApplicationStrategy(this RemediationStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            // The RemediationStrategy base class already implements IRemediationStrategy
            return strategy;
        }
        
        // Note: The ToDomainStrategy method is already defined in RemediationStrategy.cs
    }
} 