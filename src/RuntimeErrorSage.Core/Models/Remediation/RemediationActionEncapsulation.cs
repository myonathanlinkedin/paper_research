using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Provides encapsulation functionality for remediation actions.
    /// </summary>
    public class RemediationActionEncapsulation
    {
        private readonly Dictionary<string, object> _privateData = new();
        private readonly Dictionary<string, Action<object>> _stateChangeHandlers = new();

        /// <summary>
        /// Gets or sets the encapsulation level.
        /// </summary>
        public EncapsulationLevel Level { get; set; } = EncapsulationLevel.Private;

        /// <summary>
        /// Gets or sets whether the action is encapsulated.
        /// </summary>
        public bool IsEncapsulated { get; private set; }

        /// <summary>
        /// Gets the private data.
        /// </summary>
        public IReadOnlyDictionary<string, object> PrivateData => _privateData;

        /// <summary>
        /// Sets private data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetPrivateData(string key, object value)
        {
            ArgumentNullException.ThrowIfNull(key);
            _privateData[key] = value;
            NotifyStateChange(key, value);
        }

        /// <summary>
        /// Gets private data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public object GetPrivateData(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            return _privateData.TryGetValue(key, out var value) ? value : null;
        }

        /// <summary>
        /// Registers a state change handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void RegisterStateChangeHandler(string key, Action<object> handler)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(handler);
            _stateChangeHandlers[key] = handler;
        }

        /// <summary>
        /// Notifies state change.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void NotifyStateChange(string key, object value)
        {
            if (_stateChangeHandlers.TryGetValue(key, out var handler))
            {
                handler(value);
            }
        }

        /// <summary>
        /// Encapsulates the action.
        /// </summary>
        public void Encapsulate()
        {
            IsEncapsulated = true;
        }

        /// <summary>
        /// Decapsulates the action.
        /// </summary>
        public void Decapsulate()
        {
            IsEncapsulated = false;
        }
    }
}
 





