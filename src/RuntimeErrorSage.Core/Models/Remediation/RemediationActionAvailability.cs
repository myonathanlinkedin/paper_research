using RuntimeErrorSage.Core.Models.Remediation.Interfaces;
using RuntimeErrorSage.Core.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Manages the availability of remediation actions.
    /// </summary>
    public class RemediationActionAvailability
    {
        private readonly Dictionary<string, AvailabilityStatus> _actionStatus = new();
        private readonly Dictionary<string, List<AvailabilityWindow>> _availabilityWindows = new();
        private readonly Dictionary<string, List<string>> _dependencies = new();

        /// <summary>
        /// Gets the current availability status of an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The availability status.</returns>
        public AvailabilityStatus GetStatus(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _actionStatus.TryGetValue(actionId, out var status) ? status : AvailabilityStatus.Unknown;
        }

        /// <summary>
        /// Sets the availability status of an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="status">The availability status.</param>
        public void SetStatus(string actionId, AvailabilityStatus status)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            _actionStatus[actionId] = status;
        }

        /// <summary>
        /// Adds an availability window for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="window">The availability window.</param>
        public void AddAvailabilityWindow(string actionId, AvailabilityWindow window)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(window);

            if (!_availabilityWindows.ContainsKey(actionId))
            {
                _availabilityWindows[actionId] = new List<AvailabilityWindow>();
            }
            _availabilityWindows[actionId].Add(window);
        }

        /// <summary>
        /// Gets the availability windows for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The list of availability windows.</returns>
        public IReadOnlyList<AvailabilityWindow> GetAvailabilityWindows(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _availabilityWindows.TryGetValue(actionId, out var windows) ? windows : new List<AvailabilityWindow>();
        }

        /// <summary>
        /// Adds a dependency for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="dependencyId">The dependency ID.</param>
        public void AddDependency(string actionId, string dependencyId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(dependencyId);

            if (!_dependencies.ContainsKey(actionId))
            {
                _dependencies[actionId] = new List<string>();
            }
            if (!_dependencies[actionId].Contains(dependencyId))
            {
                _dependencies[actionId].Add(dependencyId);
            }
        }

        /// <summary>
        /// Gets the dependencies for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The list of dependency IDs.</returns>
        public IReadOnlyList<string> GetDependencies(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _dependencies.TryGetValue(actionId, out var deps) ? deps : new List<string>();
        }

        /// <summary>
        /// Checks if an action is available at the current time.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>True if the action is available, false otherwise.</returns>
        public bool IsAvailable(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);

            // Check if the action is enabled
            if (GetStatus(actionId) != AvailabilityStatus.Enabled)
            {
                return false;
            }

            // Check if any dependencies are unavailable
            foreach (var depId in GetDependencies(actionId))
            {
                if (!IsAvailable(depId))
                {
                    return false;
                }
            }

            // Check if we're in an availability window
            var now = DateTime.UtcNow;
            var windows = GetAvailabilityWindows(actionId);
            foreach (var window in windows)
            {
                if (window.IsInWindow(now))
                {
                    return true;
                }
            }

            // If no windows are defined, the action is available
            return windows.Count == 0;
        }

        /// <summary>
        /// Clears all availability data for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public void ClearActionData(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            _actionStatus.Remove(actionId);
            _availabilityWindows.Remove(actionId);
            _dependencies.Remove(actionId);
        }

        /// <summary>
        /// Clears all availability data.
        /// </summary>
        public void ClearAllData()
        {
            _actionStatus.Clear();
            _availabilityWindows.Clear();
            _dependencies.Clear();
        }
    }

    /// <summary>
    /// Defines the availability status of a remediation action.
    /// </summary>
    public enum AvailabilityStatus
    {
        /// <summary>
        /// The action is enabled and available.
        /// </summary>
        Enabled,

        /// <summary>
        /// The action is disabled and unavailable.
        /// </summary>
        Disabled,

        /// <summary>
        /// The action is in maintenance mode.
        /// </summary>
        Maintenance,

        /// <summary>
        /// The action's availability status is unknown.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Represents a time window during which a remediation action is available.
    /// </summary>
    public class AvailabilityWindow
    {
        /// <summary>
        /// Gets or sets the start time of the window.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the window.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the days of the week when the window is active.
        /// </summary>
        public DayOfWeek[] DaysOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the time zone for the window.
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Checks if a given time falls within this window.
        /// </summary>
        /// <param name="time">The time to check.</param>
        /// <returns>True if the time is within the window, false otherwise.</returns>
        public bool IsInWindow(DateTime time)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(time, TimeZone);
            return localTime >= StartTime && localTime <= EndTime && DaysOfWeek.Contains(localTime.DayOfWeek);
        }
    }
} 


