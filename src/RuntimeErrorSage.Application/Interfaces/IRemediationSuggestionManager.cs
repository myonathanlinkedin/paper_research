using System.Threading.Tasks;
using RuntimeErrorSage.Application.Models.Error;
using RuntimeErrorSage.Application.Models.Remediation;
using RuntimeErrorSage.Application.Models.Validation;

namespace RuntimeErrorSage.Application.Remediation.Interfaces
{
    public interface IRemediationSuggestionManager
    {
        Task<RemediationSuggestion> GetSuggestionsAsync(ErrorContext errorContext);
        Task<ValidationResult> ValidateSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
        Task<RemediationResult> ExecuteSuggestionAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
        Task<RemediationImpact> GetSuggestionImpactAsync(RemediationSuggestion suggestion, ErrorContext errorContext);
    }
} 
