using System;

namespace RuntimeErrorSage.Domain.Enums;

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
            ImpactSeverity.Critical => SeverityLevel.Critical,
            ImpactSeverity.Error => SeverityLevel.High,
            ImpactSeverity.Warning => SeverityLevel.Medium,
            ImpactSeverity.Info => SeverityLevel.Low,
            ImpactSeverity.Success => SeverityLevel.Info,
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
            ValidationSeverity.Info => SeverityLevel.Low,
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
            SeverityLevel.Critical => ImpactSeverity.Critical,
            SeverityLevel.High => ImpactSeverity.Error,
            SeverityLevel.Medium => ImpactSeverity.Warning,
            SeverityLevel.Low => ImpactSeverity.Info,
            SeverityLevel.Info => ImpactSeverity.Success,
            _ => ImpactSeverity.None
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
    /// Converts an ImpactLevel to RemediationActionSeverity.
    /// </summary>
    /// <param name="impactLevel">The impact level to convert.</param>
    /// <returns>The corresponding RemediationActionSeverity.</returns>
    public static RemediationActionSeverity ToRemediationActionSeverity(this ImpactLevel impactLevel)
    {
        return impactLevel switch
        {
            ImpactLevel.Critical => RemediationActionSeverity.Critical,
            ImpactLevel.High => RemediationActionSeverity.High,
            ImpactLevel.Medium => RemediationActionSeverity.Medium,
            ImpactLevel.Low => RemediationActionSeverity.Low,
            _ => RemediationActionSeverity.Unknown
        };
    }

    /// <summary>
    /// Converts an ImpactLevel to a SeverityLevel.
    /// </summary>
    /// <param name="impactLevel">The impact level to convert.</param>
    /// <returns>The corresponding SeverityLevel.</returns>
    public static SeverityLevel ToSeverityLevel(this ImpactLevel impactLevel)
    {
        return impactLevel switch
        {
            ImpactLevel.Critical => SeverityLevel.Critical,
            ImpactLevel.High => SeverityLevel.High,
            ImpactLevel.Medium => SeverityLevel.Medium,
            ImpactLevel.Low => SeverityLevel.Low,
            _ => SeverityLevel.Unknown
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
            SeverityLevel.Low => ValidationSeverity.Info,
            _ => ValidationSeverity.Info
        };
    }

    /// <summary>
    /// Converts a RemediationRiskLevel to RiskLevel.
    /// </summary>
    /// <param name="riskLevel">The remediation risk level to convert.</param>
    /// <returns>The corresponding RiskLevel.</returns>
    public static RiskLevel ToRiskLevel(this RemediationRiskLevel riskLevel)
    {
        return riskLevel switch
        {
            RemediationRiskLevel.Critical => RiskLevel.Critical,
            RemediationRiskLevel.High => RiskLevel.High,
            RemediationRiskLevel.Medium => RiskLevel.Medium,
            RemediationRiskLevel.Low => RiskLevel.Low,
            RemediationRiskLevel.None => RiskLevel.None,
            RemediationRiskLevel.Unknown => RiskLevel.Unknown,
            _ => RiskLevel.Unknown
        };
    }
    
    /// <summary>
    /// Converts a RiskLevel to RemediationRiskLevel.
    /// </summary>
    /// <param name="riskLevel">The risk level to convert.</param>
    /// <returns>The corresponding RemediationRiskLevel.</returns>
    public static RemediationRiskLevel ToRemediationRiskLevel(this RiskLevel riskLevel)
    {
        return riskLevel switch
        {
            RiskLevel.Critical => RemediationRiskLevel.Critical,
            RiskLevel.High => RemediationRiskLevel.High,
            RiskLevel.Medium => RemediationRiskLevel.Medium,
            RiskLevel.Low => RemediationRiskLevel.Low,
            RiskLevel.None => RemediationRiskLevel.None,
            RiskLevel.Unknown => RemediationRiskLevel.Unknown,
            _ => RemediationRiskLevel.Unknown
        };
    }

    /// <summary>
    /// Converts an ImpactSeverity to RemediationActionSeverity.
    /// </summary>
    /// <param name="severity">The impact severity to convert.</param>
    /// <returns>The corresponding RemediationActionSeverity.</returns>
    public static RemediationActionSeverity ToRemediationActionSeverity(this ImpactSeverity severity)
    {
        return severity switch
        {
            ImpactSeverity.Critical => RemediationActionSeverity.Critical,
            ImpactSeverity.Error => RemediationActionSeverity.High,
            ImpactSeverity.Warning => RemediationActionSeverity.Medium,
            ImpactSeverity.Info => RemediationActionSeverity.Low,
            ImpactSeverity.Success => RemediationActionSeverity.None,
            _ => RemediationActionSeverity.Unknown
        };
    }

    /// <summary>
    /// Converts an ImpactScope to RemediationActionImpactScope.
    /// </summary>
    /// <param name="scope">The impact scope to convert.</param>
    /// <returns>The corresponding RemediationActionImpactScope.</returns>
    public static RemediationActionImpactScope ToRemediationActionImpactScope(this ImpactScope scope)
    {
        return scope switch
        {
            ImpactScope.None => RemediationActionImpactScope.None,
            ImpactScope.Local => RemediationActionImpactScope.Local,
            ImpactScope.Service => RemediationActionImpactScope.Service,
            ImpactScope.System => RemediationActionImpactScope.System,
            ImpactScope.Component => RemediationActionImpactScope.Module,
            ImpactScope.MultiComponent => RemediationActionImpactScope.Module,
            ImpactScope.External => RemediationActionImpactScope.Global,
            _ => RemediationActionImpactScope.None
        };
    }

    /// <summary>
    /// Converts a RemediationActionImpactScope to ImpactScope.
    /// </summary>
    /// <param name="scope">The remediation action impact scope to convert.</param>
    /// <returns>The corresponding ImpactScope.</returns>
    public static ImpactScope ToImpactScope(this RemediationActionImpactScope scope)
    {
        return scope switch
        {
            RemediationActionImpactScope.None => ImpactScope.None,
            RemediationActionImpactScope.Local => ImpactScope.Local,
            RemediationActionImpactScope.Module => ImpactScope.Component,
            RemediationActionImpactScope.Service => ImpactScope.Service,
            RemediationActionImpactScope.System => ImpactScope.System,
            RemediationActionImpactScope.Global => ImpactScope.External,
            _ => ImpactScope.None
        };
    }
} 
