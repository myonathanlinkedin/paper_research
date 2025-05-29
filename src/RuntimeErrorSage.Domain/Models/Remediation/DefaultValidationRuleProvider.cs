using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Validation;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public class DefaultValidationRuleProvider : IValidationRuleProvider
    {
        private readonly Collection<ValidationRule> _rules = new();

        public IEnumerable<ValidationRule> GetRules() => _rules.AsReadOnly();

        public ValidationRule rule { ArgumentNullException.ThrowIfNull(ValidationRule rule); }
        {
            ArgumentNullException.ThrowIfNull(rule);
            _rules.Add(rule);
        }

        public ValidationRule rule { ArgumentNullException.ThrowIfNull(ValidationRule rule); }
        {
            ArgumentNullException.ThrowIfNull(rule);
            _rules.Remove(rule);
        }
    }
} 




