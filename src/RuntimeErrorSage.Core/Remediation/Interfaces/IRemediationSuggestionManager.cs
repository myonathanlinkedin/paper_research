using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Remediation.Interfaces
{
    public interface IRemediationSuggestionManager
    {
        Task<RemediationSuggestion> GetSuggestionsAsync(ErrorContext errorContext);
        Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
        Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
        Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
    }
} 
