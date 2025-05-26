namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the impact of a remediation action.
    /// </summary>
    public class RemediationImpact
    {
        public string ImpactType { get; set; }
        public double SeverityScore { get; set; }
        public string Description { get; set; }
    }
} 