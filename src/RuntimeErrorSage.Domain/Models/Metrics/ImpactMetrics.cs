using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Models.Error;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Validation;

namespace RuntimeErrorSage.Domain.Models.Metrics
{
    /// <summary>
    /// Represents metrics for remediation impact.
    /// </summary>
    public class ImpactMetrics
    {
        public ImpactMetrics()
        {
            SeverityDistribution = new Dictionary<ErrorSeverity, int>();
            ScopeDistribution = new Dictionary<ImpactScope, int>();
            ImpactSeverityDistribution = new Dictionary<ImpactSeverity, int>();
            BusinessImpactDistribution = new Dictionary<string, int>();
            DurationDistribution = new Dictionary<TimeSpan, int>();
            RecoveryDurationDistribution = new Dictionary<TimeSpan, int>();
            ErrorTypeDistribution = new Dictionary<string, int>();
            StrategyDistribution = new Dictionary<string, int>();
            ValidationStatusDistribution = new Dictionary<AnalysisValidationStatus, int>();
            AdditionalMetrics = new Dictionary<string, object>();
            TotalRemediations = 0;
            SuccessfulRemediations = 0;
            FailedRemediations = 0;
            AverageTimeToRemediationMs = 0.0;
            AverageTimeToRecoveryMs = 0.0;
            AverageAffectedUsers = 0.0;
            AverageEstimatedRecoveryTimeMinutes = 0.0;
        }

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
        public Dictionary<ErrorSeverity, int> SeverityDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of impact scopes.
        /// </summary>
        public Dictionary<ImpactScope, int> ScopeDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of impact severities.
        /// </summary>
        public Dictionary<ImpactSeverity, int> ImpactSeverityDistribution { get; set; }

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
        public Dictionary<string, int> BusinessImpactDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of remediation durations.
        /// </summary>
        public Dictionary<TimeSpan, int> DurationDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of recovery durations.
        /// </summary>
        public Dictionary<TimeSpan, int> RecoveryDurationDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of error types.
        /// </summary>
        public Dictionary<string, int> ErrorTypeDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of remediation strategies.
        /// </summary>
        public Dictionary<string, int> StrategyDistribution { get; set; }

        /// <summary>
        /// Gets or sets the distribution of validation statuses.
        /// </summary>
        public Dictionary<AnalysisValidationStatus, int> ValidationStatusDistribution { get; set; }

        /// <summary>
        /// Gets or sets any additional metrics.
        /// </summary>
        public Dictionary<string, object> AdditionalMetrics { get; set; }

        private ErrorSeverity MapImpactSeverityToErrorSeverity(ImpactSeverity impactSeverity)
        {
            // Map ImpactSeverity to ErrorSeverity directly since they now have the same values
            return impactSeverity switch
            {
                ImpactSeverity.Fatal => ErrorSeverity.Fatal,
                ImpactSeverity.Critical => ErrorSeverity.Critical,
                ImpactSeverity.Error => ErrorSeverity.Error,
                ImpactSeverity.Warning => ErrorSeverity.Warning,
                ImpactSeverity.Info => ErrorSeverity.Info,
                ImpactSeverity.Success => ErrorSeverity.Success,
                _ => ErrorSeverity.None
            };
        }

        /// <summary>
        /// Updates the metrics with a new remediation result.
        /// </summary>
        /// <param name="analysisResult">The error analysis result.</param>
        /// <param name="remediationDuration">The duration of the remediation.</param>
        /// <param name="recoveryDuration">The duration of the recovery.</param>
        /// <param name="strategyName">The name of the strategy used.</param>
        /// <param name="isSuccessful">Whether the remediation was successful.</param>
        public void UpdateMetrics(
            ImpactErrorAnalysisResult analysisResult,
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
            var errorSeverity = MapImpactSeverityToErrorSeverity(analysisResult.Severity);
            if (!SeverityDistribution.ContainsKey(errorSeverity))
                SeverityDistribution[errorSeverity] = 0;
            SeverityDistribution[errorSeverity]++;

            // Update impact distributions using analysisResult's direct properties
            if (!ScopeDistribution.ContainsKey(analysisResult.Scope))
                ScopeDistribution[analysisResult.Scope] = 0;
            ScopeDistribution[analysisResult.Scope]++;

            if (!ImpactSeverityDistribution.ContainsKey(analysisResult.Severity))
                ImpactSeverityDistribution[analysisResult.Severity] = 0;
            ImpactSeverityDistribution[analysisResult.Severity]++;

            // Update average affected users
            AverageAffectedUsers = ((AverageAffectedUsers * (TotalRemediations - 1)) + analysisResult.AffectedUsers) / TotalRemediations;

            // Update average estimated recovery time
            AverageEstimatedRecoveryTimeMinutes = ((AverageEstimatedRecoveryTimeMinutes * (TotalRemediations - 1)) + analysisResult.EstimatedRecoveryTime.TotalMinutes) / TotalRemediations;

            // Update business impact distribution
            if (!string.IsNullOrEmpty(analysisResult.BusinessImpact))
            {
                if (!BusinessImpactDistribution.ContainsKey(analysisResult.BusinessImpact))
                    BusinessImpactDistribution[analysisResult.BusinessImpact] = 0;
                BusinessImpactDistribution[analysisResult.BusinessImpact]++;
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

        public void UpdateValidationStatus(AnalysisValidationStatus status)
        {
            // ... existing code ...
        }
    }

    public class ImpactErrorAnalysisResult
    {
        public ImpactLevel Impact { get; set; }
        public AnalysisValidationStatus ValidationStatus { get; set; }
        public string RootCause { get; set; }
        public ImpactSeverity Severity { get; set; }
        public ImpactScope Scope { get; set; }
        public int AffectedUsers { get; set; }
        public TimeSpan EstimatedRecoveryTime { get; set; }
        public string BusinessImpact { get; set; }
    }
} 
