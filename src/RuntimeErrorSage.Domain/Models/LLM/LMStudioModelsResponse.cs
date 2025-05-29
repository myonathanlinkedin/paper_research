using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.LLM
{
    public class LMStudioModelsResponse
    {
        public IReadOnlyCollection<Data> Data { get; }
        public string Object { get; }
    }
} 





