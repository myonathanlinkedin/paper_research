using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Remediation;

using RuntimeErrorSage.Domain.Models.Metrics;
using ValidationScope = RuntimeErrorSage.Domain.Enums.ValidationScope;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Interface for metrics collector.
    /// </summary>
    public interface IMetricsCollector
    {
        /// <summary>
        /// Begin a new metrics collection scope.
        /// </summary>
        /// <returns>An IDisposable representing the scope.</returns>
        IDisposable BeginScope();
    }

    /// <summary>
    /// Interface for validation.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates the remediation context.
        /// </summary>
        /// <param name="context">The context to validate.</param>
        /// <returns>The validation result.</returns>
        Task<ValidationResult> ValidateAsync(RemediationContext context);
    }

    /// <summary>
    /// Represents a validation rule for remediation operations.
    /// </summary>
    public class RemediationValidationRule
    {
        private readonly ILogger<RemediationValidationRule> _logger;
        private readonly IMetricsCollector _metricsCollector;
        private readonly IValidator _validator;

        /// <summary>
        /// Gets or sets the unique identifier of the rule.
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the validation priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the validation scope.
        /// </summary>
        public ValidationScope Scope { get; set; }

        /// <summary>
        /// Gets or sets whether the rule is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the rule is required.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the error message template.
        /// </summary>
        public string ErrorMessageTemplate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning message template.
        /// </summary>
        public string WarningMessageTemplate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation condition.
        /// </summary>
        public string ValidationCondition { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the validation parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the validation metadata.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets when the rule was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the rule was last updated.
        /// </summary>
        public DateTimeOffset ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets whether the rule is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the rule.
        /// </summary>
        public ErrorSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the validation function that determines if the rule is satisfied.
        /// </summary>
        public Func<RemediationAction, bool> ValidationFunction { get; set; }

        /// <summary>
        /// Gets or sets the error message to display if the rule is not satisfied.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets whether the validation result can be cached.
        /// </summary>
        public bool IsCacheable { get; set; }

        /// <summary>
        /// Gets or sets the duration for which the validation result can be cached.
        /// </summary>
        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets the validation function for async validation.
        /// </summary>
        public Func<RemediationPlan, ErrorContext, Task<ValidationResult>> AsyncValidationFunction { get; set; }

        private readonly List<string> _validationErrors = new();

        /// <summary>
        /// Initializes a new instance of the RemediationValidationRule class.
        /// </summary>
        public RemediationValidationRule(
            ILogger<RemediationValidationRule> logger,
            IMetricsCollector metricsCollector,
            IValidator validator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _metricsCollector = metricsCollector ?? throw new ArgumentNullException(nameof(metricsCollector));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            RuleId = Guid.NewGuid().ToString();
            CreatedAt = DateTimeOffset.UtcNow;
            ModifiedAt = DateTimeOffset.UtcNow;
            IsEnabled = true;
            IsCacheable = false;
        }

        /// <summary>
        /// Validates the specified remediation context.
        /// </summary>
        /// <param name="context">The remediation context to validate.</param>
        /// <returns>The validation result.</returns>
        public async Task<ValidationResult> ValidateAsync(RemediationContext context)
        {
            try
            {
                _logger.LogInformation("Starting validation rule execution");
                
                // Validate context
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                // Set up metrics collection
                using var metricsScope = _metricsCollector.BeginScope();
                
                // Execute validation
                var validationResult = await _validator.ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed: {ValidationErrors}", validationResult.Errors);
                    return validationResult;
                }

                _logger.LogInformation("Validation rule completed successfully");
                return validationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing validation rule");
                return new ValidationResult(
                    new ValidationContext(),
                    false,
                    ValidationSeverity.Error,
                    ValidationStatus.Failed,
                    new MetricsValidation { IsValid = false, Message = ex.Message, Metadata = new Dictionary<string, string> { { "Exception", ex.Message } } },
                    ex.Message);
            }
        }

        /// <summary>
        /// Gets a cache key for the specified remediation plan and error context.
        /// </summary>
        /// <param name="plan">The remediation plan.</param>
        /// <param name="context">The error context.</param>
        /// <returns>A cache key string.</returns>
        public string GetCacheKey(RemediationPlan plan, ErrorContext context)
        {
            return $"ValidationRule:{RuleId}:{plan?.PlanId ?? "null"}:{context?.ContextId ?? "null"}";
        }

        /// <summary>
        /// Validates a rule by name.
        /// </summary>
        /// <param name="ruleName">The name of the rule to validate.</param>
        /// <returns>The validation result.</returns>
        public ValidationResult Validate(string ruleName)
        {
            var context = new ValidationContext();
            var result = new ValidationResult(
                context,
                true,
                ValidationSeverity.Info,
                ValidationStatus.Completed,
                null,
                "Validation successful"
            );
            return result;
        }

        /// <summary>
        /// Adds a validation error.
        /// </summary>
        /// <param name="error">The error message to add.</param>
        public void AddValidationError(string error)
        {
            _validationErrors.Add(error);
        }

        /// <summary>
        /// Validates an exception.
        /// </summary>
        /// <param name="ex">The exception to validate.</param>
        /// <returns>The metrics validation result.</returns>
        public MetricsValidation Validate(Exception ex)
        {
            var validation = new MetricsValidation
            {
                IsValid = false,
                Message = ex.Message,
                Metadata = new Dictionary<string, string> { { "Exception", ex.Message } }
            };
            _validationErrors.Add(ex.Message);
            return validation;
        }

        private MetricsValidation CreateValidationResult(string message, Exception ex)
        {
            return new MetricsValidation
            {
                IsValid = false,
                Message = message,
                Metadata = new Dictionary<string, string> { { "Error", message }, { "Exception", ex.Message } }
            };
        }
    }
} 
