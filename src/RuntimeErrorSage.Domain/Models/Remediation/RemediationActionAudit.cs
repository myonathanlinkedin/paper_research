using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Handles auditing of remediation actions.
    /// </summary>
    public class RemediationActionAudit
    {
        private readonly Collection<AuditEntry> _auditLog = new();
        private readonly Dictionary<string, Collection<AuditEntry>> _actionAuditLogs = new();

        /// <summary>
        /// Records an audit entry for a remediation action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="details">Additional details about the event.</param>
        /// <param name="userId">The ID of the user who performed the action.</param>
        public string actionId, AuditEventType eventType, string details, string userId { ArgumentNullException.ThrowIfNull(string actionId, AuditEventType eventType, string details, string userId); }
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
                _actionAuditLogs[actionId] = new Collection<AuditEntry>();
            }
            _actionAuditLogs[actionId].Add(entry);
        }

        /// <summary>
        /// Gets the audit log for a specific action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The list of audit entries for the action.</returns>
        public IReadOnlyCollection<AuditEntry> GetActionAuditLog(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _actionAuditLogs.TryGetValue(actionId, out var entries) ? entries : new Collection<AuditEntry>();
        }

        /// <summary>
        /// Gets the complete audit log.
        /// </summary>
        /// <returns>The list of all audit entries.</returns>
        public IReadOnlyCollection<AuditEntry> GetAuditLog()
        {
            return _auditLog.AsReadOnly();
        }

        /// <summary>
        /// Gets audit entries within a specific time range.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>The list of audit entries within the time range.</returns>
        public IReadOnlyCollection<AuditEntry> GetAuditEntriesInTimeRange(DateTime startTime, DateTime endTime)
        {
            return _auditLog.FindAll(e => e.Timestamp >= startTime && e.Timestamp <= endTime);
        }

        /// <summary>
        /// Gets audit entries for a specific event type.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <returns>The list of audit entries for the event type.</returns>
        public IReadOnlyCollection<AuditEntry> GetAuditEntriesByEventType(AuditEventType eventType)
        {
            return _auditLog.FindAll(e => e.EventType == eventType);
        }

        /// <summary>
        /// Gets audit entries for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The list of audit entries for the user.</returns>
        public IReadOnlyCollection<AuditEntry> GetAuditEntriesByUser(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId);
            return _auditLog.FindAll(e => e.UserId == userId);
        }

        /// <summary>
        /// Clears the audit log for a specific action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public string actionId { ArgumentNullException.ThrowIfNull(string actionId); }
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
} 








