using System;

namespace RuntimeErrorSage.Core.LLM.Options
{
    public class LMStudioOptions
    {
        /// <summary>
        /// Gets or sets the base URL for LM Studio.
        /// </summary>
        public string BaseUrl { get; set; } = "http://localhost:1234";

        /// <summary>
        /// Gets or sets the model identifier.
        /// Must be set to "Qwen-2.5-7B-Instruct-1M" for research compliance.
        /// </summary>
        public string ModelId { get; set; } = "Qwen-2.5-7B-Instruct-1M";

        /// <summary>
        /// Gets or sets the model version.
        /// </summary>
        public string ModelVersion { get; set; } = "1.0";

        /// <summary>
        /// Gets or sets the timeout for LM Studio operations in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets the maximum number of tokens for generation.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public int MaxTokens { get; set; } = 2048;

        /// <summary>
        /// Gets or sets the temperature for generation.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public float Temperature { get; set; } = 0.3f;

        /// <summary>
        /// Gets or sets the top-p sampling parameter.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public float TopP { get; set; } = 0.9f;

        /// <summary>
        /// Gets or sets the frequency penalty.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public float FrequencyPenalty { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the presence penalty.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public float PresencePenalty { get; set; } = 0.1f;

        /// <summary>
        /// Gets or sets the stop sequences for generation.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public string[] StopSequences { get; set; } = new[] { "Human:", "Assistant:", "\n\n" };

        /// <summary>
        /// Gets or sets whether to use system prompts.
        /// Required for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public bool UseSystemPrompt { get; set; } = true;

        /// <summary>
        /// Gets or sets the system prompt template.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public string SystemPromptTemplate { get; set; } = "You are an expert .NET runtime error analyzer. Analyze the following error and provide a detailed analysis with root causes, remediation steps, and prevention strategies. Be precise and technical in your analysis.";

        /// <summary>
        /// Gets or sets the context window size.
        /// Optimized for Qwen 2.5 7B Instruct 1M.
        /// </summary>
        public int ContextWindowSize { get; set; } = 8192;

        /// <summary>
        /// Gets or sets whether to enable streaming responses.
        /// </summary>
        public bool EnableStreaming { get; set; } = false;

        /// <summary>
        /// Gets or sets the streaming chunk size.
        /// </summary>
        public int StreamingChunkSize { get; set; } = 128;
    }
} 
