using System;
using System.Collections.Generic;

namespace CodeSage.Core.Models
{
    /// <summary>
    /// Represents a remediation action suggested by the error analysis.
    /// </summary>
    public class RemediationAction
    {
        /// <summary>
        /// Gets or sets the unique identifier of the action.
        /// </summary>
        public string ActionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        public string ActionType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parameters required for the action.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new();

        /// <summary>
        /// Gets or sets the priority of the action (1-5, where 5 is highest).
        /// </summary>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Gets or sets whether the action requires manual approval.
        /// </summary>
        public bool RequiresApproval { get; set; }

        /// <summary>
        /// Gets or sets the estimated time to execute the action in minutes.
        /// </summary>
        public int EstimatedDurationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the potential impact of the action.
        /// </summary>
        public string Impact { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any prerequisites for the action.
        /// </summary>
        public List<string> Prerequisites { get; set; } = new();
    }
} 