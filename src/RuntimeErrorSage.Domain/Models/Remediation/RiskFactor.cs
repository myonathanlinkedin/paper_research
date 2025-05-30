namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Represents a risk factor in the assessment.
    /// </summary>
    public class RiskFactor
    {
        /// <summary>
        /// Gets or sets the risk factor name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the risk factor description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the risk factor weight (0-1).
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the risk factor value (0-1).
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the risk factor score (0-1).
        /// </summary>
        public double Score { get; set; }
    }
} 
