namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the stages of validation processing.
    /// </summary>
    public enum ValidationStage
    {
        /// <summary>
        /// No validation stage specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Pre-validation stage.
        /// </summary>
        PreValidation = 1,

        /// <summary>
        /// Input validation stage.
        /// </summary>
        Input = 2,

        /// <summary>
        /// Processing validation stage.
        /// </summary>
        Processing = 3,

        /// <summary>
        /// Output validation stage.
        /// </summary>
        Output = 4,

        /// <summary>
        /// Post-validation stage.
        /// </summary>
        PostValidation = 5,

        /// <summary>
        /// Error handling stage.
        /// </summary>
        ErrorHandling = 6,

        /// <summary>
        /// Recovery stage.
        /// </summary>
        Recovery = 7,

        /// <summary>
        /// Cleanup stage.
        /// </summary>
        Cleanup = 8,

        /// <summary>
        /// Final stage.
        /// </summary>
        Final = 9
    }
} 
