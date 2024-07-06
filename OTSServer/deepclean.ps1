# Define the root directory where the search should begin
$rootDirectory = "./"

# Get all directories named 'bin' within the root directory and its subdirectories
$binDirectories = Get-ChildItem -Path $rootDirectory -Recurse -Directory -Filter "bin"

# Loop through each 'bin' directory and remove it
foreach ($binDir in $binDirectories) {
    try {
        Remove-Item -Path $binDir.FullName -Recurse -Force
        Write-Host "Deleted: $($binDir.FullName)"
    } catch {
        Write-Host "Failed to delete: $($binDir.FullName) - $_"
    }
}

Write-Host "Completed deleting all 'bin' directories."
