using System.Collections.ObjectModel;
using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Manages coupling between remediation actions.
    /// </summary>
    public class RemediationActionCoupling
    {
        private readonly Dictionary<string, Collection<string>> _dependencies = new();
        private readonly Dictionary<string, Collection<string>> _dependents = new();
        private readonly Dictionary<string, CouplingType> _couplingTypes = new();

        /// <summary>
        /// Gets the dependencies for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The list of dependent action IDs.</returns>
        public IReadOnlyCollection<string> GetDependencies(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _dependencies.TryGetValue(actionId, out var deps) ? deps : new Collection<string>();
        }

        /// <summary>
        /// Gets the dependents for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The list of dependent action IDs.</returns>
        public IReadOnlyCollection<string> GetDependents(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            return _dependents.TryGetValue(actionId, out var deps) ? deps : new Collection<string>();
        }

        /// <summary>
        /// Adds a dependency between actions.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="dependencyId">The dependency ID.</param>
        /// <param name="couplingType">The type of coupling.</param>
        public string actionId, string dependencyId, CouplingType couplingType { ArgumentNullException.ThrowIfNull(string actionId, string dependencyId, CouplingType couplingType); }
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(dependencyId);

            if (!_dependencies.ContainsKey(actionId))
                _dependencies[actionId] = new Collection<string>();
            if (!_dependencies[actionId].Contains(dependencyId))
                _dependencies[actionId].Add(dependencyId);

            if (!_dependents.ContainsKey(dependencyId))
                _dependents[dependencyId] = new Collection<string>();
            if (!_dependents[dependencyId].Contains(actionId))
                _dependents[dependencyId].Add(actionId);

            _couplingTypes[$"{actionId}_{dependencyId}"] = couplingType;
        }

        /// <summary>
        /// Removes a dependency between actions.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="dependencyId">The dependency ID.</param>
        public string actionId, string dependencyId { ArgumentNullException.ThrowIfNull(string actionId, string dependencyId); }
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(dependencyId);

            if (_dependencies.ContainsKey(actionId))
                _dependencies[actionId].Remove(dependencyId);

            if (_dependents.ContainsKey(dependencyId))
                _dependents[dependencyId].Remove(actionId);

            _couplingTypes.Remove($"{actionId}_{dependencyId}");
        }

        /// <summary>
        /// Gets the coupling type between actions.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="dependencyId">The dependency ID.</param>
        /// <returns>The coupling type.</returns>
        public string actionId, string dependencyId { ArgumentNullException.ThrowIfNull(string actionId, string dependencyId); }
        {
            ArgumentNullException.ThrowIfNull(actionId);
            ArgumentNullException.ThrowIfNull(dependencyId);

            return _couplingTypes.TryGetValue($"{actionId}_{dependencyId}", out var type) ? type : CouplingType.Loose;
        }

        /// <summary>
        /// Checks if there are any circular dependencies.
        /// </summary>
        /// <returns>True if there are circular dependencies, false otherwise.</returns>
        public bool HasCircularDependencies()
        {
            var visited = new HashSet<string>();
            var recursionStack = new HashSet<string>();

            foreach (var actionId in _dependencies.Keys)
            {
                if (IsCyclicUtil(actionId, visited, recursionStack))
                    return true;
            }

            return false;
        }

        private bool IsCyclicUtil(string actionId, HashSet<string> visited, HashSet<string> recursionStack)
        {
            if (recursionStack.Contains(actionId))
                return true;

            if (visited.Contains(actionId))
                return false;

            visited.Add(actionId);
            recursionStack.Add(actionId);

            if (_dependencies.TryGetValue(actionId, out var deps))
            {
                foreach (var dep in deps)
                {
                    if (IsCyclicUtil(dep, visited, recursionStack))
                        return true;
                }
            }

            recursionStack.Remove(actionId);
            return false;
        }

        /// <summary>
        /// Clears all dependencies.
        /// </summary>
        public void Clear()
        {
            _dependencies.Clear();
            _dependents.Clear();
            _couplingTypes.Clear();
        }
    }
} 









