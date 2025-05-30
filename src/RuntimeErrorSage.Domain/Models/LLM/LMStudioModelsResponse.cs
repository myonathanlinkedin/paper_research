using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.LLM
{
    public class LMStudioModelsResponse
    {
        public List<ModelInfo> Data { get; set; }
        public string Object { get; set; }
    }
} 
