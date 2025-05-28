using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.LLM.Interfaces
{
    public interface ILMStudioClient
    {
        Task<string> AnalyzeErrorAsync(string prompt);
        Task<string> GenerateRemediationAsync(object analysis);
    }
} 

