using RuntimeErrorSage.Domain.Models.Validation;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    public class DefaultValidationResultStorage : IValidationResultStorage
    {
        private readonly Dictionary<string, ValidationResult> _results = new();

        public void StoreResult(string actionId, ValidationResult result)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(result);
            _results[actionId] = result;
        }

        public ValidationResult GetResult(string actionId)
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
