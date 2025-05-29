using RuntimeErrorSage.Application.Models.Validation;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public interface IValidationRuleProvider
    {
        IEnumerable<ValidationRule> GetRules();
        void AddRule(ValidationRule rule);
        void RemoveRule(ValidationRule rule);
    }
} 