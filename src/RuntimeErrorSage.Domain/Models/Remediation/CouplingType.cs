namespace RuntimeErrorSage.Application.Models.Remediation
{
    /// <summary>
    /// Defines the type of coupling between actions.
    /// </summary>
    public enum CouplingType
    {
        Loose,
        Tight,
        Data,
        Control,
        Common,
        Content
    }
} 