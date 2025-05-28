using System;

namespace RuntimeErrorSage.Core.Models.Enums
{
    public enum ImpactSeverity
    {
        Critical,
        High,
        Medium,
        Low,
        Minimal
    }

    public enum ImpactScope
    {
        Global,
        System,
        Component,
        Service,
        Local
    }

    public enum GraphNodeType
    {
        Service,
        Component,
        Dependency,
        Error,
        Remediation
    }

    public enum AggregationType
    {
        Sum,
        Average,
        Max,
        Min,
        Count
    }

    public enum ActionStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        RolledBack
    }

    public enum RemediationStatusEnum
    {
        NotStarted,
        InProgress,
        Completed,
        Failed,
        RolledBack
    }

    public enum RollbackStatus
    {
        NotRequired,
        Pending,
        InProgress,
        Completed,
        Failed
    }

    public enum AnalysisValidationStatus
    {
        Valid,
        Invalid,
        Warning,
        Unknown
    }

    /// <summary>
    /// Defines the type of error pattern
    /// </summary>
    public enum ErrorPatternType
    {
        /// <summary>
        /// Unknown pattern type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Sequential pattern where errors occur in a specific order
        /// </summary>
        Sequential = 1,

        /// <summary>
        /// Concurrent pattern where errors occur simultaneously
        /// </summary>
        Concurrent = 2,

        /// <summary>
        /// Recurring pattern where errors repeat at intervals
        /// </summary>
        Recurring = 3,

        /// <summary>
        /// Cascading pattern where one error triggers others
        /// </summary>
        Cascading = 4,

        /// <summary>
        /// Intermittent pattern where errors occur sporadically
        /// </summary>
        Intermittent = 5
    }

    /// <summary>
    /// Defines the type of error propagation
    /// </summary>
    public enum ErrorPropagation
    {
        /// <summary>
        /// Unknown propagation type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Direct propagation where error affects immediate components
        /// </summary>
        Direct = 1,

        /// <summary>
        /// Indirect propagation where error affects dependent components
        /// </summary>
        Indirect = 2,

        /// <summary>
        /// Cascading propagation where error triggers chain of failures
        /// </summary>
        Cascading = 3,

        /// <summary>
        /// Isolated propagation where error is contained
        /// </summary>
        Isolated = 4
    }

    /// <summary>
    /// Defines the type of error correlation
    /// </summary>
    public enum ErrorCorrelationType
    {
        /// <summary>
        /// Unknown correlation type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Temporal correlation based on timing
        /// </summary>
        Temporal = 1,

        /// <summary>
        /// Causal correlation based on cause-effect
        /// </summary>
        Causal = 2,

        /// <summary>
        /// Spatial correlation based on location
        /// </summary>
        Spatial = 3,

        /// <summary>
        /// Functional correlation based on functionality
        /// </summary>
        Functional = 4
    }
} 