using RuntimeErrorSage.Application.Models.Remediation.Interfaces;
using RuntimeErrorSage.Application.Models.Remediation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Provides cohesion functionality for remediation actions.
    /// </summary>
    public class RemediationActionCohesion
    {
        private readonly List<string> _relatedActions = new();
        private readonly Dictionary<string, double> _cohesionScores = new();

        /// <summary>
        /// Gets or sets the cohesion level.
        /// </summary>
        public CohesionLevel Level { get; set; } = CohesionLevel.High;

        /// <summary>
        /// Gets the related actions.
        /// </summary>
        public IReadOnlyList<string> RelatedActions => _relatedActions;

        /// <summary>
        /// Gets the cohesion scores.
        /// </summary>
        public IReadOnlyDictionary<string, double> CohesionScores => _cohesionScores;

        /// <summary>
        /// Adds a related action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <param name="cohesionScore">The cohesion score.</param>
        public void AddRelatedAction(string actionId, double cohesionScore)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            
            if (cohesionScore < 0 || cohesionScore > 1)
                throw new ArgumentOutOfRangeException(nameof(cohesionScore), "Cohesion score must be between 0 and 1");

            if (!_relatedActions.Contains(actionId))
            {
                _relatedActions.Add(actionId);
                _cohesionScores[actionId] = cohesionScore;
            }
        }

        /// <summary>
        /// Removes a related action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        public void RemoveRelatedAction(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            
            _relatedActions.Remove(actionId);
            _cohesionScores.Remove(actionId);
        }

        /// <summary>
        /// Gets the average cohesion score.
        /// </summary>
        /// <returns>The average cohesion score.</returns>
        public double GetAverageCohesionScore()
        {
            if (!_cohesionScores.Any())
                return 0;

            return _cohesionScores.Values.Average();
        }

        /// <summary>
        /// Gets the cohesion score for an action.
        /// </summary>
        /// <param name="actionId">The action ID.</param>
        /// <returns>The cohesion score.</returns>
        public double GetCohesionScore(string actionId)
        {
            ArgumentNullException.ThrowIfNull(actionId);
            
            return _cohesionScores.TryGetValue(actionId, out var score) ? score : 0;
        }

        /// <summary>
        /// Updates the cohesion level based on scores.
        /// </summary>
        public void UpdateCohesionLevel()
        {
            var averageScore = GetAverageCohesionScore();
            
            if (averageScore >= 0.8)
                Level = CohesionLevel.High;
            else if (averageScore >= 0.5)
                Level = CohesionLevel.Medium;
            else
                Level = CohesionLevel.Low;
        }

        /// <summary>
        /// Clears all related actions and scores.
        /// </summary>
        public void Clear()
        {
            _relatedActions.Clear();
            _cohesionScores.Clear();
        }
    }
}
 





