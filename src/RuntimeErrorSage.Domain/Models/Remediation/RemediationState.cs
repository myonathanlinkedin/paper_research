using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Provides constants for remediation states.
    /// </summary>
    public static class RemediationStateConstants
    {
        // Define static constants that map to the enum values
        public static readonly string NotStarted = "NotStarted";
        public static readonly string InProgress = "InProgress";
        public static readonly string Completed = "Completed";
        public static readonly string Failed = "Failed";
        public static readonly string Cancelled = "Cancelled";
        public static readonly string RolledBack = "RolledBack";
        public static readonly string Waiting = "Waiting";
        public static readonly string Paused = "Paused";
    }

    /// <summary>
    /// Represents the state tracking for a remediation process.
    /// </summary>
    public class RemediationStateTracking
    {
        /// <summary>
        /// Gets or sets the unique identifier for this state.
        /// </summary>
        public string StateId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the current step of the remediation.
        /// </summary>
        public string CurrentStep { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress of the remediation (0-100).
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the state information.
        /// </summary>
        public RemediationStateInfo StateInfo { get; set; } = new RemediationStateInfo();

        /// <summary>
        /// Gets or sets whether the remediation is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation is paused.
        /// </summary>
        public bool IsPaused { get; set; }
    }

    /// <summary>
    /// Represents the state information of a remediation process.
    /// </summary>
    public class RemediationStateInfo
    {
        /// <summary>
        /// Gets or sets the unique identifier for this state.
        /// </summary>
        public string StateId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the status of the remediation.
        /// </summary>
        public RemediationStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the current step of the remediation.
        /// </summary>
        public string CurrentStep { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the progress of the remediation (0-100).
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the start time of the remediation.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the remediation.
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the error message if the remediation failed.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the metadata of the remediation.
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new();

        /// <summary>
        /// Gets or sets the completed steps.
        /// </summary>
        public List<string> CompletedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the failed steps.
        /// </summary>
        public List<string> FailedSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets the pending steps.
        /// </summary>
        public List<string> PendingSteps { get; set; } = new();

        /// <summary>
        /// Gets or sets whether the remediation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets whether the remediation has been rolled back.
        /// </summary>
        public bool IsRolledBack { get; set; }

        /// <summary>
        /// Gets or sets the rollback error message if the rollback failed.
        /// </summary>
        public string RollbackErrorMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the state of a remediation process.
    /// </summary>
    public class RemediationState
    {
        /// <summary>
        /// Gets or sets the current state of the remediation.
        /// </summary>
        public RemediationStateEnum CurrentState { get; set; } = RemediationStateEnum.NotStarted;

        /// <summary>
        /// Gets or sets the timestamp when the state was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the reason for the current state.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the details about the current state.
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state history.
        /// </summary>
        public System.Collections.Generic.List<RemediationStateChange> History { get; set; } = new System.Collections.Generic.List<RemediationStateChange>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationState"/> class.
        /// </summary>
        public RemediationState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemediationState"/> class.
        /// </summary>
        /// <param name="state">The initial state.</param>
        /// <param name="reason">The reason for the state.</param>
        public RemediationState(RemediationStateEnum state, string reason = "")
        {
            CurrentState = state;
            Reason = reason;
            AddStateChange(state, reason);
        }

        /// <summary>
        /// Changes the current state.
        /// </summary>
        /// <param name="newState">The new state.</param>
        /// <param name="reason">The reason for the state change.</param>
        /// <returns>True if the state was changed; otherwise, false.</returns>
        public bool ChangeState(RemediationStateEnum newState, string reason = "")
        {
            if (CurrentState == newState)
            {
                return false;
            }

            var previousState = CurrentState;
            CurrentState = newState;
            LastUpdated = DateTime.UtcNow;
            Reason = reason;

            AddStateChange(newState, reason, previousState);
            return true;
        }

        /// <summary>
        /// Adds a state change to the history.
        /// </summary>
        /// <param name="state">The new state.</param>
        /// <param name="reason">The reason for the state change.</param>
        /// <param name="previousState">The previous state.</param>
        private void AddStateChange(RemediationStateEnum state, string reason, RemediationStateEnum? previousState = null)
        {
            History.Add(new RemediationStateChange
            {
                State = state,
                PreviousState = previousState,
                Timestamp = DateTime.UtcNow,
                Reason = reason
            });
        }
    }
} 
