using System;

namespace CodeSage.Core.LLM.Options
{
    public class LMStudioOptions
    {
        public string BaseUrl { get; set; } = "http://localhost:1234";
        public string ModelId { get; set; } = "local-model";
        public int TimeoutSeconds { get; set; } = 30;
        public int MaxTokens { get; set; } = 500;
        public float Temperature { get; set; } = 0.7f;
        public float TopP { get; set; } = 0.9f;
        public float FrequencyPenalty { get; set; } = 0.0f;
        public float PresencePenalty { get; set; } = 0.0f;
        public string[] StopSequences { get; set; } = Array.Empty<string>();
    }
} 