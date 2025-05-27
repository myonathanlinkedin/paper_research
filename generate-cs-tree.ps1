$root = "."
function Show-Tree($path, $prefix = "") {
    $dirs = Get-ChildItem -Path $path -Directory | Sort-Object Name
    $files = Get-ChildItem -Path $path -File -Filter *.cs | Sort-Object Name
    foreach ($file in $files) {
        Write-Output "$prefix|-- $($file.Name)"
    }
    for ($i = 0; $i -lt $dirs.Count; $i++) {
        $dir = $dirs[$i]
        Write-Output "$prefix|-- $($dir.Name)\\"
        Show-Tree -path $dir.FullName -prefix:("$prefix|   ")
    }
}
"Project Structure:" | Out-File projectstructure.txt
Show-Tree $root | Out-File projectstructure.txt -Append 