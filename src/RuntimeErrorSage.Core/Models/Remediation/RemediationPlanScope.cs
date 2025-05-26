namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Defines scopes for remediation plans.
    /// </summary>
    public enum RemediationPlanScope
    {
        /// <summary>
        /// Single component scope.
        /// </summary>
        Component = 0,

        /// <summary>
        /// Module or subsystem scope.
        /// </summary>
        Module = 1,

        /// <summary>
        /// Service scope.
        /// </summary>
        Service = 2,

        /// <summary>
        /// Multiple services scope.
        /// </summary>
        MultiService = 3,

        /// <summary>
        /// System-wide scope.
        /// </summary>
        System = 4,

        /// <summary>
        /// External dependencies scope.
        /// </summary>
        External = 5,

        /// <summary>
        /// Data or storage scope.
        /// </summary>
        Data = 6,

        /// <summary>
        /// Network or connectivity scope.
        /// </summary>
        Network = 7,

        /// <summary>
        /// Security or access control scope.
        /// </summary>
        Security = 8
    }
} 