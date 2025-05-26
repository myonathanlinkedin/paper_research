using System.Threading.Tasks;

namespace CodeSage.Core.Interfaces
{
    public interface ILMStudioClient
    {
        Task<string> AnalyzeErrorAsync(string prompt);
        Task<string> GenerateRemediationAsync(object analysis);
    }
} 