using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a validated remediation suggestion with a score.
    /// </summary>
    public class ValidatedRemediationSuggestion
    {
        public RemediationSuggestion Suggestion { get; }
        public RemediationValidationResult ValidationResult { get; }
        public double Score { get; }
    }
} 






