using RuntimeErrorSage.Domain.Models.Remediation;
using RuntimeErrorSage.Application.Interfaces;

namespace RuntimeErrorSage.Application.Examples;

/// <summary>
/// Demonstrates proper RemediationAction conversion patterns
/// </summary>
public static class RemediationActionConversionExample
{
    /// <summary>
    /// Shows how to handle IRemediationAction to RemediationAction conversion
    /// </summary>
    public static void RemediationActionConversion()
    {
        var actions = new List<IRemediationAction>();
        
        // INCORRECT: Direct conversion from IRemediationAction to RemediationAction
        /*
        RemediationAction action = actions.First(); // Error: Cannot convert IRemediationAction to RemediationAction
        */
        
        // CORRECT: Check type and cast
        if (actions.Any() && actions.First() is RemediationAction action)
        {
            // Now use the action...
            Console.WriteLine($"Action ID: {action.ActionId}");
        }
        
        // ALTERNATIVE: Use LINQ to filter and cast
        var remediationActions = actions
            .OfType<RemediationAction>()
            .ToList();
        
        // Now you have a list of RemediationAction objects
        foreach (var action in remediationActions)
        {
            Console.WriteLine($"Action ID: {action.ActionId}");
        }
    }
} 


