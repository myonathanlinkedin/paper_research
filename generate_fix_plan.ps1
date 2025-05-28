param(
    [Parameter(Mandatory=$true)]
    [string]$InputPath,
    
    [Parameter(Mandatory=$true)]
    [string]$OutputPath
)

# Priority error codes
$priorityErrors = @(
    "CS0101",  # Duplicate type definitions
    "CS0117",  # Member does not exist
    "CS1061",  # Type does not contain definition
    "CS1929",  # Extension method error
    "CS7036",  # Missing argument
    "CS0029",  # Cannot implicitly convert
    "CS0246",  # Type or namespace not found
    "CS0234"   # Type or namespace does not exist
)

# Read build output
$buildOutput = Get-Content -Path $InputPath

# Initialize fix plan
$fixPlan = @()

# Process errors by priority
foreach ($errorCode in $priorityErrors) {
    $errors = $buildOutput | Select-String -Pattern $errorCode
    foreach ($error in $errors) {
        $fixPlan += "PRIORITY: $errorCode"
        $fixPlan += "ERROR: $($error.Line)"
        $fixPlan += "ACTION: Fix $errorCode error"
        $fixPlan += "---"
    }
}

# Process remaining errors
$remainingErrors = $buildOutput | Select-String -Pattern "error CS\d+" | Where-Object {
    $errorCode = $_.Line -match "error (CS\d+)"
    if ($matches[1]) {
        -not ($priorityErrors -contains $matches[1])
    }
}

foreach ($error in $remainingErrors) {
    $fixPlan += "ERROR: $($error.Line)"
    $fixPlan += "ACTION: Fix error"
    $fixPlan += "---"
}

# Write fix plan to file
$fixPlan | Out-File -FilePath $OutputPath -Encoding UTF8 