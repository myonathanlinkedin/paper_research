using RuntimeErrorSage.Model.Models.Validation;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Models.Remediation
{
    public class DefaultValidationRuleProvider : IValidationRuleProvider
    {
        private readonly List<ValidationRule> _rules = new();

        public IEnumerable<ValidationRule> GetRules() => _rules.AsReadOnly();

        public void AddRule(ValidationRule rule)
        {
            ArgumentNullException.ThrowIfNull(rule);
            _rules.Add(rule);
        }

        public void RemoveRule(ValidationRule rule)
        {
            ArgumentNullException.ThrowIfNull(rule);
            _rules.Remove(rule);
        }
    }
} 