using System.Collections.ObjectModel;
using System;

namespace RuntimeErrorSage.Application.Models.LLM
{
    public class Choice
    {
        public string Text { get; }
        public int Index { get; }
        public string FinishReason { get; }
    }
} 






