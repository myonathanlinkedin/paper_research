using System;

namespace RuntimeErrorSage.Model.LLM.Options
{
    public class LMStudioOptions
    {
        /// <summary>
        /// Gets or sets the base URL for LM Studio.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model identifier.
        /// Must be set to "Qwen-2.5-7B-Instruct-1M" for research compliance.
        /// </summary>
        public string ModelId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the model version.
        /// </summary>
        public string ModelVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timeout for LM Studio operations in seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of tokens for generation.
        /// </summary>
        public int MaxTokens { get; set; }

        /// <summary>
        /// Gets or sets the temperature for generation.
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// Gets or sets the top-p sampling parameter.
        /// </summary>
        public float TopP { get; set; }

        /// <summary>
        /// Gets or sets the frequency penalty.
        /// </summary>
        public float FrequencyPenalty { get; set; }

        /// <summary>
        /// Gets or sets the presence penalty.
        /// </summary>
        public float PresencePenalty { get; set; }

        /// <summary>
        /// Gets or sets the stop sequences for generation.
        /// </summary>
        public string[] StopSequences { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets whether to use system prompts.
        /// </summary>
        public bool UseSystemPrompt { get; set; }

        /// <summary>
        /// Gets or sets the system prompt template.
        /// </summary>
        public string SystemPromptTemplate { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the context window size.
        /// </summary>
        public int ContextWindowSize { get; set; }

        /// <summary>
        /// Gets or sets whether to enable streaming responses.
        /// </summary>
        public bool EnableStreaming { get; set; }

        /// <summary>
        /// Gets or sets the streaming chunk size.
        /// </summary>
        public int StreamingChunkSize { get; set; }
    }
} 

