using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.LLM.Interfaces
{
    public interface ILMStudioClient
    {
        Task<string> AnalyzeErrorAsync(string prompt);
        Task<string> GenerateRemediationAsync(object analysis);
        Task<bool> IsModelReadyAsync();
    }
} 

