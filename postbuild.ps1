# Define the source and destination paths
$sourcePath = Join-Path $PSScriptRoot "data"
$destinationPath = Join-Path "$env:ProgramData" "AutoAlteration"

# Create the destination directory if it doesn't exist
if (-not (Test-Path $destinationPath)) {
    New-Item -Path $destinationPath -ItemType Directory
}

# Copy the data folder to ProgramData
Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force
