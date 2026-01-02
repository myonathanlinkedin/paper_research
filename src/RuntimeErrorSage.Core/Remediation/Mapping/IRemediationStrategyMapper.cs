using RuntimeErrorSage.Application.Interfaces;
using RuntimeErrorSage.Domain.Interfaces;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Core.Remediation.Mapping
{
    /// <summary>
    /// Maps between Application and Domain remediation strategy interfaces.
    /// This preserves DDD architecture by keeping the domain layer independent.
    /// </summary>
    public interface IRemediationStrategyMapper
    {
        /// <summary>
        /// Converts an Application IRemediationStrategy to a Domain IRemediationStrategy.
        /// </summary>
        Domain.Interfaces.IRemediationStrategy ToDomain(Application.Interfaces.IRemediationStrategy appStrategy);

        /// <summary>
        /// Converts a Domain IRemediationStrategy to an Application IRemediationStrategy.
        /// </summary>
        Application.Interfaces.IRemediationStrategy ToApplication(Domain.Interfaces.IRemediationStrategy domainStrategy);

        /// <summary>
        /// Converts a RemediationStrategyModel to a Domain IRemediationStrategy.
        /// </summary>
        Domain.Interfaces.IRemediationStrategy ToDomain(RemediationStrategyModel model);

        /// <summary>
        /// Converts a RemediationStrategyModel to an Application IRemediationStrategy.
        /// </summary>
        Application.Interfaces.IRemediationStrategy ToApplication(RemediationStrategyModel model);
    }
}







