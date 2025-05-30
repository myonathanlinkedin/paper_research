using System;
using RuntimeErrorSage.Domain.Models.Remediation;

namespace RuntimeErrorSage.Application.Examples
{
    /// <summary>
    /// Examples showing the correct way to set success state on remediation results
    /// </summary>
    public class RemediationResultSuccessFixExample
    {
        /// <summary>
        /// Shows correct vs incorrect ways to set Success property.
        /// </summary>
        public void ShowCorrectSuccessUsage()
        {
            // INCORRECT: Don't use object initializer with Success property
            // This will cause: 'Member 'Success' cannot be initialized. It is not a field or property.'
            // var result = new RemediationResult { Success = true };
            
            // CORRECT: Use the property setter after initialization
            var result1 = new RemediationResult();
            result1.Success = true;
            
            // CORRECT: Use IsSuccessful directly in object initializer
            var result2 = new RemediationResult { IsSuccessful = true };
            
            // CORRECT: Use the constructor with isSuccessful parameter
            var result3 = new RemediationResult(true);
            
            // CORRECT: Use the static Success factory method
            var result4 = RemediationResult.Success("Operation completed successfully");
        }
        
        /// <summary>
        /// Shows correct vs incorrect ways to set Success property on action results.
        /// </summary>
        public void ShowCorrectActionResultSuccessUsage()
        {
            // INCORRECT: Don't use object initializer with Success property
            // This will cause: 'Member 'Success' cannot be initialized. It is not a field or property.'
            // var result = new RemediationActionResult { Success = true };
            
            // CORRECT: Use the property setter after initialization
            var result1 = new RemediationActionResult();
            result1.Success = true;
            
            // CORRECT: Use IsSuccessful directly in object initializer
            var result2 = new RemediationActionResult { IsSuccessful = true };
            
            // CORRECT: Use the constructor with isSuccessful parameter
            var result3 = new RemediationActionResult(true);
            
            // CORRECT: Use the static Success factory method
            var result4 = RemediationActionResult.Success();
        }
        
        /// <summary>
        /// Shows correct methods for handling Success property in existing code.
        /// </summary>
        /// <param name="action">A remediation action to execute</param>
        /// <returns>The action execution result</returns>
        public RemediationActionResult ExecuteAction(RemediationAction action)
        {
            try
            {
                // Simulate execution
                Console.WriteLine($"Executing action {action.Name}");
                
                // INCORRECT: Don't create with Success in initializer
                // return new RemediationActionResult { Success = true };
                
                // CORRECT: Set property after initialization
                var result = new RemediationActionResult();
                result.Success = true;
                return result;
                
                // CORRECT ALTERNATIVE: Use static factory method
                // return RemediationActionResult.Success();
            }
            catch (Exception ex)
            {
                // INCORRECT: Don't create with Success in initializer
                // return new RemediationActionResult { Success = false, ErrorMessage = ex.Message };
                
                // CORRECT: Set property after initialization
                var result = new RemediationActionResult();
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
                
                // CORRECT ALTERNATIVE: Use static factory method
                // return RemediationActionResult.Failure(ex.Message);
            }
        }
    }
} 
