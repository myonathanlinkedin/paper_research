using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Interfaces;


namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Handles validation of remediation actions.
    /// </summary>
    public class RemediationActionValidation
    {
        private readonly List<string> _errors = new();
        private readonly List<string> _warnings = new();
        private readonly List<string> _validationRules = new();
        private readonly ValidationContext _context;
        private readonly IRemediationAction _action;

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        /// <summary>
        /// Gets the validation warnings.
        /// </summary>
        public IReadOnlyList<string> Warnings => _warnings.AsReadOnly();

        /// <summary>
        /// Gets the validation rules.
        /// </summary>
        public IReadOnlyList<string> ValidationRules => _validationRules.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationActionValidation"/> class.
        /// </summary>
        /// <param name="action">The remediation action to validate.</param>
        /// <param name="context">The validation context.</param>
        public RemediationActionValidation(IRemediationAction action, ValidationContext context)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Validates the action.
        /// </summary>
        /// <returns>The validation result.</returns>
        public async Task<ValidationResult> ValidateAsync()
        {
            _errors.Clear();
            _warnings.Clear();

            // Validate required properties
            if (string.IsNullOrEmpty(_action.Id))
                _errors.Add("Action ID is required");

            if (string.IsNullOrEmpty(_action.Name))
                _errors.Add("Action name is required");

            if (string.IsNullOrEmpty(_action.ActionType))
                _errors.Add("Action type is required");

            if (_action.Context == null)
                _errors.Add("Error context is required");

            // Create validation result with proper constructor parameters
            var result = new ValidationResult(
                _context,
                isValid: !_errors.Any(),
                severity: _errors.Any() ? ValidationSeverity.Error : ValidationSeverity.Info);
            
            // Add messages to the validation result
            foreach (var error in _errors)
            {
                result.AddError(error);
            }
            
            foreach (var warning in _warnings)
            {
                result.AddWarning(warning);
            }

            return result;
        }

        /// <summary>
        /// Adds a validation error.
        /// </summary>
        /// <param name="error">The error message.</param>
        public void AddError(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                _errors.Add(error);
            }
        }

        /// <summary>
        /// Adds a validation warning.
        /// </summary>
        /// <param name="warning">The warning message.</param>
        public void AddWarning(string warning)
        {
            if (!string.IsNullOrEmpty(warning))
            {
                _warnings.Add(warning);
            }
        }

        /// <summary>
        /// Adds a validation rule.
        /// </summary>
        /// <param name="rule">The validation rule.</param>
        public void AddRule(string rule)
        {
            if (!string.IsNullOrEmpty(rule))
            {
                _validationRules.Add(rule);
            }
        }

        /// <summary>
        /// Removes a validation rule.
        /// </summary>
        /// <param name="rule">The validation rule to remove.</param>
        public void RemoveRule(string rule)
        {
            if (!string.IsNullOrEmpty(rule))
            {
                _validationRules.Remove(rule);
            }
        }

        /// <summary>
        /// Gets all validation rules.
        /// </summary>
        /// <returns>A list of validation rules.</returns>
        public IReadOnlyList<string> GetRules()
        {
            return _validationRules.AsReadOnly();
        }
    }
} 





