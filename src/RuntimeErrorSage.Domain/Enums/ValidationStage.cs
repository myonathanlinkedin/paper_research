namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Defines the stages of validation operations.
    /// </summary>
    public enum ValidationStage
    {
        /// <summary>
        /// Unknown validation stage.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Initial validation stage.
        /// </summary>
        Initial = 1,

        /// <summary>
        /// Pre-processing validation stage.
        /// </summary>
        PreProcessing = 2,

        /// <summary>
        /// Processing validation stage.
        /// </summary>
        Processing = 3,

        /// <summary>
        /// Post-processing validation stage.
        /// </summary>
        PostProcessing = 4,

        /// <summary>
        /// Final validation stage.
        /// </summary>
        Final = 5,

        /// <summary>
        /// Pre-execution stage.
        /// </summary>
        PreExecution = 6,

        /// <summary>
        /// Execution stage.
        /// </summary>
        Execution = 7,

        /// <summary>
        /// Post-execution stage.
        /// </summary>
        PostExecution = 8,

        /// <summary>
        /// Initialization stage.
        /// </summary>
        Initialization = 9,

        /// <summary>
        /// Finalization stage.
        /// </summary>
        Finalization = 10
    }
} 
