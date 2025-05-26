using System;
using System.Collections.Generic;
using RuntimeErrorSage.Core.Models.Validation;

namespace RuntimeErrorSage.Core.Models.Remediation
{
    /// <summary>
    /// Represents the result of a remediation validation operation.
    /// </summary>
    public class RemediationValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
        public ValidationMetadata ValidationMetadata { get; set; } = new();

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public void AddMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public static RemediationValidationResult Success()
        {
            return new RemediationValidationResult { IsValid = true };
        }

        public static RemediationValidationResult Failure(string error)
        {
            var result = new RemediationValidationResult { IsValid = false };
            result.AddError(error);
            return result;
        }
    }

    public class ValidationMetadata
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string ValidatorId { get; set; } = string.Empty;
        public string ValidatorVersion { get; set; } = string.Empty;
        public Dictionary<string, object> Properties { get; set; } = new();
    }
} 