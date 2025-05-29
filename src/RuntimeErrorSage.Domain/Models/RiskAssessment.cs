using System.Collections.ObjectModel;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Application.Models
{
    public class RiskAssessment
    {
        public string Id { get; }
        public string ActionId { get; }
        public RemediationRiskLevel RiskLevel { get; }
        public IReadOnlyCollection<PotentialIssues> PotentialIssues { get; }
        public IReadOnlyCollection<MitigationSteps> MitigationSteps { get; }
        public Dictionary<string, object> Context { get; set; }
        public DateTime AssessedAt { get; }
        public string AssessedBy { get; }
        public string Notes { get; }

        public RiskAssessment()
        {
            Id = Guid.NewGuid().ToString();
            PotentialIssues = new Collection<string>();
            MitigationSteps = new Collection<string>();
            Context = new Dictionary<string, object>();
            AssessedAt = DateTime.UtcNow;
            RiskLevel = RemediationRiskLevel.Medium;
        }
    }
} 






