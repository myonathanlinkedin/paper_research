namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the impact level of a remediation action.
    /// </summary>
    public enum ImpactLevel
    {
        /// <summary>
        /// No impact.
        /// </summary>
        None,

        /// <summary>
        /// Low impact.
        /// </summary>
        Low,

        /// <summary>
        /// Medium impact.
        /// </summary>
        Medium,

        /// <summary>
        /// High impact.
        /// </summary>
        High,

        /// <summary>
        /// Critical impact.
        /// </summary>
        Critical
    }
} 