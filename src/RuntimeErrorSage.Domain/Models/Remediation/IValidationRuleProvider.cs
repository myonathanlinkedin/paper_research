using RuntimeErrorSage.Model.Models.Validation;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    public interface IValidationRuleProvider
    {
        IEnumerable<ValidationRule> GetRules();
        void AddRule(ValidationRule rule);
        void RemoveRule(ValidationRule rule);
    }
} 