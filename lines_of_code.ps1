$files = Get-ChildItem -Path . -Recurse -File -Filter *.cs |
Where-Object {
    $_.FullName -notmatch '\\bin\\' -and
    $_.FullName -notmatch '\\obj\\' -and
    $_.Name -notmatch '\.g\.cs$' -and
    $_.Name -notmatch '\.designer\.cs$' -and
    $_.Name -notmatch 'AssemblyInfo\.cs$'
}

$totalLines = 0
$testLines = 0
$prodLines = 0

foreach ($file in $files)
{
    $lineCount = (Get-Content -Path $file.FullName | Measure-Object -Line).Lines
    $totalLines += $lineCount

    $isTestFile = $file.FullName -match '\\[^\\]+\.Tests\\'

    if ($isTestFile)
    {
        $testLines += $lineCount
    }
    else
    {
        $prodLines += $lineCount
    }
}

Write-Host "Total C# lines: $totalLines"
Write-Host "Test lines: $testLines"
Write-Host "Production lines: $prodLines"