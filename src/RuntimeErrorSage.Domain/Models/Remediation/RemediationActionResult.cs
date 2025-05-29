using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Application.Models.Common;
using RuntimeErrorSage.Application.Models.Validation;
using RuntimeErrorSage.Application.Models.Remediation;

namespace RuntimeErrorSage.Application.Models.Remediation
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
} 





