using RuntimeErrorSage.Domain.Models.Error;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Common;
using RuntimeErrorSage.Domain.Models.Validation;
using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Domain.Interfaces;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Handles results of remediation actions.
    /// </summary>
    public class RemediationActionResult
    {
        private readonly Dictionary<string, ActionResult> _results = new();
        private readonly List<Action<ActionResult>> _resultHandlers = new();

        /// <summary>
        /// Gets or sets the action ID.
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether the action was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if the action failed.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the action result.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets the collection of results.
        /// </summary>
        public IReadOnlyDictionary<string, ActionResult> Results => new ReadOnlyDictionary<string, ActionResult>(_results);

        /// <summary>
        /// Gets the collection of result handlers.
        /// </summary>
        public IReadOnlyList<Action<ActionResult>> ResultHandlers => new ReadOnlyCollection<Action<ActionResult>>(_resultHandlers);

        public RemediationActionResult()
        {
            ActionId = string.Empty;
            Name = string.Empty;
            ErrorMessage = string.Empty;
            _results = new Dictionary<string, ActionResult>();
            _resultHandlers = new List<Action<ActionResult>>();
            Success = false;
        }

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
                catch (Exception ex)
                {
                    // Log the exception but continue processing other handlers
                    System.Diagnostics.Debug.WriteLine($"Error in result handler: {ex.Message}");
                }
            }
        }

        public void RegisterHandler(Action<ActionResult> handler)
        {
            if (handler != null)
            {
                _resultHandlers.Add(handler);
            }
        }

        public void UnregisterHandler(Action<ActionResult> handler)
        {
            if (handler != null)
            {
                _resultHandlers.Remove(handler);
            }
        }

        public void Clear()
        {
            _resultHandlers.Clear();
        }
    }
} 





