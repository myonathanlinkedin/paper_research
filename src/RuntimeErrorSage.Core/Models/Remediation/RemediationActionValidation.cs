using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Interface for validation rule providers.
    /// </summary>
    public interface IValidationRuleProvider
    {
        IEnumerable<ValidationRule> GetRules();
        void AddRule(ValidationRule rule);
        void RemoveRule(ValidationRule rule);
    }

    /// <summary>
    /// Interface for validation result handlers.
    /// </summary>
    public interface IValidationResultHandler
    {
        void HandleResult(ValidationResult result);
    }

    /// <summary>
    /// Interface for validation state checkers.
    /// </summary>
    public interface IValidationStateChecker
    {
        bool IsValidState(IRemediationAction action);
    }

    /// <summary>
    /// Interface for validation result storage.
    /// </summary>
    public interface IValidationResultStorage
    {
        void StoreResult(string actionId, ValidationResult result);
        ValidationResult GetResult(string actionId);
        void ClearResults();
    }

    /// <summary>
    /// Handles validation of remediation actions.
    /// </summary>
    public class RemediationActionValidation
    {
        private readonly IValidationRuleProvider _ruleProvider;
        private readonly IValidationResultStorage _resultStorage;
        private readonly IValidationStateChecker _stateChecker;
        private readonly List<IValidationResultHandler> _resultHandlers;

        public RemediationActionValidation(
            IValidationRuleProvider ruleProvider,
            IValidationResultStorage resultStorage,
            IValidationStateChecker stateChecker)
        {
            _ruleProvider = ruleProvider ?? throw new ArgumentNullException(nameof(ruleProvider));
            _resultStorage = resultStorage ?? throw new ArgumentNullException(nameof(resultStorage));
            _stateChecker = stateChecker ?? throw new ArgumentNullException(nameof(stateChecker));
            _resultHandlers = new List<IValidationResultHandler>();
        }

        /// <summary>
        /// Validates a remediation action.
        /// </summary>
        /// <param name="action">The action to validate.</param>
        /// <param name="context">The error context.</param>
        /// <returns>The validation result.</returns>
        public async Task<ValidationResult> ValidateActionAsync(IRemediationAction action, ErrorContext context)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(context);

            var result = new ValidationResult
            {
                ActionId = action.ActionId,
                Timestamp = DateTime.UtcNow,
                IsValid = true,
                Errors = new List<string>(),
                Warnings = new List<string>(),
                ValidationRules = new List<string>()
            };

            try
            {
                var startTime = DateTime.UtcNow;

                // Validate prerequisites
                await ValidatePrerequisitesAsync(action, context, result);

                // Validate dependencies
                await ValidateDependenciesAsync(action, context, result);

                // Validate parameters
                ValidateParameters(action, result);

                // Run custom validation rules
                await ValidateWithRulesAsync(action, result);

                // Validate action state
                ValidateActionState(action, result);

                result.DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
                _resultStorage.StoreResult(action.ActionId, result);
                NotifyResultHandlers(result);
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Validation failed: {ex.Message}");
                _resultStorage.StoreResult(action.ActionId, result);
                NotifyResultHandlers(result);
                return result;
            }
        }

        /// <summary>
        /// Gets the validation result for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The validation result, or null if not found.</returns>
        public ValidationResult GetValidationResult(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _resultStorage.GetResult(actionId);
        }

        /// <summary>
        /// Clears all validation results.
        /// </summary>
        public void ClearResults()
        {
            _resultStorage.ClearResults();
        }

        /// <summary>
        /// Registers a validation result handler.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        public void RegisterResultHandler(IValidationResultHandler handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _resultHandlers.Add(handler);
        }

        /// <summary>
        /// Unregisters a validation result handler.
        /// </summary>
        /// <param name="handler">The handler to unregister.</param>
        public void UnregisterResultHandler(IValidationResultHandler handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _resultHandlers.Remove(handler);
        }

        private async Task ValidatePrerequisitesAsync(IRemediationAction action, ErrorContext context, ValidationResult result)
        {
            if (action.Prerequisites?.Any() != true) return;

            foreach (var prerequisite in action.Prerequisites)
            {
                if (!await ValidatePrerequisiteAsync(prerequisite, context))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Prerequisite not met: {prerequisite}");
                }
            }
        }

        private async Task ValidateDependenciesAsync(IRemediationAction action, ErrorContext context, ValidationResult result)
        {
            if (action.Dependencies?.Any() != true) return;

            foreach (var dependency in action.Dependencies)
            {
                if (!await ValidateDependencyAsync(dependency, context))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Dependency not met: {dependency}");
                }
            }
        }

        private void ValidateParameters(IRemediationAction action, ValidationResult result)
        {
            if (action.Parameters?.Any() != true) return;

            foreach (var param in action.Parameters)
            {
                if (!ValidateParameter(param.Key, param.Value))
                {
                    result.IsValid = false;
                    result.Errors.Add($"Invalid parameter: {param.Key}");
                }
            }
        }

        private async Task ValidateWithRulesAsync(IRemediationAction action, ValidationResult result)
        {
            foreach (var rule in _ruleProvider.GetRules().Where(r => r.IsEnabled))
            {
                try
                {
                    result.ValidationRules.Add(rule.Name);
                    var ruleResult = await rule.ValidateAsync(action);
                    if (!ruleResult.IsValid)
                    {
                        result.IsValid = false;
                        result.Errors.AddRange(ruleResult.Errors);
                    }
                    if (ruleResult.Warnings?.Any() == true)
                    {
                        result.Warnings.AddRange(ruleResult.Warnings);
                    }
                }
                catch (Exception ex)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Error validating rule {rule.Name}: {ex.Message}");
                }
            }
        }

        private void ValidateActionState(IRemediationAction action, ValidationResult result)
        {
            if (!_stateChecker.IsValidState(action))
            {
                result.IsValid = false;
                result.Errors.Add("Invalid action state");
            }
        }

        private async Task<bool> ValidatePrerequisiteAsync(string prerequisite, ErrorContext context)
        {
            // Implement prerequisite validation logic
            return true;
        }

        private async Task<bool> ValidateDependencyAsync(string dependency, ErrorContext context)
        {
            // Implement dependency validation logic
            return true;
        }

        private bool ValidateParameter(string key, object value)
        {
            // Implement parameter validation logic
            return true;
        }

        private void NotifyResultHandlers(ValidationResult result)
        {
            foreach (var handler in _resultHandlers)
            {
                try
                {
                    handler.HandleResult(result);
                }
                catch
                {
                    // Ignore handler exceptions
                }
            }
        }
    }

    /// <summary>
    /// Default implementation of validation rule provider.
    /// </summary>
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

    /// <summary>
    /// Default implementation of validation result storage.
    /// </summary>
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

    /// <summary>
    /// Default implementation of validation state checker.
    /// </summary>
    public class DefaultValidationStateChecker : IValidationStateChecker
    {
        public bool IsValidState(IRemediationAction action)
        {
            if (action.Status == RemediationStatusEnum.Running)
            {
                return false;
            }

            if (action.RequiresManualApproval && action.Status != RemediationStatusEnum.WaitingForApproval)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Represents a validation rule for remediation actions.
    /// </summary>
    public class ValidationRule
    {
        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the validation function.
        /// </summary>
        public Func<IRemediationAction, Task<ValidationResult>> ValidateAsync { get; set; }

        /// <summary>
        /// Gets or sets the severity of the rule.
        /// </summary>
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;

        /// <summary>
        /// Gets or sets whether the rule is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }

    /// <summary>
    /// Represents the result of a validation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets or sets the ID of the action that was validated.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the validation.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the list of validation errors.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of validation warnings.
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of validation rules that were applied.
        /// </summary>
        public List<string> ValidationRules { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the validation duration in milliseconds.
        /// </summary>
        public long DurationMs { get; set; }
    }

    /// <summary>
    /// Represents the severity of a validation rule.
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// The rule is informational only.
        /// </summary>
        Info,

        /// <summary>
        /// The rule generates a warning.
        /// </summary>
        Warning,

        /// <summary>
        /// The rule generates an error.
        /// </summary>
        Error
    }
} 





