# Script: Replace-Text.ps1
param (
    [Parameter(Mandatory = $true)]
    [string]$FilePath, # Path to the file

    [Parameter(Mandatory = $true)]
    [string]$OldText,  # Text to be replaced

    [Parameter(Mandatory = $true)]
    [string]$NewText   # Replacement text
)

# Validate the file path
if (!(Test-Path $FilePath)) {
    Write-Error "The file '$FilePath' does not exist."
    exit 1
}

try {
    # Read the content of the file
    $content = Get-Content -Path $FilePath -Raw

    # Replace the old text with the new text
    $updatedContent = $content -replace [regex]::Escape($OldText), $NewText

    # Write the updated content back to the file
    Set-Content -Path $FilePath -Value $updatedContent

    Write-Output "Text replaced successfully in '$FilePath'."
} catch {
    Write-Error "An error occurred: $_"
    exit 1
}
