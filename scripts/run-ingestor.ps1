param(
    [ValidateSet("auctions", "metadata")]
    [string]$Mode = "auctions"
)

$ErrorActionPreference = "Stop"
$exitCode = 1

$projectRoot = Split-Path -Parent $PSScriptRoot
$logDirectory = Join-Path $projectRoot "logs"
$logFile = Join-Path $logDirectory "ingestor-scheduler.log"

New-Item `
    -ItemType Directory `
    -Path $logDirectory `
    -Force |
        Out-Null

Push-Location $projectRoot

try {
    $startedAt = Get-Date

    # Ask Docker Compose to send its own status messages to stdout.
    $env:COMPOSE_STATUS_STDOUT = "1"

    "[$($startedAt.ToString('o'))] Starting ingestor mode '$Mode'." |
            Tee-Object -FilePath $logFile -Append

    # Windows PowerShell 5.1 may represent native stderr as ErrorRecord
    # objects. Do not let harmless stderr terminate this pipeline.
    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"

    try {
        docker compose run --rm ingestor $Mode 2>&1 |
                ForEach-Object {
                    $_.ToString()
                } |
                Tee-Object -FilePath $logFile -Append

        # Capture this immediately after Docker finishes.
        $exitCode = $LASTEXITCODE
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    $finishedAt = Get-Date

    "[$($finishedAt.ToString('o'))] Finished with exit code $exitCode." |
            Tee-Object -FilePath $logFile -Append
}
finally {
    Pop-Location
}

exit $exitCode