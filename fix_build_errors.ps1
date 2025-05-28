# ========================================
# FULL BUILD ERROR FIXING WORKFLOW (SILENT)
# ========================================

# STEP 0: Initial Compile — Identify Errors
dotnet build --nologo | Tee-Object -FilePath initial_build_output.txt

if (!(Select-String -Path initial_build_output.txt -Pattern 'error')) {
    exit 0
}

# STEP 1: Snapshot Project Structure
.\generate-cs-tree.ps1 > projectstructure.txt

# STEP 2: Static Code Report (Excludes Tests)
.\analyze_code.ps1 > solid_report.txt

# STEP 2.1: Optional — Aggregate .tex files from /paper
Get-ChildItem -Path .\paper -Recurse -Filter *.tex | Get-Content | Set-Content -Encoding UTF8 paper_read.txt

# STEP 3: Generate Fix Plan — Prioritize Critical Errors First
.\generate_fix_plan.ps1 -InputPath initial_build_output.txt -OutputPath fix_plan.txt

# STEP 4: Apply Fix Plan Automatically
.\apply_fix_plan.ps1 -PlanPath fix_plan.txt

# STEP 5: Rebuild to Validate Fixes
dotnet build --nologo | Tee-Object -FilePath build_output_after_fix.txt

if (Select-String -Path build_output_after_fix.txt -Pattern 'error') {
    exit 1
}

# STEP 6: Sanity Checks and Final Pass
.\generate-cs-tree.ps1 > projectstructure.txt
.\analyze_code.ps1 > solid_report.txt
dotnet build --nologo | Tee-Object -FilePath final_build_output.txt 