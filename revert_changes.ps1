# revert_changes.ps1
# This script reverts all files that only have changes adding blank lines (newlines) at the end of the file.

# Get the list of modified files
$modifiedFiles = git diff --name-only

foreach ($file in $modifiedFiles) {
    # Check if the file only has changes adding blank lines (newlines) at the end of the file
    $diff = git diff --unified=0 $file
    # Get all added lines (lines starting with '+')
    $addedLines = $diff | Select-String '^\+' | ForEach-Object { $_.Line.Trim() }
    # Remove the diff header (e.g., +++ b/file)
    $addedLines = $addedLines | Where-Object { $_ -notmatch '^\+\+\+' }
    # If all added lines are empty (i.e., only blank lines were added)
    if ($addedLines.Count -gt 0 -and ($addedLines | Where-Object { $_ -ne '+' -and $_ -ne '+\r' -and $_ -ne '+\n' -and $_ -ne '+' }).Count -eq 0) {
        Write-Host "Reverting $file (only blank lines added)"
        git checkout -- $file
    }
}

Write-Host "Revert complete." 