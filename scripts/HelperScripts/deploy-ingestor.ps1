$ErrorActionPreference = "Stop"

# -----------------------------
# Settings
# -----------------------------

$ProjectRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)

$ResourceGroup = "rg-wow-paper-trader-dev"
$IngestorJobName = "wow-paper-trader-ingestor-dev"

$ImageName = "ghcr.io/callumwilkinson/wow-paper-trader-ingestor"
$DockerfilePath = ".\WowPaperTrader.Ingestor\Dockerfile"

# -----------------------------
# Helper functions
# -----------------------------

function Stop-IfCommandFailed {
    param(
        [string]$ErrorMessage
    )

    if ($LASTEXITCODE -ne 0) {
        throw $ErrorMessage
    }
}

function Connect-AzureIfNeeded {
    Write-Host "Checking Azure login..."

    az account show --only-show-errors 1>$null 2>$null

    if ($LASTEXITCODE -eq 0) {
        return
    }

    Write-Host "You are not logged in to Azure."
    az login --only-show-errors

    Stop-IfCommandFailed "Azure login failed."
}

function Connect-GhcrIfNeeded {
    Write-Host "Checking GHCR login..."

    $DockerConfigPath = "$env:USERPROFILE\.docker\config.json"

    if (Test-Path $DockerConfigPath) {
        $DockerConfigText = Get-Content $DockerConfigPath -Raw

        if ($DockerConfigText -like "*ghcr.io*") {
            return
        }
    }

    Write-Host "You do not appear to be logged in to GHCR."

    $GitHubUsername = Read-Host "GitHub username"
    $GitHubToken = Read-Host "GitHub classic PAT with write:packages and read:packages" -AsSecureString

    $TokenPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($GitHubToken)

    try {
        $PlainToken = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($TokenPointer)
        $PlainToken | docker login ghcr.io -u $GitHubUsername --password-stdin
    }
    finally {
        [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($TokenPointer)
        $PlainToken = $null
    }

    Stop-IfCommandFailed "GHCR login failed."
}

function Test-IngestorJobExists {
    Write-Host "Checking Azure Container Apps Job..."

    az containerapp job show `
      --name $IngestorJobName `
      --resource-group $ResourceGroup `
      --only-show-errors `
      1>$null 2>$null

    Stop-IfCommandFailed "Could not find Container Apps Job: $IngestorJobName"
}

function New-ImageTag {
    $Timestamp = Get-Date -Format "yyyyMMdd-HHmmss"

    return "dev-$Timestamp"
}

function Build-DockerImage {
    param(
        [string]$Image
    )

    Write-Host "Building Docker image..."

    docker build `
      -t $Image `
      -f $DockerfilePath `
      .

    Stop-IfCommandFailed "Docker build failed."
}

function Push-DockerImage {
    param(
        [string]$Image
    )

    Write-Host "Pushing Docker image to GHCR..."

    docker push $Image

    Stop-IfCommandFailed "Docker push failed. Check your GHCR token permissions."
}

function Update-IngestorJobImage {
    param(
        [string]$Image
    )

    Write-Host "Updating Azure Container Apps Job..."

    az containerapp job update `
      --name $IngestorJobName `
      --resource-group $ResourceGroup `
      --image $Image `
      --only-show-errors

    Stop-IfCommandFailed "Azure Container Apps Job update failed."
}

function Get-DeployedIngestorImage {
    return az containerapp job show `
      --name $IngestorJobName `
      --resource-group $ResourceGroup `
      --query "properties.template.containers[0].image" `
      --only-show-errors `
      -o tsv
}

# -----------------------------
# Deploy flow
# -----------------------------

Set-Location $ProjectRoot

if (-not (Test-Path $DockerfilePath)) {
    throw "Dockerfile not found at: $DockerfilePath"
}

Connect-AzureIfNeeded
Connect-GhcrIfNeeded
Test-IngestorJobExists

$ImageTag = New-ImageTag
$Image = "${ImageName}:${ImageTag}"

Write-Host ""
Write-Host "Image to deploy:"
Write-Host $Image
Write-Host ""

Build-DockerImage $Image
Push-DockerImage $Image
Update-IngestorJobImage $Image

$DeployedImage = Get-DeployedIngestorImage

Write-Host ""
Write-Host "Deployment complete."
Write-Host "Job:   $IngestorJobName"
Write-Host "Image: $DeployedImage"
Write-Host ""