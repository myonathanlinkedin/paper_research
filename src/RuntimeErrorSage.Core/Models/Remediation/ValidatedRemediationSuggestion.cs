using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
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
