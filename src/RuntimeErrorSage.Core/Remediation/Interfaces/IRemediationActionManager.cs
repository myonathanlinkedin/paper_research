using RuntimeErrorSage.Model.Models.Remediation.Interfaces;
using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.Models.Validation;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Model.Remediation.Interfaces
{
    public interface IRemediationActionManager
    {
        Task<RemediationResult> ExecuteActionAsync(RemediationAction action);
        Task<ValidationResult> ValidateActionAsync(RemediationAction action);
        Task<RollbackStatus> RollbackActionAsync(string actionId);
        Task<RemediationResult> GetActionStatusAsync(string actionId);
    }
} 


