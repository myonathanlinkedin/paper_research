using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Models.Remediation
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
