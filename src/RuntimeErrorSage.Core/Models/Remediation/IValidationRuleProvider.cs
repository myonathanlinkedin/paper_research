using RuntimeErrorSage.Core.Models.Validation;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    public interface IValidationRuleProvider
    {
        IEnumerable<ValidationRule> GetRules();
        void AddRule(ValidationRule rule);
        void RemoveRule(ValidationRule rule);
    }
} 