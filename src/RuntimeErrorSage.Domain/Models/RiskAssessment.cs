using RuntimeErrorSage.Domain.Enums;
using System;

namespace RuntimeErrorSage.Domain.Models
{
    public class RiskAssessment
    {
        public string Id { get; set; }
        public string ActionId { get; set; }
        public RemediationRiskLevel RiskLevel { get; set; }
        public List<string> PotentialIssues { get; set; }
        public List<string> MitigationSteps { get; set; }
        public Dictionary<string, object> Context { get; set; }
        public DateTime AssessedAt { get; set; }
        public string AssessedBy { get; set; }
        public string Notes { get; set; }
        
        // Additional properties for compatibility
        public AnalysisStatus Status { get; set; }
        public List<string> RiskFactors { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public RemediationActionImpactScope ImpactScope { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public string CorrelationId { get; set; }
        public double Confidence { get; set; }
        public List<string> AffectedComponents { get; set; }

        public RiskAssessment()
        {
            Id = Guid.NewGuid().ToString();
            PotentialIssues = new List<string>();
            MitigationSteps = new List<string>();
            Context = new Dictionary<string, object>();
            AssessedAt = DateTime.UtcNow;
            RiskLevel = RemediationRiskLevel.Medium;
            Status = AnalysisStatus.Pending;
            RiskFactors = new List<string>();
            Metadata = new Dictionary<string, object>();
            ImpactScope = RemediationActionImpactScope.None;
            EstimatedDuration = TimeSpan.Zero;
            CorrelationId = string.Empty;
            Confidence = 0.0;
            AffectedComponents = new List<string>();
        }
    }
} 
