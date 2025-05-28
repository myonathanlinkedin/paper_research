param (
    [string]$StructureFile = "projectstructure.txt",
    [string]$ReportFile = "solid_report.txt"
)

# Function to log messages
function Write-Log {
    param ([string]$message)
    Write-Host $message
    Add-Content -Path "fix_violations_log.txt" -Value $message
}

Write-Log "Starting SOLID violations remediation..."
Write-Log "Structure file: $StructureFile"
Write-Log "Report file: $ReportFile"

# Fix CS0101 errors (duplicate type definitions in the same namespace)
Write-Log "Fixing duplicate type definitions..."

# Find and remove ErrorEnums.cs file if it exists
if (Test-Path "src\RuntimeErrorSage.Core\Models\Enums\ErrorEnums.cs") {
    Write-Log "Removing src\RuntimeErrorSage.Core\Models\Enums\ErrorEnums.cs to fix duplicate enum definitions"
    Remove-Item "src\RuntimeErrorSage.Core\Models\Enums\ErrorEnums.cs" -Force
}

# Ensure AggregationType enum exists
if (-not (Test-Path "src\RuntimeErrorSage.Core\Models\Enums\AggregationType.cs")) {
    Write-Log "Creating AggregationType enum..."
    @"
namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the type of aggregation for metrics collection.
/// </summary>
public enum AggregationType
{
    /// <summary>
    /// Sum of values.
    /// </summary>
    Sum = 0,
    
    /// <summary>
    /// Average of values.
    /// </summary>
    Average = 1,
    
    /// <summary>
    /// Maximum value.
    /// </summary>
    Max = 2,
    
    /// <summary>
    /// Minimum value.
    /// </summary>
    Min = 3,
    
    /// <summary>
    /// Count of values.
    /// </summary>
    Count = 4
}
"@ | Set-Content "src\RuntimeErrorSage.Core\Models\Enums\AggregationType.cs"
}

# Ensure AnalysisValidationStatus enum exists
if (-not (Test-Path "src\RuntimeErrorSage.Core\Models\Enums\AnalysisValidationStatus.cs")) {
    Write-Log "Creating AnalysisValidationStatus enum..."
    @"
namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the validation status of an analysis.
/// </summary>
public enum AnalysisValidationStatus
{
    /// <summary>
    /// The analysis is valid.
    /// </summary>
    Valid = 0,
    
    /// <summary>
    /// The analysis is invalid.
    /// </summary>
    Invalid = 1,
    
    /// <summary>
    /// The analysis has warnings.
    /// </summary>
    Warning = 2,
    
    /// <summary>
    /// The analysis status is unknown.
    /// </summary>
    Unknown = 3
}
"@ | Set-Content "src\RuntimeErrorSage.Core\Models\Enums\AnalysisValidationStatus.cs"
}

# Ensure RemediationRiskLevel enum exists
if (-not (Test-Path "src\RuntimeErrorSage.Core\Models\Enums\RemediationRiskLevel.cs")) {
    Write-Log "Creating RemediationRiskLevel enum..."
    @"
namespace RuntimeErrorSage.Core.Models.Enums;

/// <summary>
/// Defines the risk level of a remediation action.
/// </summary>
public enum RemediationRiskLevel
{
    /// <summary>
    /// Unknown risk level.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Low risk level.
    /// </summary>
    Low = 1,
    
    /// <summary>
    /// Medium risk level.
    /// </summary>
    Medium = 2,
    
    /// <summary>
    /// High risk level.
    /// </summary>
    High = 3,
    
    /// <summary>
    /// Critical risk level.
    /// </summary>
    Critical = 4
}
"@ | Set-Content "src\RuntimeErrorSage.Core\Models\Enums\RemediationRiskLevel.cs"
}

# Fix RemediationService implementation
$remediationServiceFile = "src\RuntimeErrorSage.Core\Services\Remediation\RemediationService.cs"
if (Test-Path $remediationServiceFile) {
    Write-Log "Updating RemediationService implementation..."
    $content = Get-Content $remediationServiceFile -Raw
    
    # Check if it already has correct namespace
    if (-not $content.Contains("using RuntimeErrorSage.Core.Interfaces;")) {
        $content = $content -replace "using RuntimeErrorSage.Core.Models.Remediation.Interfaces;", "using RuntimeErrorSage.Core.Interfaces;"
        Set-Content $remediationServiceFile $content
    }
}

# Fix interface reference in RemediationService
$remediationExecutorFile = "src\RuntimeErrorSage.Core\Remediation\RemediationExecutor.cs"
if (Test-Path $remediationExecutorFile) {
    Write-Log "Updating RemediationExecutor implementation..."
    $content = Get-Content $remediationExecutorFile -Raw
    
    # Fix the interface implementation
    if ($content.Contains("public async Task<RemediationResult> ExecuteStrategyAsync(Models.Remediation.Interfaces.IRemediationStrategy strategy, ErrorContext context)")) {
        $content = $content -replace "public async Task<RemediationResult> ExecuteStrategyAsync\(Models\.Remediation\.Interfaces\.IRemediationStrategy strategy, ErrorContext context\)", "public async Task<RemediationResult> ExecuteStrategyAsync(IRemediationStrategy strategy, ErrorContext context)"
        Set-Content $remediationExecutorFile $content
    }
}

# Fix IRemediationService interface usage
$files = Get-ChildItem -Path "src" -Filter "*.cs" -Recurse
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match "IRemediationService" -and -not $content.Contains("using RuntimeErrorSage.Core.Interfaces;")) {
        Write-Log "Adding correct namespace for IRemediationService in $($file.FullName)"
        $content = "using RuntimeErrorSage.Core.Interfaces;`r`n" + $content
        Set-Content $file.FullName $content
    }
}

# Fix IPatternStorage implementation in RedisPatternStorage
$redisPatternStorageFile = "src\RuntimeErrorSage.Core\Storage\RedisPatternStorage.cs"
if (Test-Path $redisPatternStorageFile) {
    Write-Log "Fixing RedisPatternStorage implementation..."
    $content = Get-Content $redisPatternStorageFile -Raw
    
    # Fix GetPatternsAsync return type
    if ($content -match "public async Task<IEnumerable<KeyValuePair<string, string>>> GetPatternsAsync\(string category\)") {
        $content = $content -replace "public async Task<IEnumerable<KeyValuePair<string, string>>> GetPatternsAsync\(string category\)", "public async Task<Dictionary<string, string>> GetPatternsAsync(string category)"
        
        # Also fix method implementation if needed
        if ($content -match "return result;") {
            $content = $content -replace "return result;", "return result.ToDictionary(kv => kv.Key, kv => kv.Value);"
        }
    }
    
    # Fix GetPatternCountAsync return type
    if ($content -match "public async Task<int> GetPatternCountAsync\(\)") {
        $content = $content -replace "public async Task<int> GetPatternCountAsync\(\)", "public async Task<long> GetPatternCountAsync()"
    }
    
    Set-Content $redisPatternStorageFile $content
}

Write-Log "SOLID violations remediation completed"

# Run a test build to check if issues have been fixed
Write-Log "Running test build to verify fixes..."
$buildResult = dotnet build --nologo
Add-Content -Path "build_after_fixes.txt" -Value $buildResult

# Check if build succeeded
$errorCount = ($buildResult | Select-String -Pattern "error").Count
Write-Log "Build completed with $errorCount errors"

if ($errorCount -eq 0) {
    Write-Log "All errors have been fixed successfully!"
} else {
    Write-Log "Some errors still remain. Check build_after_fixes.txt for details."
} 