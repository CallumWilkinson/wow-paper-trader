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
# Move to project folder
# -----------------------------

Set-Location $ProjectRoot

# -----------------------------
# Check Azure login
# -----------------------------

Write-Host "Checking Azure login..."

az account show 1>$null 2>$null

if ($LASTEXITCODE -ne 0) {
    Write-Host "You are not logged in to Azure. Logging in now..."
    az login
}

# -----------------------------
# Create image tag
# -----------------------------

$Timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$ImageTag = "dev-$Timestamp"
$Image = "${ImageName}:${ImageTag}"

Write-Host "Deploying image:"
Write-Host $Image

# -----------------------------
# Build Docker image
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
# Push Docker image
# -----------------------------

Write-Host "Pushing Docker image to GHCR..."

docker push $Image

if ($LASTEXITCODE -ne 0) {
    Write-Host "Docker push failed. You may not be logged in to GHCR."
    Write-Host "Log in using your GitHub username and a classic PAT with write:packages and read:packages."

    $GitHubUsername = Read-Host "GitHub username"
    $GitHubToken = Read-Host "GitHub classic PAT" -AsSecureString

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

    Write-Host "Trying Docker push again..."

    docker push $Image

    if ($LASTEXITCODE -ne 0) {
        throw "Docker push failed again."
    }
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
# Print API URL
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
    Write-Host "Health check passed:"
    Write-Host $Response.Content
}
catch {
    Write-Host "Health check failed."
    Write-Host "This is okay if you have not added /health yet."
}