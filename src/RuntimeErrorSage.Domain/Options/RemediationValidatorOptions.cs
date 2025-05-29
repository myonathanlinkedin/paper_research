using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Model.Options
{
    /// <summary>
    /// Configuration options for the RemediationValidator.
    /// </summary>
    public class RemediationValidatorOptions
    {
        /// <summary>
        /// Gets or sets whether the validator is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to enable strict validation.
        /// </summary>
        public bool EnableStrictValidation { get; set; } = true;

        /// <summary>
        /// Gets or sets the validation timeout.
        /// </summary>
        public TimeSpan ValidationTimeout { get; set; } = TimeSpan.FromMinutes(2);

        /// <summary>
        /// Gets or sets the maximum number of validation retries.
        /// </summary>
        public int MaxValidationRetries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the default threshold for metric validation.
        /// </summary>
        public double DefaultMetricThreshold { get; set; } = 80.0; // 80% threshold by default

        /// <summary>
        /// Gets or sets the allowed step types and their parameters.
        /// </summary>
        public Dictionary<string, string[]> AllowedStepTypes { get; set; } = new()
        {
            ["restart"] = new[] { "service" },
            ["clear"] = new[] { "resource" },
            ["update"] = new[] { "component", "version" },
            ["script"] = new[] { "script", "timeout" }
        };

        /// <summary>
        /// Gets or sets the allowed strategy types and their parameters.
        /// </summary>
        public Dictionary<string, string[]> AllowedStrategyTypes { get; set; } = new()
        {
            ["monitor"] = new[] { "metric", "threshold" },
            ["alert"] = new[] { "channel", "severity" },
            ["backup"] = new[] { "target", "schedule" }
        };
    }
} 
