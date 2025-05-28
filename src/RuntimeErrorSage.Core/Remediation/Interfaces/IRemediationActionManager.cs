using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    public interface IRemediationActionManager
    {
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action);
        Task<ValidationResult> ValidateActionAsync(RemediationAction action);
        Task<RollbackStatus> RollbackActionAsync(string actionId);
        Task<RemediationResult> GetActionStatusAsync(string actionId);
    }
} 


