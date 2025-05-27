using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Services.Remediation
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
        Task<RollbackStatus> RollbackActionAsync(string actionId);
    }
} 