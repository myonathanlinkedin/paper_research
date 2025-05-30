
using RuntimeErrorSage.Domain.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Manages various attributes of remediation actions.
    /// </summary>
    public class RemediationActionAttributes
    {
        private readonly Dictionary<string, Dictionary<string, object>> _attributes = new();

        /// <summary>
        /// Sets an attribute value for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="attributeType">The type of attribute.</param>
        /// <param name="value">The attribute value.</param>
        public void SetAttribute(string actionId, AttributeType attributeType, object value)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(value);

            if (!_attributes.ContainsKey(actionId))
            {
                _attributes[actionId] = new Dictionary<string, object>();
            }
            _attributes[actionId][attributeType.ToString()] = value;
        }

        /// <summary>
        /// Gets an attribute value for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="attributeType">The type of attribute.</param>
        /// <returns>The attribute value, or null if not found.</returns>
        public object GetAttribute(string actionId, AttributeType attributeType)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            if (_attributes.TryGetValue(actionId, out var attrs) &&
                attrs.TryGetValue(attributeType.ToString(), out var value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// Gets all attributes for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>A dictionary of attribute types and values.</returns>
        public IReadOnlyDictionary<string, object> GetAttributes(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _attributes.TryGetValue(actionId, out var attrs) ? attrs : new Dictionary<string, object>();
        }

        /// <summary>
        /// Removes an attribute from an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="attributeType">The type of attribute.</param>
        public void RemoveAttribute(string actionId, AttributeType attributeType)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            if (_attributes.TryGetValue(actionId, out var attrs))
            {
                attrs.Remove(attributeType.ToString());
            }
        }

        /// <summary>
        /// Clears all attributes for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public void ClearAttributes(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            _attributes.Remove(actionId);
        }

        /// <summary>
        /// Clears all attributes for all actions.
        /// </summary>
        public void ClearAllAttributes()
        {
            _attributes.Clear();
        }
    }
} 


