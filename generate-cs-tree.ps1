# Function to create tree structure
function Get-FileTree {
    param (
        [string]$Path,
        [string]$Indent = "",
        [string]$Last = $true
    )
    
    $files = Get-ChildItem -Path $Path -File -Filter "*.cs"
    $dirs = Get-ChildItem -Path $Path -Directory
    
    # Process files
    foreach ($file in $files) {
        $prefix = if ($Last) { "+-- " } else { "+-- " }
        "$Indent$prefix$($file.Name)" | Out-File -FilePath "projectstructure.txt" -Append
    }
    
    # Process directories
    foreach ($dir in $dirs) {
        $prefix = if ($Last) { "+-- " } else { "+-- " }
        "$Indent$prefix$($dir.Name)/" | Out-File -FilePath "projectstructure.txt" -Append
        $newIndent = if ($Last) { "$Indent    " } else { "$Indent|   " }
        Get-FileTree -Path $dir.FullName -Indent $newIndent -Last ($dir -eq $dirs[-1])
    }
}

# Clear existing file
"" | Out-File -FilePath "projectstructure.txt"

# Start tree generation from current directory
Get-FileTree -Path "." 