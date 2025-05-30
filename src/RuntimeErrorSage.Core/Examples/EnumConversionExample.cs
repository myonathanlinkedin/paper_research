using System;
using System.Collections.Generic;
using RuntimeErrorSage.Domain.Enums;

namespace RuntimeErrorSage.Core.Examples;

/// <summary>
/// Demonstrates proper Enum conversion patterns
/// </summary>
public class EnumConversionExample
{
    /// <summary>
    /// Shows how to handle ErrorCategory enum to string conversion
    /// </summary>
    public void ShowEnumConversion()
    {
        // Example of enum to string conversion
        Domain.Enums.ErrorCategory category = Domain.Enums.ErrorCategory.RuntimeException;
        string categoryStr = category.ToString();

        // Example of string to enum conversion
        string categoryString = "RuntimeException";
        if (Enum.TryParse<Domain.Enums.ErrorCategory>(categoryString, out var parsedCategory))
        {
            // Use the parsed category
        }

        // Example of enum to string mapping
        var categoryMappings = new Dictionary<Domain.Enums.ErrorCategory, string>
        {
            { Domain.Enums.ErrorCategory.RuntimeException, "Runtime Exception" },
            { Domain.Enums.ErrorCategory.CompilationError, "Compilation Error" },
            // Add more mappings as needed
        };
    }
} 
