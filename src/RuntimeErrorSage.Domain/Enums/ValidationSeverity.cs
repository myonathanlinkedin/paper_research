using System.Collections.ObjectModel;
namespace RuntimeErrorSage.Domain.Enums
{
    /// <summary>
    /// Represents the severity of a validation rule.
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// The rule is informational only.
        /// </summary>
        Info,

        /// <summary>
        /// The rule generates a warning.
        /// </summary>
        Warning,

        /// <summary>
        /// The rule generates an error.
        /// </summary>
        Error,

        /// <summary>
        /// The rule generates a critical error.
        /// </summary>
        Critical
    }
} 







