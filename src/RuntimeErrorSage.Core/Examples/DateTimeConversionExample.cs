namespace RuntimeErrorSage.Application.Examples;

/// <summary>
/// Demonstrates proper DateTime conversion patterns
/// </summary>
public static class DateTimeConversionExample
{
    /// <summary>
    /// Shows how to handle DateTime? to DateTime conversion
    /// </summary>
    public static void NullableDateTimeConversion()
    {
        DateTime? nullableDate = DateTime.UtcNow;

        if (nullableDate.HasValue)
        {
            DateTime date = nullableDate.Value;
            Console.WriteLine($"Date: {date}");
        }
        
        // ALTERNATIVE: Use null-coalescing operator
        DateTime date2 = nullableDate ?? DateTime.MinValue;
        Console.WriteLine($"Date (with default): {date2}");
        
        // ALTERNATIVE: Use extension method for consistency
        DateTime date3 = nullableDate.GetValueOrDefault(DateTime.MinValue);
        Console.WriteLine($"Date (with extension): {date3}");
    }
} 
