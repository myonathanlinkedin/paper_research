using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Handles auditing of remediation actions.
    /// </summary>
    public class RemediationActionAudit
    {
        private readonly List<AuditEntry> _auditLog = new();
        private readonly Dictionary<string, List<AuditEntry>> _actionAuditLogs = new();

        /// <summary>
        /// Records an audit entry for a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="details">Additional details about the event.</param>
        /// <param name="userId">The ID of the user who performed the action.</param>
        public void RecordAuditEntry(string actionId, AuditEventType eventType, string details, string userId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(details);
            ArgumentNullException.ThrowIfNull(userId);

            var entry = new AuditEntry
            {
                ActionId = actionId,
                EventType = eventType,
                Details = details,
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };

            _auditLog.Add(entry);

            if (!_actionAuditLogs.ContainsKey(actionId))
            {
                _actionAuditLogs[actionId] = new List<AuditEntry>();
            }
            _actionAuditLogs[actionId].Add(entry);
        }

        /// <summary>
        /// Gets the audit log for a specific action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The list of audit entries for the action.</returns>
        public IReadOnlyList<AuditEntry> GetActionAuditLog(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _actionAuditLogs.TryGetValue(actionId, out var entries) ? entries : new List<AuditEntry>();
        }

        /// <summary>
        /// Gets the complete audit log.
        /// </summary>
        /// <returns>The list of all audit entries.</returns>
        public IReadOnlyList<AuditEntry> GetAuditLog()
        {
            return _auditLog.AsReadOnly();
        }

        /// <summary>
        /// Gets audit entries within a specific time range.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>The list of audit entries within the time range.</returns>
        public IReadOnlyList<AuditEntry> GetAuditEntriesInTimeRange(DateTime startTime, DateTime endTime)
        {
            return _auditLog.FindAll(e => e.Timestamp >= startTime && e.Timestamp <= endTime);
        }

        /// <summary>
        /// Gets audit entries for a specific event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <returns>The list of audit entries for the event type.</returns>
        public IReadOnlyList<AuditEntry> GetAuditEntriesByEventType(AuditEventType eventType)
        {
            return _auditLog.FindAll(e => e.EventType == eventType);
        }

        /// <summary>
        /// Gets audit entries for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The list of audit entries for the user.</returns>
        public IReadOnlyList<AuditEntry> GetAuditEntriesByUser(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            return _auditLog.FindAll(e => e.UserId == userId);
        }

        /// <summary>
        /// Clears the audit log for a specific action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public void ClearActionAuditLog(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            if (_actionAuditLogs.ContainsKey(actionId))
            {
                _auditLog.RemoveAll(e => e.ActionId == actionId);
                _actionAuditLogs.Remove(actionId);
            }
        }

        /// <summary>
        /// Clears the complete audit log.
        /// </summary>
        public void ClearAuditLog()
        {
            _auditLog.Clear();
            _actionAuditLogs.Clear();
        }
    }

    /// <summary>
    /// Represents an audit entry for a remediation action.
    /// </summary>
    public class AuditEntry
    {
        /// <summary>
        /// Gets or sets the ID of the action.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the type of event.
        /// </summary>
        public AuditEventType EventType { get; set; }

        /// <summary>
        /// Gets or sets additional details about the event.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who performed the action.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Defines the types of events that can be audited.
    /// </summary>
    public enum AuditEventType
    {
        /// <summary>
        /// The action was created.
        /// </summary>
        Created,

        /// <summary>
        /// The action was modified.
        /// </summary>
        Modified,

        /// <summary>
        /// The action was executed.
        /// </summary>
        Executed,

        /// <summary>
        /// The action was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The action was completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The action failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The action was deleted.
        /// </summary>
        Deleted,

        /// <summary>
        /// The action's status was changed.
        /// </summary>
        StatusChanged,

        /// <summary>
        /// The action's configuration was changed.
        /// </summary>
        ConfigurationChanged,

        /// <summary>
        /// The action's permissions were changed.
        /// </summary>
        PermissionsChanged
    }
} 


