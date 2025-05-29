using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Validation;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    public class DefaultValidationResultStorage : IValidationResultStorage
    {
        private readonly Dictionary<string, ValidationResult> _results = new();

        public string actionId, ValidationResult result { ArgumentNullException.ThrowIfNull(string actionId, ValidationResult result); }
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(result);
            _results[actionId] = result;
        }

        public string actionId { ArgumentNullException.ThrowIfNull(string actionId); }
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _results.TryGetValue(actionId, out var result) ? result : null;
        }

        public void ClearResults()
        {
            _results.Clear();
        }
    }
} 





