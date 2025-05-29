namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the stages of validation operations.
    /// </summary>
    public enum ValidationStage
    {
        /// <summary>
        /// Unknown stage.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Pre-processing stage.
        /// </summary>
        PreProcessing = 1,

        /// <summary>
        /// Processing stage.
        /// </summary>
        Processing = 2,

        /// <summary>
        /// Post-processing stage.
        /// </summary>
        PostProcessing = 3,

        /// <summary>
        /// Pre-execution stage.
        /// </summary>
        PreExecution = 4,

        /// <summary>
        /// Execution stage.
        /// </summary>
        Execution = 5,

        /// <summary>
        /// Post-execution stage.
        /// </summary>
        PostExecution = 6,

        /// <summary>
        /// Initialization stage.
        /// </summary>
        Initialization = 7,

        /// <summary>
        /// Finalization stage.
        /// </summary>
        Finalization = 8
    }
} 
