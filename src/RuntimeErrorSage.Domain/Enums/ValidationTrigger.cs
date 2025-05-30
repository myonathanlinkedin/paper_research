namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the triggers of validation operations.
    /// </summary>
    public enum ValidationTrigger
    {
        /// <summary>
        /// Unknown validation trigger.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Automatic validation trigger.
        /// </summary>
        Automatic = 1,

        /// <summary>
        /// Manual validation trigger.
        /// </summary>
        Manual = 2,

        /// <summary>
        /// Scheduled validation trigger.
        /// </summary>
        Scheduled = 3,

        /// <summary>
        /// OnDemand validation trigger.
        /// </summary>
        OnDemand = 4,

        /// <summary>
        /// Event validation trigger.
        /// </summary>
        Event = 5,

        /// <summary>
        /// Time validation trigger.
        /// </summary>
        Time = 6
    }
} 
