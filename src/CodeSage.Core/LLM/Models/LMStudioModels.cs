using System;
using System.Collections.Generic;

namespace CodeSage.Core.LLM.Models
{
    public class LMStudioResponse
    {
        public List<Choice> Choices { get; set; }
        public string Id { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
    }

    public class Choice
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public string FinishReason { get; set; }
    }

    public class LMStudioModelsResponse
    {
        public List<ModelInfo> Data { get; set; }
        public string Object { get; set; }
    }

    public class ModelInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public double SizeGB { get; set; }
        public int ContextWindowSize { get; set; }
        public bool IsLoaded { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Status { get; set; }
        public string Object { get; set; }

        public bool IsReady => Status == "ready" && IsLoaded;
    }
} 