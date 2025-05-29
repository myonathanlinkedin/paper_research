using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.Remediation;
using RuntimeErrorSage.Model.Models.Validation;

namespace RuntimeErrorSage.Model.Remediation.Interfaces
{
    public interface IRemediationSuggestionManager
    {
        Task<RemediationSuggestion> GetSuggestionsAsync(ErrorContext errorContext);
        Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
        Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
        Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
    }
} 
