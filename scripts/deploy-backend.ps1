$ErrorActionPreference = "Stop"

# -----------------------------
# Settings
# -----------------------------

$ProjectRoot = Split-Path -Parent $PSScriptRoot

$HelperScriptsRoot = Join-Path $PSScriptRoot "HelperScripts"

$ApiDeployScript = Join-Path $HelperScriptsRoot "deploy-api.ps1"
$IngestorDeployScript = Join-Path $HelperScriptsRoot "deploy-ingestor.ps1"

# -----------------------------
# Helper functions
# -----------------------------

function Write-Section {
    param(
        [string]$Title
    )

    Write-Host ""
    Write-Host "-----------------------------"
    Write-Host $Title
    Write-Host "-----------------------------"
}

function Test-ScriptExists {
    param(
        [string]$ScriptPath,
        [string]$ScriptName
    )

    if (-not (Test-Path $ScriptPath)) {
        throw "$ScriptName script not found at: $ScriptPath"
    }
}

function Invoke-DeployScript {
    param(
        [string]$ScriptPath,
        [string]$ScriptName
    )

    Write-Section "Running $ScriptName"

    & $ScriptPath

    if ($LASTEXITCODE -ne 0) {
        throw "$ScriptName failed."
    }
}

# -----------------------------
# Deploy flow
# -----------------------------

Set-Location $ProjectRoot

Test-ScriptExists $ApiDeployScript "API deploy"
Test-ScriptExists $IngestorDeployScript "Ingestor deploy"

Invoke-DeployScript $ApiDeployScript "API deploy"
Invoke-DeployScript $IngestorDeployScript "Ingestor deploy"

Write-Section "Deployment complete"

Write-Host "API and ingestor deployment scripts finished successfully."