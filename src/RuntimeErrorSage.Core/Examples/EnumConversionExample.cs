using RuntimeErrorSage.Core.Models.Enums;

namespace RuntimeErrorSage.Core.Examples;

/// <summary>
/// Demonstrates proper Enum conversion patterns
/// </summary>
public static class EnumConversionExample
{
    /// <summary>
    /// Shows how to handle ErrorCategory enum to string conversion
    /// </summary>
    public static void EnumToStringConversion()
    {
        ErrorCategory category = ErrorCategory.RuntimeException;
        
        // INCORRECT: Direct assignment of enum to string
        /*
        string categoryStr = category; // Error: Cannot implicitly convert type 'ErrorCategory' to 'string'
        */
        
        // CORRECT: Use ToString()
        string categoryStr = category.ToString();
        Console.WriteLine($"Category: {categoryStr}");
        
        // ALTERNATIVE: Use a mapping dictionary for custom string representations
        var categoryMappings = new Dictionary<ErrorCategory, string>
        {
            { ErrorCategory.RuntimeException, "Runtime Exception" },
            { ErrorCategory.CompilationError, "Compilation Error" },
            // Add other mappings as needed
        };
        
        string customCategoryStr = categoryMappings.TryGetValue(category, out var value)
            ? value
            : category.ToString();
            
        Console.WriteLine($"Custom category: {customCategoryStr}");
    }
} 
