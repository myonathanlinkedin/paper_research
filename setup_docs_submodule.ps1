# Script to setup docs as private Git submodule
# This script helps convert the docs folder to a private submodule

param(
    [string]$PrivateRepoUrl = ""
)

Write-Host "=== RuntimeErrorSage Docs Submodule Setup ===" -ForegroundColor Cyan
Write-Host ""

# Step 1: Check if docs folder exists
if (-not (Test-Path "docs")) {
    Write-Host "ERROR: docs folder not found!" -ForegroundColor Red
    exit 1
}

Write-Host "Step 1: Checking current Git status..." -ForegroundColor Yellow
git status --short

# Check if private repo URL is provided
if ($PrivateRepoUrl -eq "") {
    Write-Host ""
    Write-Host "=== INSTRUCTIONS ===" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "To setup docs as private submodule, follow these steps:" -ForegroundColor White
    Write-Host ""
    Write-Host "1. Create a NEW PRIVATE repository on GitHub:" -ForegroundColor Yellow
    Write-Host "   - Go to: https://github.com/new" -ForegroundColor Gray
    Write-Host "   - Repository name: RuntimeErrorSage-docs (or similar)" -ForegroundColor Gray
    Write-Host "   - Set visibility to: PRIVATE" -ForegroundColor Gray
    Write-Host "   - DO NOT initialize with README, .gitignore, or license" -ForegroundColor Gray
    Write-Host ""
    Write-Host "2. After creating the private repo, run this command:" -ForegroundColor Yellow
    Write-Host "   .\setup_docs_submodule.ps1 -PrivateRepoUrl <your-private-repo-url>" -ForegroundColor Green
    Write-Host ""
    Write-Host "   Example:" -ForegroundColor Gray
    Write-Host "   .\setup_docs_submodule.ps1 -PrivateRepoUrl https://github.com/myonathanlinkedin/RuntimeErrorSage-docs.git" -ForegroundColor Gray
    Write-Host ""
    Write-Host "3. The script will:" -ForegroundColor Yellow
    Write-Host "   - Backup current docs folder" -ForegroundColor Gray
    Write-Host "   - Initialize the private repo with docs content" -ForegroundColor Gray
    Write-Host "   - Remove docs from main repo" -ForegroundColor Gray
    Write-Host "   - Add docs as submodule" -ForegroundColor Gray
    Write-Host ""
    Write-Host "No private repo URL provided. Showing instructions above." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "To proceed, create the private repo first, then run:" -ForegroundColor Cyan
    Write-Host "  .\setup_docs_submodule.ps1 -PrivateRepoUrl <your-private-repo-url>" -ForegroundColor Green
    exit 0
}

Write-Host "Starting submodule setup with URL: $PrivateRepoUrl" -ForegroundColor Green
Write-Host ""

# Step 2: Backup docs folder
Write-Host "Step 2: Creating backup of docs folder..." -ForegroundColor Yellow
$backupPath = "docs_backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
Copy-Item -Path "docs" -Destination $backupPath -Recurse -Force
Write-Host "Backup created at: $backupPath" -ForegroundColor Green

# Step 3: Create temporary directory for private repo
Write-Host "Step 3: Preparing private repository..." -ForegroundColor Yellow
$tempRepoPath = "temp_docs_repo"
if (Test-Path $tempRepoPath) {
    Remove-Item -Path $tempRepoPath -Recurse -Force
}
New-Item -ItemType Directory -Path $tempRepoPath | Out-Null

# Step 4: Initialize git repo in temp directory
Write-Host "Step 4: Initializing Git repository..." -ForegroundColor Yellow
Set-Location $tempRepoPath
git init
git branch -M main

# Step 5: Copy docs content
Write-Host "Step 5: Copying docs content..." -ForegroundColor Yellow
Set-Location ..
Copy-Item -Path "docs\*" -Destination "$tempRepoPath\" -Recurse -Force

# Step 6: Commit to private repo
Write-Host "Step 6: Committing to private repository..." -ForegroundColor Yellow
Set-Location $tempRepoPath
git add .
git commit -m "Initial commit: RuntimeErrorSage documentation"

# Step 7: Add remote and push
Write-Host "Step 7: Pushing to private repository..." -ForegroundColor Yellow
git remote add origin $PrivateRepoUrl
git push -u origin main

# Step 8: Go back to main repo
Set-Location ..

# Step 9: Remove docs folder from main repo
Write-Host "Step 8: Removing docs folder from main repository..." -ForegroundColor Yellow
if (Test-Path "docs") {
    Remove-Item -Path "docs" -Recurse -Force
}

# Step 10: Add as submodule
Write-Host "Step 9: Adding docs as submodule..." -ForegroundColor Yellow
git submodule add $PrivateRepoUrl docs

# Step 11: Cleanup temp directory
Write-Host "Step 10: Cleaning up temporary files..." -ForegroundColor Yellow
if (Test-Path $tempRepoPath) {
    Remove-Item -Path $tempRepoPath -Recurse -Force
}

Write-Host ""
Write-Host "=== SUCCESS ===" -ForegroundColor Green
Write-Host "Docs folder has been converted to a private submodule!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Commit the changes:" -ForegroundColor White
Write-Host "   git add .gitmodules docs" -ForegroundColor Gray
Write-Host "   git commit -m 'Convert docs folder to private submodule'" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Push to main repository:" -ForegroundColor White
Write-Host "   git push origin main" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Update README.md with submodule instructions" -ForegroundColor White
Write-Host ""
Write-Host "Note: Backup is available at: $backupPath" -ForegroundColor Cyan

