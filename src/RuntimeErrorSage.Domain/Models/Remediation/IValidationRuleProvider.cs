using RuntimeErrorSage.Domain.Models.Validation;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    public interface IValidationRuleProvider
    {
        IEnumerable<ValidationRule> GetRules();
        void AddRule(ValidationRule rule);
        void RemoveRule(ValidationRule rule);
    }
} 
