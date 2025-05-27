$report = @()
$excludePatterns = @(
    "*Test*.cs",
    "*Tests*.cs",
    "*\Test*\*",
    "*\Tests*\*",
    "*\UnitTests\*",
    "*\IntegrationTests\*",
    "*\TestHelpers\*",
    "*\TestData\*",
    "*\TestFixtures\*",
    "*\TestUtils\*",
    "*\TestInfrastructure\*",
    "*\TestSupport\*",
    "*\TestCommon\*",
    "*\TestBase\*",
    "*\TestFramework\*",
    "*\TestRunner\*",
    "*\TestResults\*"
)

Get-ChildItem -Path . -Filter *.cs -Recurse | Where-Object {
    $file = $_
    -not ($excludePatterns | Where-Object { $file.FullName -like $_ })
} | ForEach-Object {
    $file = $_
    $content = Get-Content $file.FullName -Raw
    
    # Count types more accurately by looking for class/interface/enum declarations
    $classMatches = [regex]::Matches($content, "(?m)^\s*(?:public|private|internal|protected)?\s*class\s+(\w+)")
    $enumMatches = [regex]::Matches($content, "(?m)^\s*(?:public|private|internal|protected)?\s*enum\s+(\w+)")
    $interfaceMatches = [regex]::Matches($content, "(?m)^\s*(?:public|private|internal|protected)?\s*interface\s+(\w+)")
    
    $totalTypes = $classMatches.Count + $enumMatches.Count + $interfaceMatches.Count
    
    # Find large types (>200 lines)
    $largeTypes = @()
    $currentType = ""
    $typeStartLine = 0
    $lineCount = 0
    $lines = $content -split "`n"
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        if ($line -match "(?m)^\s*(?:public|private|internal|protected)?\s*(class|enum|interface)\s+(\w+)") {
            if ($currentType -ne "") {
                if ($lineCount -gt 200) {
                    $largeTypes += "$currentType ($lineCount lines)"
                }
            }
            $currentType = $matches[2]
            $typeStartLine = $i
            $lineCount = 0
        }
        $lineCount++
    }
    
    # Check for large methods (>50 lines)
    $largeMethods = @()
    $currentMethod = ""
    $methodStartLine = 0
    $methodLineCount = 0
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        if ($line -match "(?m)^\s*(?:public|private|internal|protected)?\s*(?:static\s+)?(?:async\s+)?(?:[\w<>[\],\s]+)\s+(\w+)\s*\([^)]*\)\s*{") {
            if ($currentMethod -ne "") {
                if ($methodLineCount -gt 50) {
                    $largeMethods += "$currentMethod ($methodLineCount lines)"
                }
            }
            $currentMethod = $matches[1]
            $methodStartLine = $i
            $methodLineCount = 0
        }
        $methodLineCount++
    }
    
    # Find tight coupling (new keyword usage)
    $tightCouplings = @()
    $newMatches = [regex]::Matches($content, "(?m)^\s*new\s+(\w+)")
    foreach ($match in $newMatches) {
        $tightCouplings += "Line $($match.Index): new $($match.Groups[1].Value)"
    }
    
    $hasViolation = $false
    $violationReport = "`nFile: $($file.FullName)"
    if ($totalTypes -gt 1) {
        $hasViolation = $true
        $violationReport += "VIOLATION: Multiple types found ($totalTypes total)"
        $violationReport += "Types found:"
        foreach ($match in $classMatches) {
            $violationReport += "- Class: $($match.Groups[1].Value)"
        }
        foreach ($match in $enumMatches) {
            $violationReport += "- Enum: $($match.Groups[1].Value)"
        }
        foreach ($match in $interfaceMatches) {
            $violationReport += "- Interface: $($match.Groups[1].Value)"
        }
        $violationReport += "SUGGESTION: Consider splitting these types into separate files following the Single Responsibility Principle"
    }
    if ($largeTypes.Count -gt 0) {
        $hasViolation = $true
        $violationReport += "`nLarge Types (over 200 lines):"
        $largeTypes | ForEach-Object { $violationReport += "- $_" }
        $violationReport += "SUGGESTION: Consider breaking down these large types into smaller, more focused components"
    }
    if ($largeMethods.Count -gt 0) {
        $hasViolation = $true
        $violationReport += "`nLarge Methods (over 50 lines):"
        $largeMethods | ForEach-Object { $violationReport += "- $_" }
        $violationReport += "SUGGESTION: Consider refactoring these methods into smaller, more focused methods"
    }
    if ($tightCouplings.Count -gt 0) {
        $hasViolation = $true
        $violationReport += "`nTight Couplings (direct instantiation):"
        $tightCouplings | ForEach-Object { $violationReport += "- $_" }
        $violationReport += "SUGGESTION: Consider using dependency injection instead of direct instantiation"
    }
    if ($hasViolation) {
        $violationReport += "`n----------------------------------------"
        $report += $violationReport
    }
}

$report | Out-File -FilePath solid_report.txt 