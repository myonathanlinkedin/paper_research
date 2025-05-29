using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Represents a validated remediation suggestion with a score.
    /// </summary>
    public class ValidatedRemediationSuggestion
    {
        public RemediationSuggestion Suggestion { get; set; }
        public RemediationValidationResult ValidationResult { get; set; }
        public double Score { get; set; }
    }
} 
