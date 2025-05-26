using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RuntimeErrorSage.Core.Models.Error;
using RuntimeErrorSage.Core.Models.Remediation;
using RuntimeErrorSage.Core.Remediation.Interfaces;

namespace RuntimeErrorSage.Core.Remediation.Strategies
{
    /// <summary>
    /// Strategy for backing up system components.
    /// </summary>
    public class BackupStrategy : RemediationStrategy
    {
        public BackupStrategy(
            ILogger<RemediationStrategy> logger,
            IRemediationValidator validator)
            : base(logger, validator, "Backup", "Creates backups of system components", 3)
        {
        }

        public override async Task<RemediationResult> ExecuteAsync(ErrorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            try
            {
                await ValidateParametersAsync();

                // Validate strategy before execution
                var validationResult = await ValidateAsync(context);
                if (!validationResult.IsValid)
                {
                    return await CreateFailureResultAsync(
                        $"Strategy validation failed: {string.Join(", ", validationResult.Errors)}");
                }

                // Execute backup logic
                var target = Parameters["target"];
                var schedule = Parameters["schedule"];

                // Simulate backup creation
                await CreateBackupAsync(target, schedule);
                
                return await CreateSuccessResultAsync(
                    $"Backup created for target '{target}' according to schedule '{schedule}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing backup strategy");
                return await CreateFailureResultAsync("Failed to execute backup strategy", ex);
            }
        }

        protected override IEnumerable<string> GetRequiredParameters()
        {
            return new[] { "target", "schedule" };
        }

        private async Task CreateBackupAsync(string target, string schedule)
        {
            // Simulate backup creation
            await Task.Delay(100);
            _logger.LogInformation(
                "Backup created for target {Target} according to schedule {Schedule}",
                target, schedule);
        }
    }
} 