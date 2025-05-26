using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common
{
    public class RemediationImpact
    {
        public string RemediationId { get; set; } = string.Empty;
        public ImpactScope Scope { get; set; }
        public ImpactSeverity Severity { get; set; }
        public List<string> AffectedComponents { get; set; } = new();
        public Dictionary<string, object> ImpactDetails { get; set; } = new();
        public DateTime AssessmentTime { get; set; }
        public string? AssessmentNotes { get; set; }
    }

    /// <summary>
    /// Defines the scope of impact.
    /// </summary>
    public enum ImpactScope
    {
        /// <summary>
        /// Local impact scope.
        /// </summary>
        Local,

        /// <summary>
        /// Component-level impact scope.
        /// </summary>
        Component,

        /// <summary>
        /// Service-level impact scope.
        /// </summary>
        Service,

        /// <summary>
        /// System-level impact scope.
        /// </summary>
        System,

        /// <summary>
        /// Global impact scope.
        /// </summary>
        Global,

        /// <summary>
        /// Operation-level impact scope.
        /// </summary>
        Operation,

        /// <summary>
        /// Instance-level impact scope.
        /// </summary>
        Instance,

        /// <summary>
        /// Application-level impact scope.
        /// </summary>
        Application,

        /// <summary>
        /// Module-level impact scope.
        /// </summary>
        Module,

        /// <summary>
        /// Feature-level impact scope.
        /// </summary>
        Feature,

        /// <summary>
        /// Unknown impact scope.
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Defines the severity of impact.
    /// </summary>
    public enum ImpactSeverity
    {
        /// <summary>
        /// No impact severity.
        /// </summary>
        None,

        /// <summary>
        /// Low impact severity.
        /// </summary>
        Low,

        /// <summary>
        /// Medium impact severity.
        /// </summary>
        Medium,

        /// <summary>
        /// High impact severity.
        /// </summary>
        High,

        /// <summary>
        /// Critical impact severity.
        /// </summary>
        Critical
    }
} 
