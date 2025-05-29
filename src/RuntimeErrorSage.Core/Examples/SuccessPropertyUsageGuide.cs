using System;
using RuntimeErrorSage.Model.Models.Remediation;

namespace RuntimeErrorSage.Model.Examples
{
    /// <summary>
    /// Guide for using Success/IsSuccessful properties correctly
    /// </summary>
    public static class SuccessPropertyUsageGuide
    {
        /// <summary>
        /// DON'T use Success in object initializers
        /// Error: 'Member 'Success' cannot be initialized. It is not a field or property.'
        /// </summary>
        public static void DontUseSuccessInInitializer()
        {
            // INCORRECT: Success is a property getter/setter that maps to IsSuccessful
            /*
            var result = new RemediationResult 
            {
                Success = true  // Error: Member 'Success' cannot be initialized
            };
            */
            
            // INCORRECT: Same issue with RemediationActionResult
            /*
            var actionResult = new RemediationActionResult 
            {
                Success = true  // Error: Member 'Success' cannot be initialized
            };
            */
        }
        
        /// <summary>
        /// DO use IsSuccessful property in initializers
        /// </summary>
        public static void UseIsSuccessfulInInitializer()
        {
            // CORRECT: Use IsSuccessful which is the actual backing property
            var result = new RemediationResult 
            {
                IsSuccessful = true  // This works correctly
            };
            
            var actionResult = new RemediationActionResult 
            {
                IsSuccessful = true  // This works correctly
            };
        }
        
        /// <summary>
        /// DO use Success property after initialization
        /// </summary>
        public static void UseSuccessAfterInitialization()
        {
            // CORRECT: Set Success after initialization
            var result = new RemediationResult();
            result.Success = true;  // This works correctly
            
            var actionResult = new RemediationActionResult();
            actionResult.Success = true;  // This works correctly
        }
        
        /// <summary>
        /// DO use factory methods for creating success/failure results
        /// </summary>
        public static void UseFactoryMethods()
        {
            // CORRECT: Use static factory methods
            var successResult = RemediationResult.Success("Operation completed successfully");
            var failureResult = RemediationResult.Failure("Operation failed");
            
            var successActionResult = RemediationActionResult.Success();
            var failureActionResult = RemediationActionResult.Failure("Action failed");
        }
        
        /// <summary>
        /// DO check IsSuccessful property in conditions
        /// </summary>
        public static void CheckIsSuccessfulInConditions()
        {
            var result = new RemediationResult();
            
            // CORRECT: Check IsSuccessful directly
            if (result.IsSuccessful)
            {
                Console.WriteLine("Operation succeeded");
            }
            
            // CORRECT: Check Success which maps to IsSuccessful
            if (result.Success)
            {
                Console.WriteLine("Operation succeeded");
            }
            
            // INCORRECT: Cannot assign to 'Success' because it is a method group
            /*
            result.Success() = true;  // Error: Cannot assign to 'Success' because it is a method group
            */
        }
        
        /// <summary>
        /// Examples of fixed places where Success was previously used incorrectly
        /// </summary>
        public static void FixedSuccessUsageExamples()
        {
            var result = new RemediationResult();
            var actionResult = new RemediationActionResult();
            
            // BEFORE:
            // if (!actionResult.Success) { result.Success = false; }
            
            // AFTER:
            if (!actionResult.IsSuccessful)
            {
                result.IsSuccessful = false;
            }
            
            // ALTERNATIVE:
            if (!actionResult.Success)
            {
                result.Success = false;
            }
        }
    }
} 
