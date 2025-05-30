
using RuntimeErrorSage.Domain.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Domain.Models.Remediation
{
    /// <summary>
    /// Tracks and manages resource usage for remediation actions.
    /// </summary>
    public class ResourceUsage
    {
        private readonly Dictionary<string, Dictionary<string, ResourceMetrics>> _actionResources = new();
        private readonly Dictionary<string, ResourceLimits> _resourceLimits = new();

        /// <summary>
        /// Records resource usage for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="resourceType">The type of resource.</param>
        /// <param name="usage">The resource usage value.</param>
        /// <param name="unit">The unit of measurement.</param>
        public void RecordUsage(string actionId, string resourceType, double usage, string unit)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(resourceType);
            ArgumentNullException.ThrowIfNull(unit);

            if (!_actionResources.ContainsKey(actionId))
            {
                _actionResources[actionId] = new Dictionary<string, ResourceMetrics>();
            }

            if (!_actionResources[actionId].ContainsKey(resourceType))
            {
                _actionResources[actionId][resourceType] = new ResourceMetrics
                {
                    ResourceType = resourceType,
                    Unit = unit,
                    CurrentUsage = 0,
                    PeakUsage = 0,
                    UsageHistory = new List<UsageRecord>()
                };
            }

            var metrics = _actionResources[actionId][resourceType];
            metrics.CurrentUsage = usage;
            metrics.PeakUsage = Math.Max(metrics.PeakUsage, usage);
            metrics.UsageHistory.Add(new UsageRecord
            {
                Timestamp = DateTime.UtcNow,
                Usage = usage
            });
        }

        /// <summary>
        /// Gets the current resource usage for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="resourceType">The type of resource.</param>
        /// <returns>The current resource usage.</returns>
        public ResourceMetrics GetUsage(string actionId, string resourceType)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(resourceType);

            if (_actionResources.TryGetValue(actionId, out var resources) &&
                resources.TryGetValue(resourceType, out var metrics))
            {
                return metrics;
            }

            return null;
        }

        /// <summary>
        /// Sets resource limits for a resource type.
        /// </summary>
        /// <param name="resourceType">The type of resource.</param>
        /// <param name="limits">The resource limits.</param>
        public void SetResourceLimits(string resourceType, ResourceLimits limits)
        {
            ArgumentNullException.ThrowIfNull(resourceType);
            ArgumentNullException.ThrowIfNull(limits);
            _resourceLimits[resourceType] = limits;
        }

        /// <summary>
        /// Gets resource limits for a resource type.
        /// </summary>
        /// <param name="resourceType">The type of resource.</param>
        /// <returns>The resource limits.</returns>
        public ResourceLimits GetResourceLimits(string resourceType)
        {
            ArgumentNullException.ThrowIfNull(resourceType);
            return _resourceLimits.TryGetValue(resourceType, out var limits) ? limits : null;
        }

        /// <summary>
        /// Checks if an action's resource usage is within limits.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>True if usage is within limits, false otherwise.</returns>
        public bool IsWithinLimits(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            if (!_actionResources.TryGetValue(actionId, out var resources))
            {
                return true;
            }

            foreach (var resource in resources)
            {
                var limits = GetResourceLimits(resource.Key);
                if (limits != null && resource.Value.CurrentUsage > limits.Maximum)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Clears resource usage data for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public void ClearActionData(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            _actionResources.Remove(actionId);
        }

        /// <summary>
        /// Clears all resource usage data.
        /// </summary>
        public void ClearAllData()
        {
            _actionResources.Clear();
            _resourceLimits.Clear();
        }
    }
} 
