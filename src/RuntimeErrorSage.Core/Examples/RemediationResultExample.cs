using System;
using RuntimeErrorSage.Core.Models.Remediation;

namespace RuntimeErrorSage.Core.Examples
{
    /// <summary>
    /// Example code showing how to fix RemediationResult.Success issues.
    /// </summary>
    public class RemediationResultExample
    {
        /// <summary>
        /// Shows incorrect vs correct ways to set Success property.
        /// </summary>
        public void ShowRemediationResultSuccessExample()
        {
            // INCORRECT: Treating Success as a field when it's a property or method group
            // var result = new RemediationResult { Success = true };
            
            // CORRECT: Use the appropriate initialization method based on the class design
            // Option 1: Using a constructor parameter (if the class is designed this way)
            var result1 = new RemediationResult(true);
            
            // Option 2: Using a setter method (if the class is designed this way)
            var result2 = new RemediationResult();
            result2.SetSuccess(true);
            
            // Option 3: Using factory methods (if the class is designed this way)
            var successResult = RemediationResult.Success("Operation completed successfully");
            var failureResult = RemediationResult.Failure("Operation failed");
        }

        /// <summary>
        /// Shows how to handle RemediationActionResult Success property.
        /// </summary>
        public void ShowActionResultSuccessExample()
        {
            // INCORRECT: Treating Success as a field when it's a property or method group
            // var result = new RemediationActionResult { Success = true };
            
            // CORRECT: Use the appropriate initialization method based on the class design
            // Option 1: Using a constructor parameter (if the class is designed this way)
            var result1 = new RemediationActionResult(true);
            
            // Option 2: Using a setter method (if the class is designed this way)
            var result2 = new RemediationActionResult();
            result2.SetSuccess(true);
            
            // Option 3: Using factory methods (if the class is designed this way)
            var successResult = RemediationActionResult.Success();
            var failureResult = RemediationActionResult.Failure("Action failed");
        }

        /// <summary>
        /// Shows how to check success status without treating it as a field.
        /// </summary>
        /// <param name="result">The remediation result.</param>
        public void HandleRemediationResult(RemediationResult result)
        {
            // INCORRECT: Treating Success as a boolean field
            // if (result.Success) { ... }
            
            // CORRECT: Use the appropriate method to check success based on the class design
            // Option 1: Using a property accessor (if the class is designed this way)
            if (result.IsSuccess)
            {
                Console.WriteLine("Remediation was successful");
            }
            
            // Option 2: Using a method (if the class is designed this way)
            if (result.WasSuccessful())
            {
                Console.WriteLine("Remediation was successful");
            }
        }
    }
} 
