using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models
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

        public RiskAssessment()
        {
            Id = Guid.NewGuid().ToString();
            PotentialIssues = new List<string>();
            MitigationSteps = new List<string>();
            Context = new Dictionary<string, object>();
            AssessedAt = DateTime.UtcNow;
            RiskLevel = RemediationRiskLevel.Medium;
        }
    }
} 