using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Application.Models.LLM
{
    public class LMStudioResponse
    {
        public IReadOnlyCollection<Choices> Choices { get; }
        public string Id { get; }
        public string Model { get; }
        public string Object { get; }
    }
} 





