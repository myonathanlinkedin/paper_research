using System.Threading.Tasks;

namespace CodeSage.Core
{
    /// <summary>
    /// Defines the interface for interacting with the LM Studio API.
    /// </summary>
    public interface ILMStudioClient
    {
        /// <summary>
        /// Analyzes an error using the local LLM through LM Studio API.
        /// </summary>
        /// <param name="prompt">The prompt to send to the LLM</param>
        /// <returns>The LLM's analysis of the error</returns>
        Task<string> AnalyzeErrorAsync(string prompt);

        /// <summary>
        /// Initializes the LM Studio client with the specified model.
        /// </summary>
        /// <param name="modelName">The name of the model to use</param>
        /// <param name="endpoint">The LM Studio API endpoint</param>
        /// <returns>A task representing the asynchronous initialization</returns>
        Task InitializeAsync(string modelName, string endpoint);

        /// <summary>
        /// Gets the current model information.
        /// </summary>
        /// <returns>Information about the currently loaded model</returns>
        Task<ModelInfo> GetModelInfoAsync();

        /// <summary>
        /// Updates the model to a new version if available.
        /// </summary>
        /// <returns>A task representing the asynchronous update</returns>
        Task<bool> UpdateModelAsync();
    }

    /// <summary>
    /// Contains information about the loaded LLM model.
    /// </summary>
    public class ModelInfo
    {
        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the version of the model.
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the model in gigabytes.
        /// </summary>
        public double SizeGB { get; set; }

        /// <summary>
        /// Gets or sets the context window size of the model.
        /// </summary>
        public int ContextWindowSize { get; set; }

        /// <summary>
        /// Gets or sets whether the model is currently loaded.
        /// </summary>
        public bool IsLoaded { get; set; }

        /// <summary>
        /// Gets or sets the last time the model was updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
} 