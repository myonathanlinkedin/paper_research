using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.Models.Validation;

namespace RuntimeErrorSage.Model.Models.Remediation
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
