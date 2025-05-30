using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Domain.Models.LLM
{
    public class LMStudioResponse
    {
        public List<Choice> Choices { get; set; }
        public string Id { get; set; }
        public string Model { get; set; }
        public string Object { get; set; }
    }
} 
