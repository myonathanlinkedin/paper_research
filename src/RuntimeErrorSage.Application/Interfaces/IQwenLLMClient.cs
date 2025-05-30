using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Models.Context;
using RuntimeErrorSage.Domain.Models.LLM;

namespace RuntimeErrorSage.Application.LLM.Interfaces
{
    /// <summary>
    /// Interface for interacting with the Qwen 2.5 7B Instruct 1M LLM.
    /// </summary>
    public interface IQwenLLMClient
    {
        /// <summary>
        /// Generates a response from the LLM based on the given prompt.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM.</param>
        /// <param name="maxTokens">The maximum number of tokens to generate.</param>
        /// <param name="temperature">The temperature to use for generation.</param>
        /// <returns>The generated response.</returns>
        Task<string> GenerateResponseAsync(string prompt, int maxTokens = 1024, float temperature = 0.7f);

        /// <summary>
        /// Generates a response from the LLM based on the given prompt and context.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM.</param>
        /// <param name="context">The context to include with the prompt.</param>
        /// <param name="maxTokens">The maximum number of tokens to generate.</param>
        /// <param name="temperature">The temperature to use for generation.</param>
        /// <returns>The generated response.</returns>
        Task<string> GenerateResponseWithContextAsync(string prompt, string context, int maxTokens = 1024, float temperature = 0.7f);

        /// <summary>
        /// Generates a response from the LLM based on the given prompt and system message.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM.</param>
        /// <param name="systemMessage">The system message to include with the prompt.</param>
        /// <param name="maxTokens">The maximum number of tokens to generate.</param>
        /// <param name="temperature">The temperature to use for generation.</param>
        /// <returns>The generated response.</returns>
        Task<string> GenerateResponseWithSystemMessageAsync(string prompt, string systemMessage, int maxTokens = 1024, float temperature = 0.7f);

        /// <summary>
        /// Generates a response from the LLM based on the given prompt, context, and system message.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM.</param>
        /// <param name="context">The context to include with the prompt.</param>
        /// <param name="systemMessage">The system message to include with the prompt.</param>
        /// <param name="maxTokens">The maximum number of tokens to generate.</param>
        /// <param name="temperature">The temperature to use for generation.</param>
        /// <returns>The generated response.</returns>
        Task<string> GenerateResponseWithContextAndSystemMessageAsync(string prompt, string context, string systemMessage, int maxTokens = 1024, float temperature = 0.7f);

        /// <summary>
        /// Generates a response from the LLM based on the given prompt and conversation history.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM.</param>
        /// <param name="conversationHistory">The conversation history to include with the prompt.</param>
        /// <param name="maxTokens">The maximum number of tokens to generate.</param>
        /// <param name="temperature">The temperature to use for generation.</param>
        /// <returns>The generated response.</returns>
        Task<string> GenerateResponseWithConversationHistoryAsync(string prompt, string[] conversationHistory, int maxTokens = 1024, float temperature = 0.7f);

        /// <summary>
        /// Generates a response from the LLM based on the given prompt, context, system message, and conversation history.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM.</param>
        /// <param name="context">The context to include with the prompt.</param>
        /// <param name="systemMessage">The system message to include with the prompt.</param>
        /// <param name="conversationHistory">The conversation history to include with the prompt.</param>
        /// <param name="maxTokens">The maximum number of tokens to generate.</param>
        /// <param name="temperature">The temperature to use for generation.</param>
        /// <returns>The generated response.</returns>
        Task<string> GenerateResponseWithContextSystemMessageAndConversationHistoryAsync(string prompt, string context, string systemMessage, string[] conversationHistory, int maxTokens = 1024, float temperature = 0.7f);

        /// <summary>
        /// Analyzes the given context using the LLM
        /// </summary>
        /// <param name="context">The context to analyze</param>
        /// <returns>The analysis result</returns>
        Task<LLMAnalysisResult> AnalyzeContextAsync(RuntimeContext context);
    }
} 
