namespace RuntimeErrorSage.Model.Models.Enums
{
    /// <summary>
    /// Defines the priority levels for remediation actions.
    /// </summary>
    public enum RemediationPriority
    {
        /// <summary>
        /// Critical priority - requires immediate attention.
        /// </summary>
        Critical = 0,

        /// <summary>
        /// High priority - requires prompt attention.
        /// </summary>
        High = 1,

        /// <summary>
        /// Medium priority - requires attention but not urgent.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// Low priority - can be addressed when convenient.
        /// </summary>
        Low = 3
    }
} 
