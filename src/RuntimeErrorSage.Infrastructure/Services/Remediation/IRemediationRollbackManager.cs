using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Services.Remediation
{
    /// <summary>
    /// Interface for managing remediation rollbacks.
    /// </summary>
    public interface IRemediationRollbackManager
    {
        /// <summary>
        /// Rolls back a remediation action.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <returns>The rollback status.</returns>
        Task<RuntimeErrorSage.Domain.Enums.RollbackStatus> RollbackActionAsync(string actionId);
    }
} 
