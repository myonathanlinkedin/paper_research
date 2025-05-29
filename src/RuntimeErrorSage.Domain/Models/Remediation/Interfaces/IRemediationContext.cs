using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Model.Models.Error;

namespace RuntimeErrorSage.Model.Models.Remediation.Interfaces
{
    public interface IRemediationContext
    {
        ErrorAnalysisResult AnalysisResult { get; }
        RemediationPlan Plan { get; }
        IRemediationAction CurrentAction { get; }
        List<RemediationResult> History { get; }
        Dictionary<string, object> ContextData { get; }
        bool IsRollbackMode { get; set; }
        RemediationState State { get; }

        void UpdateContext(RemediationResult result);
        void ClearContext();
        void SetCurrentAction(IRemediationAction action);
    }
} 
