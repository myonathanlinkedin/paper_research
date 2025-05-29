using System;

namespace RuntimeErrorSage.Model.Models.LLM
{
    public class Choice
    {
        public string Text { get; set; }
        public int Index { get; set; }
        public string FinishReason { get; set; }
    }
} 
