# Setup Documentation as Private Submodule

This guide explains how to set up the `docs/` folder as a private Git submodule.

## Overview

The documentation is stored in a **private repository** and linked to this public repository as a Git submodule. This allows:
- ✅ Main repository remains public
- ✅ Documentation stays private
- ✅ Documentation is still accessible to authorized users

## Prerequisites

1. Access to create a private repository on GitHub
2. Git installed and configured
3. PowerShell (for Windows) or Bash (for Linux/Mac)

## Step-by-Step Setup

### Step 1: Create Private Repository

1. Go to [GitHub New Repository](https://github.com/new)
2. Repository name: `RuntimeErrorSage-docs` (or your preferred name)
3. **Set visibility to: PRIVATE** ⚠️
4. **DO NOT** initialize with README, .gitignore, or license
5. Click "Create repository"
6. Copy the repository URL (e.g., `https://github.com/yourusername/RuntimeErrorSage-docs.git`)

### Step 2: Run Setup Script

Run the PowerShell script with your private repository URL:

```powershell
.\setup_docs_submodule.ps1 -PrivateRepoUrl https://github.com/yourusername/RuntimeErrorSage-docs.git
```

The script will:
1. ✅ Create a backup of the current `docs/` folder
2. ✅ Initialize the private repository with docs content
3. ✅ Push documentation to the private repository
4. ✅ Remove `docs/` from the main repository
5. ✅ Add `docs/` as a submodule pointing to the private repository

### Step 3: Commit Changes

After the script completes, commit the changes:

```bash
git add .gitmodules docs
git commit -m "Convert docs folder to private submodule"
git push origin main
```

## Using the Repository with Submodule

### Cloning with Submodules

When cloning the repository, include submodules:

```bash
git clone --recurse-submodules https://github.com/myonathanlinkedin/paper_research.git
```

### Cloning Without Submodules (Default)

If you clone normally, the `docs/` folder will be empty. To initialize it:

```bash
git submodule init
git submodule update
```

Or in one command:

```bash
git submodule update --init --recursive
```

### Updating Submodule

To update the documentation submodule to the latest version:

```bash
cd docs
git pull origin main
cd ..
```

Or from the root:

```bash
git submodule update --remote docs
```

## Access Control

### Granting Access to Documentation

To give someone access to the documentation:

1. Go to your private documentation repository settings
2. Navigate to "Collaborators" or "Manage access"
3. Add the user with appropriate permissions (Read, Write, or Admin)

### Important Notes

- ⚠️ Users who clone the public repository **will not automatically have access** to the private submodule
- ⚠️ They need to be added as collaborators to the private repository
- ⚠️ Without access, `git submodule update` will fail with authentication errors

## Troubleshooting

### Submodule is Empty

If the `docs/` folder appears empty:

```bash
git submodule init
git submodule update
```

### Authentication Errors

If you get authentication errors when updating the submodule:

1. Ensure you have access to the private repository
2. Check your Git credentials:
   ```bash
   git config --global credential.helper
   ```
3. You may need to use SSH instead of HTTPS:
   ```bash
   git submodule set-url docs git@github.com:yourusername/RuntimeErrorSage-docs.git
   ```

### Removing Submodule (if needed)

If you need to remove the submodule:

```bash
git submodule deinit -f docs
git rm -f docs
rm -rf .git/modules/docs
```

## Manual Setup (Alternative)

If you prefer to set up manually instead of using the script:

1. Create private repository on GitHub
2. Clone it locally:
   ```bash
   git clone https://github.com/yourusername/RuntimeErrorSage-docs.git temp_docs
   ```
3. Copy docs content:
   ```bash
   cp -r docs/* temp_docs/
   ```
4. Commit and push:
   ```bash
   cd temp_docs
   git add .
   git commit -m "Initial commit: RuntimeErrorSage documentation"
   git push origin main
   ```
5. Remove docs from main repo:
   ```bash
   cd ..
   rm -rf docs
   ```
6. Add as submodule:
   ```bash
   git submodule add https://github.com/yourusername/RuntimeErrorSage-docs.git docs
   ```

## Verification

After setup, verify the submodule is working:

```bash
git submodule status
```

You should see the submodule with a commit hash, indicating it's properly initialized.

