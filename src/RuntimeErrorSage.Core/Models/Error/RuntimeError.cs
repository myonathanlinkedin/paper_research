using System;

namespace RuntimeErrorSage.Core.Models.Error
{
    /// <summary>
    /// Represents a runtime error in the system.
    /// </summary>
    public class RuntimeError
    {
        /// <summary>
        /// Gets or sets the unique identifier of the error.
        /// </summary>
        public string ErrorId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp when the error occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the type of error.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component where the error occurred.
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service where the error occurred.
        /// </summary>
        public string Service { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique Id of the error (alias for ErrorId).
        /// </summary>
        public string Id
        {
            get => ErrorId;
            set => ErrorId = value;
        }

        /// <summary>
        /// Gets or sets the component ID where the error occurred.
        /// </summary>
        public string ComponentId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the error (alias for ErrorType).
        /// </summary>
        public string Type
        {
            get => ErrorType;
            set => ErrorType = value;
        }

        /// <summary>
        /// Gets or sets the location where the error occurred.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the conditions associated with the error.
        /// </summary>
        public string[] Conditions { get; set; } = Array.Empty<string>();
    }
} 