$ErrorActionPreference = "Stop"

# -----------------------------
# Settings
# -----------------------------

$ProjectRoot = "C:\Users\callu\Documents\Code\WowPaperTrader"

$ResourceGroup = "rg-wow-paper-trader-dev"
$ApiAppName = "wow-paper-trader-api-dev"

$ImageName = "ghcr.io/callumwilkinson/wow-paper-trader-api"
$DockerfilePath = ".\WowPaperTrader.Api\Dockerfile"

# -----------------------------
# Go to project folder
# -----------------------------

Set-Location $ProjectRoot

# -----------------------------
# Check Azure login
# -----------------------------

Write-Host "Checking Azure login..."

az account show 1>$null 2>$null

if ($LASTEXITCODE -ne 0) {
    Write-Host "You are not logged in to Azure."
    az login
}

if ($LASTEXITCODE -ne 0) {
    throw "Azure login failed."
}

# -----------------------------
# Check GHCR login
# -----------------------------

Write-Host "Checking GHCR login..."

$DockerConfigPath = "$env:USERPROFILE\.docker\config.json"
$IsLoggedInToGhcr = $false

if (Test-Path $DockerConfigPath) {
    $DockerConfigText = Get-Content $DockerConfigPath -Raw

    if ($DockerConfigText -like "*ghcr.io*") {
        $IsLoggedInToGhcr = $true
    }
}

if (-not $IsLoggedInToGhcr) {
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

    if ($LASTEXITCODE -ne 0) {
        throw "GHCR login failed."
    }
}

# -----------------------------
# Create a unique image tag
# -----------------------------

$Timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$ImageTag = "dev-$Timestamp"
$Image = "${ImageName}:${ImageTag}"

Write-Host ""
Write-Host "Image to deploy:"
Write-Host $Image
Write-Host ""

# -----------------------------
# Build image
# -----------------------------

Write-Host "Building Docker image..."

docker build `
  -t $Image `
  -f $DockerfilePath `
  .

if ($LASTEXITCODE -ne 0) {
    throw "Docker build failed."
}

# -----------------------------
# Push image
# -----------------------------

Write-Host "Pushing Docker image to GHCR..."

docker push $Image

if ($LASTEXITCODE -ne 0) {
    throw "Docker push failed. Check your GHCR token permissions."
}

# -----------------------------
# Update Azure Container App
# -----------------------------

Write-Host "Updating Azure Container App..."

az containerapp update `
  --name $ApiAppName `
  --resource-group $ResourceGroup `
  --image $Image

if ($LASTEXITCODE -ne 0) {
    throw "Azure Container App update failed."
}

# -----------------------------
# Get API URL
# -----------------------------

$ApiHostName = az containerapp show `
  --name $ApiAppName `
  --resource-group $ResourceGroup `
  --query "properties.configuration.ingress.fqdn" `
  -o tsv

$ApiUrl = "https://$ApiHostName"

Write-Host ""
Write-Host "Deployment complete."
Write-Host "Image: $Image"
Write-Host "API:   $ApiUrl"
Write-Host ""

# -----------------------------
# Test health endpoint
# -----------------------------

Write-Host "Testing /health..."

try {
    $Response = Invoke-WebRequest "$ApiUrl/health" -UseBasicParsing

    Write-Host "Health check passed."
    Write-Host "Status: $($Response.StatusCode)"
    Write-Host "Body:   $($Response.Content)"
}
catch {
    Write-Host "Health check failed."
    Write-Host "This is expected if /health has not been added yet."
}