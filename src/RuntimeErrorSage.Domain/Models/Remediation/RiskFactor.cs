namespace RuntimeErrorSage.Model.Models.Remediation
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
        /// Gets or sets the risk factor score (0-1).
        /// </summary>
        public double Score { get; set; }
    }
} 
