using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.LLM;

namespace RuntimeErrorSage.Core.Interfaces
{
    public interface IQwenLLMClient
    {
        Task<LLMResponse> GenerateResponseAsync(LLMRequest request);
        Task<bool> ValidateResponseAsync(LLMResponse response);
        Task<LLMAnalysis> AnalyzeErrorAsync(string errorMessage, string context);
        Task<LLMSuggestion> GetRemediationSuggestionAsync(LLMAnalysis analysis);
    }
} 