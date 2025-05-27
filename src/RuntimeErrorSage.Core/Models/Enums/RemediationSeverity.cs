// This file is not needed if the correct enum is referenced from RuntimeErrorSage.Models.Enums.
// Please use 'using RuntimeErrorSage.Models.Enums;' and reference RemediationSeverity directly.
// If you need to use an alias, use:
// using RemediationSeverity = RuntimeErrorSage.Models.Enums.RemediationSeverity;

namespace RuntimeErrorSage.Core.Models.Enums;

public enum RemediationSeverity
{
    Critical,
    High,
    Medium,
    Low,
    None,
} 