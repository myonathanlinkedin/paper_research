using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.LLM
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
} 
