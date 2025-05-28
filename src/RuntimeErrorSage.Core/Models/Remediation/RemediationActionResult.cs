using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Models.Common;
using RuntimeErrorSage.Core.Models.Validation;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Handles results of remediation actions.
    /// </summary>
    public class RemediationActionResult
    {
        private readonly Dictionary<string, ActionResult> _results = new();
        private readonly List<Action<ActionResult>> _resultHandlers = new();

        /// <summary>
        /// Records a result for a remediation action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="result">The result to record.</param>
        public void RecordResult(IRemediationAction action, ActionResult result)
        {
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(result);

            _results[action.ActionId] = result;
            NotifyResultHandlers(result);
        }

        /// <summary>
        /// Gets the result for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The result, or null if not found.</returns>
        public ActionResult GetResult(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _results.TryGetValue(actionId, out var result) ? result : null;
        }

        /// <summary>
        /// Registers a handler for action results.
        /// </summary>
        /// <param name="handler">The handler to register.</param>
        public void RegisterResultHandler(Action<ActionResult> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _resultHandlers.Add(handler);
        }

        /// <summary>
        /// Unregisters a handler for action results.
        /// </summary>
        /// <param name="handler">The handler to unregister.</param>
        public void UnregisterResultHandler(Action<ActionResult> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _resultHandlers.Remove(handler);
        }

        /// <summary>
        /// Clears all results.
        /// </summary>
        public void ClearResults()
        {
            _results.Clear();
        }

        private void NotifyResultHandlers(ActionResult result)
        {
            foreach (var handler in _resultHandlers)
            {
                try
                {
                    handler(result);
                }
                catch
                {
                    // Ignore handler exceptions
                }
            }
        }
    }

    /// <summary>
    /// Represents the result of a remediation action.
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Gets or sets the ID of the action.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the result.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets whether the action was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error that occurred, if any.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// Gets or sets the execution time in milliseconds.
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// Gets or sets the validation result.
        /// </summary>
        public ValidationResult ValidationResult { get; set; }

        /// <summary>
        /// Gets or sets the execution result.
        /// </summary>
        public ExecutionResult ExecutionResult { get; set; }

        /// <summary>
        /// Gets or sets additional data associated with the result.
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
} 


