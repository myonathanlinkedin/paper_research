using System.Collections.ObjectModel;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Application.Models.Remediation.Interfaces
{
    public interface IRemediationContext
    {
        ErrorAnalysisResult AnalysisResult { get; }
        RemediationPlan Plan { get; }
        IRemediationAction CurrentAction { get; }
        Collection<RemediationResult> History { get; }
        Dictionary<string, object> ContextData { get; }
        bool IsRollbackMode { get; set; }
        RemediationState State { get; }

        void UpdateContext(RemediationResult result);
        void ClearContext();
        void SetCurrentAction(IRemediationAction action);
    }
} 






