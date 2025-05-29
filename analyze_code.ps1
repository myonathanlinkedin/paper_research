# Run code analysis and generate SOLID report
$analysisOutput = "solid_report.txt"

# Clear previous report
"" | Out-File $analysisOutput

# Analyze each .cs file
Get-ChildItem -Recurse -Filter "*.cs" | ForEach-Object {
    $content = Get-Content $_.FullName
    
    # Check for SOLID violations
    $violations = @()
    
    # Single Responsibility Principle
    if (($content | Select-String -Pattern "class.*\{").Count -gt 1) {
        $violations += "SRP: Multiple classes in single file"
    }
    
    # Open/Closed Principle
    if ($content -match "if.*type.*|switch.*type") {
        $violations += "OCP: Type checking instead of polymorphism"
    }
    
    # Liskov Substitution Principle
    if ($content -match "is\s+\w+|as\s+\w+") {
        $violations += "LSP: Type checking in inheritance hierarchy"
    }
    
    # Interface Segregation Principle
    if ($content -match "interface.*\{.*\w+\s+\w+\s+\w+.*\}") {
        $violations += "ISP: Large interface with multiple responsibilities"
    }
    
    # Dependency Inversion Principle
    if ($content -match "new\s+\w+") {
        $violations += "DIP: Direct instantiation instead of dependency injection"
    }
    
    # Write violations to report
    if ($violations.Count -gt 0) {
        "File: $($_.FullName)" | Out-File $analysisOutput -Append
        $violations | ForEach-Object { "  $_" } | Out-File $analysisOutput -Append
        "" | Out-File $analysisOutput -Append
    }
} 