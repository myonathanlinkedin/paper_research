using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents a remediation suggestion.
    /// </summary>
    public class RemediationSuggestion
    {
        public string SuggestionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<RemediationAction> Actions { get; set; }
        public double ConfidenceScore { get; set; }
        public RemediationImpact Impact { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 