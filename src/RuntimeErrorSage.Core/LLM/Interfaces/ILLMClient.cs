using System.Threading.Tasks;
using RuntimeErrorSage.Model.Models.Error;
using RuntimeErrorSage.Model.Models.LLM;
using RuntimeErrorSage.Model.Models.Remediation;

namespace RuntimeErrorSage.Model.LLM.Interfaces
{
    /// <summary>
    /// Interface for Qwen LLM client operations.
    /// </summary>
    public interface ILLMClient
    {
        /// <summary>
        /// Gets whether the client is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the client name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the client version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets whether the client is connected.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        ///     Analyzes the provided error context and returns an analysis result.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<LLMAnalysis> AnalyzeContextAsync(ErrorContext context);

        /// <summary>
        ///     Generates a response based on the provided LLM request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<LLMResponse> GenerateResponseAsync(LLMRequest request);

        /// <summary>
        ///     Validates the response from the LLM to ensure it meets expected criteria.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        Task<bool> ValidateResponseAsync(LLMResponse response);

        /// <summary>
        ///    Analyzes an error message and context to provide a detailed analysis.                  
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<LLMAnalysis> AnalyzeErrorAsync(string errorMessage, string context);

        /// <summary>
        ///   Provides a remediation suggestion based on the analysis of the error context. 
        /// </summary>
        /// <param name="analysis"></param>
        /// <returns></returns>
        Task<LLMSuggestion> GetRemediationSuggestionAsync(LLMAnalysis analysis);
    }
} 
