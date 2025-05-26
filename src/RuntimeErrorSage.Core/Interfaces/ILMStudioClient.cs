using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Interfaces
{
    public interface ILMStudioClient
    {
        Task<string> AnalyzeErrorAsync(string prompt);
        Task<string> GenerateRemediationAsync(object analysis);
    }
} 
