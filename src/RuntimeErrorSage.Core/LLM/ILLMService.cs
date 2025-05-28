using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.LLM;

/// <summary>
/// Defines the interface for LLM services.
/// </summary>
public interface ILLMService
{
    /// <summary>
    /// Generates a response from the LLM.
    /// </summary>
    /// <param name="prompt">The prompt.</param>
    /// <returns>The response.</returns>
    Task<string> GenerateAsync(string prompt);
} 
