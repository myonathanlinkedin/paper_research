$report = @()
$excludePatterns = @("*Test*.cs", "*Tests*.cs", "*\Test*\*", "*\Tests*\*")

Get-ChildItem -Path . -Filter *.cs -Recurse | Where-Object {
    $file = $_
    -not ($excludePatterns | Where-Object { $file.FullName -like $_ })
} | ForEach-Object {
    $file = $_
    $content = Get-Content $file.FullName -Raw
    
    # Count types more accurately by looking for class/interface/enum declarations
    $classMatches = [regex]::Matches($content, "(?m)^\s*(?:public|private|internal|protected)?\s*class\s+\w+")
    $enumMatches = [regex]::Matches($content, "(?m)^\s*(?:public|private|internal|protected)?\s*enum\s+\w+")
    $interfaceMatches = [regex]::Matches($content, "(?m)^\s*(?:public|private|internal|protected)?\s*interface\s+\w+")
    
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
    
    $report += "File: $($file.FullName)"
    if ($totalTypes -gt 1) {
        $report += "VIOLATION: Multiple types found ($totalTypes total)"
        $report += "- Classes: $($classMatches.Count)"
        $report += "- Enums: $($enumMatches.Count)"
        $report += "- Interfaces: $($interfaceMatches.Count)"
    }
    if ($largeTypes.Count -gt 0) {
        $report += "Large Types:"
        $largeTypes | ForEach-Object { $report += "- $_" }
    }
    if ($largeMethods.Count -gt 0) {
        $report += "Large Methods:"
        $largeMethods | ForEach-Object { $report += "- $_" }
    }
    if ($tightCouplings.Count -gt 0) {
        $report += "Tight Couplings:"
        $tightCouplings | ForEach-Object { $report += "- $_" }
    }
    $report += "`n"
}

$report | Out-File -FilePath solid_report.txt 