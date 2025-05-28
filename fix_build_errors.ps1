# PowerShell script to fix build errors in the RuntimeErrorSage project

# Function to ensure all remediation action classes use the correct namespace
function Fix-Namespaces {
    param (
        [string]$rootDir
    )
    
    # Get all .cs files that contain remediation action classes
    $files = Get-ChildItem -Path $rootDir -Recurse -Filter "*.cs" | 
             Where-Object { $_.FullName -like "*RemediationAction*.cs" }
    
    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        
        # Ensure correct namespace is used
        if ($content -notmatch "using RuntimeErrorSage.Core.Models.Remediation;") {
            $content = "using RuntimeErrorSage.Core.Models.Remediation;`n" + $content
        }
        
        # Ensure correct namespace is used for interfaces
        if ($content -notmatch "using RuntimeErrorSage.Core.Models.Remediation.Interfaces;") {
            $content = "using RuntimeErrorSage.Core.Models.Remediation.Interfaces;`n" + $content
        }
        
        Set-Content -Path $file.FullName -Value $content
    }
}

# Function to ensure all remediation status references use Success instead of Completed
function Fix-RemediationStatus {
    param (
        [string]$rootDir
    )
    
    # Get all .cs files
    $files = Get-ChildItem -Path $rootDir -Recurse -Filter "*.cs"
    
    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        
        # Replace Completed with Success
        $content = $content -replace "RemediationStatusEnum\.Completed", "RemediationStatusEnum.Success"
        
        Set-Content -Path $file.FullName -Value $content
    }
}

# Function to ensure all remediation action classes inherit from the base class
function Fix-Inheritance {
    param (
        [string]$rootDir
    )
    
    # Get all remediation action class files except the base class
    $files = Get-ChildItem -Path $rootDir -Recurse -Filter "*.cs" | 
             Where-Object { $_.FullName -like "*RemediationAction*.cs" -and 
                          $_.FullName -notlike "*RemediationAction.cs" }
    
    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        
        # If the class doesn't inherit from RemediationAction, make it inherit
        if ($content -match "class\s+(\w+RemediationAction\w+)" -and 
            $content -notmatch ":\s*RemediationAction") {
            $className = $matches[1]
            $content = $content -replace "class\s+$className", "class $className : RemediationAction"
        }
        
        Set-Content -Path $file.FullName -Value $content
    }
}

# Main script execution
$rootDir = "src"

Write-Host "Fixing namespaces..."
Fix-Namespaces -rootDir $rootDir

Write-Host "Fixing remediation status references..."
Fix-RemediationStatus -rootDir $rootDir

Write-Host "Fixing inheritance..."
Fix-Inheritance -rootDir $rootDir

Write-Host "Build error fixes completed." 