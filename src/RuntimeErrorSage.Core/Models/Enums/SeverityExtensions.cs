using System;

namespace RuntimeErrorSage.Core.Models.Enums
{
    /// <summary>
    /// Extension methods for converting between different severity types.
    /// </summary>
    public static class SeverityExtensions
    {
        /// <summary>
        /// Converts an ErrorSeverity to SeverityLevel.
        /// </summary>
        public static SeverityLevel ToSeverityLevel(this ErrorSeverity severity)
        {
            return severity switch
            {
                ErrorSeverity.Critical => SeverityLevel.Critical,
                ErrorSeverity.High => SeverityLevel.High,
                ErrorSeverity.Medium => SeverityLevel.Medium,
                ErrorSeverity.Low => SeverityLevel.Low,
                ErrorSeverity.Info => SeverityLevel.Info,
                _ => SeverityLevel.Unknown
            };
        }

        /// <summary>
        /// Converts an ImpactSeverity to SeverityLevel.
        /// </summary>
        public static SeverityLevel ToSeverityLevel(this ImpactSeverity severity)
        {
            return severity switch
            {
                ImpactSeverity.Catastrophic => SeverityLevel.Catastrophic,
                ImpactSeverity.Critical => SeverityLevel.Critical,
                ImpactSeverity.Major => SeverityLevel.High,
                ImpactSeverity.Moderate => SeverityLevel.Medium,
                ImpactSeverity.Minor => SeverityLevel.Low,
                ImpactSeverity.Negligible => SeverityLevel.Info,
                _ => SeverityLevel.Unknown
            };
        }

        /// <summary>
        /// Converts a RemediationActionSeverity to SeverityLevel.
        /// </summary>
        public static SeverityLevel ToSeverityLevel(this RemediationActionSeverity severity)
        {
            return severity switch
            {
                RemediationActionSeverity.Critical => SeverityLevel.Critical,
                RemediationActionSeverity.High => SeverityLevel.High,
                RemediationActionSeverity.Medium => SeverityLevel.Medium,
                RemediationActionSeverity.Low => SeverityLevel.Low,
                RemediationActionSeverity.None => SeverityLevel.Info,
                _ => SeverityLevel.Unknown
            };
        }

        /// <summary>
        /// Converts a ValidationSeverity to SeverityLevel.
        /// </summary>
        public static SeverityLevel ToSeverityLevel(this ValidationSeverity severity)
        {
            return severity switch
            {
                ValidationSeverity.Critical => SeverityLevel.Critical,
                ValidationSeverity.Error => SeverityLevel.High,
                ValidationSeverity.Warning => SeverityLevel.Medium,
                ValidationSeverity.Info => SeverityLevel.Info,
                _ => SeverityLevel.Unknown
            };
        }

        /// <summary>
        /// Converts a SeverityLevel to ErrorSeverity.
        /// </summary>
        public static ErrorSeverity ToErrorSeverity(this SeverityLevel severity)
        {
            return severity switch
            {
                SeverityLevel.Critical => ErrorSeverity.Critical,
                SeverityLevel.High => ErrorSeverity.High,
                SeverityLevel.Medium => ErrorSeverity.Medium,
                SeverityLevel.Low => ErrorSeverity.Low,
                SeverityLevel.Info => ErrorSeverity.Info,
                _ => ErrorSeverity.Info
            };
        }

        /// <summary>
        /// Converts a SeverityLevel to ImpactSeverity.
        /// </summary>
        public static ImpactSeverity ToImpactSeverity(this SeverityLevel severity)
        {
            return severity switch
            {
                SeverityLevel.Catastrophic => ImpactSeverity.Catastrophic,
                SeverityLevel.Critical => ImpactSeverity.Critical,
                SeverityLevel.High => ImpactSeverity.Major,
                SeverityLevel.Medium => ImpactSeverity.Moderate,
                SeverityLevel.Low => ImpactSeverity.Minor,
                SeverityLevel.Info => ImpactSeverity.Negligible,
                _ => ImpactSeverity.Unknown
            };
        }

        /// <summary>
        /// Converts a SeverityLevel to RemediationActionSeverity.
        /// </summary>
        public static RemediationActionSeverity ToRemediationActionSeverity(this SeverityLevel severity)
        {
            return severity switch
            {
                SeverityLevel.Critical => RemediationActionSeverity.Critical,
                SeverityLevel.High => RemediationActionSeverity.High,
                SeverityLevel.Medium => RemediationActionSeverity.Medium,
                SeverityLevel.Low => RemediationActionSeverity.Low,
                _ => RemediationActionSeverity.None
            };
        }

        /// <summary>
        /// Converts a SeverityLevel to ValidationSeverity.
        /// </summary>
        public static ValidationSeverity ToValidationSeverity(this SeverityLevel severity)
        {
            return severity switch
            {
                SeverityLevel.Critical => ValidationSeverity.Critical,
                SeverityLevel.High => ValidationSeverity.Error,
                SeverityLevel.Medium => ValidationSeverity.Warning,
                _ => ValidationSeverity.Info
            };
        }
    }
} 
