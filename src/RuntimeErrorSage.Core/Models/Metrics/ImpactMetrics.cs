using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Models.Metrics
{
    /// <summary>
    /// Represents metrics for remediation impact.
    /// </summary>
    public class ImpactMetrics
    {
        /// <summary>
        /// Gets or sets the total number of remediations.
        /// </summary>
        public int TotalRemediations { get; set; }

        /// <summary>
        /// Gets or sets the number of successful remediations.
        /// </summary>
        public int SuccessfulRemediations { get; set; }

        /// <summary>
        /// Gets or sets the number of failed remediations.
        /// </summary>
        public int FailedRemediations { get; set; }

        /// <summary>
        /// Gets or sets the average time to remediation in milliseconds.
        /// </summary>
        public double AverageTimeToRemediationMs { get; set; }

        /// <summary>
        /// Gets or sets the average time to recovery in milliseconds.
        /// </summary>
        public double AverageTimeToRecoveryMs { get; set; }

        /// <summary>
        /// Gets or sets the success rate of remediations (0-1).
        /// </summary>
        public double SuccessRate => TotalRemediations > 0 ? (double)SuccessfulRemediations / TotalRemediations : 0;

        /// <summary>
        /// Gets or sets the failure rate of remediations (0-1).
        /// </summary>
        public double FailureRate => TotalRemediations > 0 ? (double)FailedRemediations / TotalRemediations : 0;

        /// <summary>
        /// Gets or sets the distribution of error severities.
        /// </summary>
        public Dictionary<ErrorSeverity, int> SeverityDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of impact scopes.
        /// </summary>
        public Dictionary<ImpactScope, int> ScopeDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of impact severities.
        /// </summary>
        public Dictionary<ImpactSeverity, int> ImpactSeverityDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the average number of affected users per remediation.
        /// </summary>
        public double AverageAffectedUsers { get; set; }

        /// <summary>
        /// Gets or sets the average estimated recovery time in minutes.
        /// </summary>
        public double AverageEstimatedRecoveryTimeMinutes { get; set; }

        /// <summary>
        /// Gets or sets the distribution of business impacts.
        /// </summary>
        public Dictionary<string, int> BusinessImpactDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of remediation durations.
        /// </summary>
        public Dictionary<TimeSpan, int> DurationDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of recovery durations.
        /// </summary>
        public Dictionary<TimeSpan, int> RecoveryDurationDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of error types.
        /// </summary>
        public Dictionary<string, int> ErrorTypeDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of remediation strategies.
        /// </summary>
        public Dictionary<string, int> StrategyDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets the distribution of validation statuses.
        /// </summary>
        public Dictionary<AnalysisValidationStatus, int> ValidationStatusDistribution { get; set; } = new();

        /// <summary>
        /// Gets or sets any additional metrics.
        /// </summary>
        public Dictionary<string, object> AdditionalMetrics { get; set; } = new();

        /// <summary>
        /// Updates the metrics with a new remediation result.
        /// </summary>
        /// <param name="analysisResult">The error analysis result.</param>
        /// <param name="remediationDuration">The duration of the remediation.</param>
        /// <param name="recoveryDuration">The duration of the recovery.</param>
        /// <param name="strategyName">The name of the strategy used.</param>
        /// <param name="isSuccessful">Whether the remediation was successful.</param>
        public void UpdateMetrics(
            ErrorAnalysisResult analysisResult,
            TimeSpan remediationDuration,
            TimeSpan recoveryDuration,
            string strategyName,
            bool isSuccessful)
        {
            TotalRemediations++;
            if (isSuccessful)
                SuccessfulRemediations++;
            else
                FailedRemediations++;

            // Update severity distribution
            if (!SeverityDistribution.ContainsKey(analysisResult.Severity))
                SeverityDistribution[analysisResult.Severity] = 0;
            SeverityDistribution[analysisResult.Severity]++;

            // Update impact distributions
            if (analysisResult.Impact != null)
            {
                if (!ScopeDistribution.ContainsKey(analysisResult.Impact.Scope))
                    ScopeDistribution[analysisResult.Impact.Scope] = 0;
                ScopeDistribution[analysisResult.Impact.Scope]++;

                if (!ImpactSeverityDistribution.ContainsKey(analysisResult.Impact.Severity))
                    ImpactSeverityDistribution[analysisResult.Impact.Severity] = 0;
                ImpactSeverityDistribution[analysisResult.Impact.Severity]++;

                // Update average affected users
                AverageAffectedUsers = ((AverageAffectedUsers * (TotalRemediations - 1)) + analysisResult.Impact.AffectedUsers) / TotalRemediations;

                // Update average estimated recovery time
                if (analysisResult.Impact.EstimatedRecoveryTime.HasValue)
                {
                    AverageEstimatedRecoveryTimeMinutes = ((AverageEstimatedRecoveryTimeMinutes * (TotalRemediations - 1)) + 
                        analysisResult.Impact.EstimatedRecoveryTime.Value.TotalMinutes) / TotalRemediations;
                }

                // Update business impact distribution
                if (!string.IsNullOrEmpty(analysisResult.Impact.BusinessImpact))
                {
                    if (!BusinessImpactDistribution.ContainsKey(analysisResult.Impact.BusinessImpact))
                        BusinessImpactDistribution[analysisResult.Impact.BusinessImpact] = 0;
                    BusinessImpactDistribution[analysisResult.Impact.BusinessImpact]++;
                }
            }

            // Update duration distributions
            var roundedRemediationDuration = TimeSpan.FromSeconds(Math.Round(remediationDuration.TotalSeconds / 5) * 5); // Round to nearest 5 seconds
            if (!DurationDistribution.ContainsKey(roundedRemediationDuration))
                DurationDistribution[roundedRemediationDuration] = 0;
            DurationDistribution[roundedRemediationDuration]++;

            var roundedRecoveryDuration = TimeSpan.FromSeconds(Math.Round(recoveryDuration.TotalSeconds / 5) * 5); // Round to nearest 5 seconds
            if (!RecoveryDurationDistribution.ContainsKey(roundedRecoveryDuration))
                RecoveryDurationDistribution[roundedRecoveryDuration] = 0;
            RecoveryDurationDistribution[roundedRecoveryDuration]++;

            // Update average times
            AverageTimeToRemediationMs = ((AverageTimeToRemediationMs * (TotalRemediations - 1)) + remediationDuration.TotalMilliseconds) / TotalRemediations;
            AverageTimeToRecoveryMs = ((AverageTimeToRecoveryMs * (TotalRemediations - 1)) + recoveryDuration.TotalMilliseconds) / TotalRemediations;

            // Update error type distribution
            if (!string.IsNullOrEmpty(analysisResult.RootCause))
            {
                var errorType = analysisResult.RootCause.Split(':')[0].Trim();
                if (!ErrorTypeDistribution.ContainsKey(errorType))
                    ErrorTypeDistribution[errorType] = 0;
                ErrorTypeDistribution[errorType]++;
            }

            // Update strategy distribution
            if (!string.IsNullOrEmpty(strategyName))
            {
                if (!StrategyDistribution.ContainsKey(strategyName))
                    StrategyDistribution[strategyName] = 0;
                StrategyDistribution[strategyName]++;
            }

            // Update validation status distribution
            if (!ValidationStatusDistribution.ContainsKey(analysisResult.ValidationStatus))
                ValidationStatusDistribution[analysisResult.ValidationStatus] = 0;
            ValidationStatusDistribution[analysisResult.ValidationStatus]++;
        }
    }
} 