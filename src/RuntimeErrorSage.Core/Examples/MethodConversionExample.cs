using RuntimeErrorSage.Core.Models.Execution;

namespace RuntimeErrorSage.Core.Examples;

/// <summary>
/// Demonstrates proper Method conversion patterns
/// </summary>
public static class MethodConversionExample
{
    /// <summary>
    /// Shows how to handle method to double conversion
    /// </summary>
    public static void MethodToDoubleConversion()
    {
        var execution = new RemediationExecution();
        
        // INCORRECT: Trying to use a method as a double
        /*
        double duration = execution.DurationSeconds; // Error: Cannot convert method group to double
        */
        
        // CORRECT: Invoke the method/property
        double? durationSeconds = execution.DurationSeconds;
        
        // Handle the nullable value
        double durationValue = durationSeconds ?? 0;
        Console.WriteLine($"Duration: {durationValue} seconds");
        
        // ALTERNATIVE: Use extension method for consistent null handling
        double durationValue2 = durationSeconds.GetValueOrDefault();
        Console.WriteLine($"Duration (with extension): {durationValue2} seconds");
    }
} 