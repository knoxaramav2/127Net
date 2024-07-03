# Define the list of directories to search for .dll files
$directories = @(
    ".\OTSDeviceInfo\bin\release\net8.0",
    ".\OTSPluginMath\bin\release\net8.0",
	".\OTSStdProvider\bin\release\net8.0",
	".\OTSStdLogic\bin\release\net8.0",
    ".\OTSStdMonitor\bin\release\net8.0"
)

# Define the target directory where .dll files will be copied
$serverDirectory = "OTSServer\bin\release\plugins"
$testsDirectory = "OTSTests\bin\release\plugins"

# Create the target directory if it doesn't exist
if (-not (Test-Path -Path $serverDirectory)) {
    New-Item -ItemType Directory -Path $serverDirectory
}

if (-not (Test-Path -Path $testsDirectory)) {
    New-Item -ItemType Directory -Path $testsDirectory
}

# Iterate through each directory in the list
foreach ($directory in $directories) {
    # Get all .dll files in the current directory
	Write-Output "Copying $directory"
    $dllFiles = Get-ChildItem -Path $directory -Filter *.dll -Recurse

    # Copy each .dll file to the target directory
    foreach ($file in $dllFiles) {
        Copy-Item -Path $file.FullName -Destination $serverDirectory -Force
		Copy-Item -Path $file.FullName -Destination $testsDirectory -Force
    }
}

Write-Output "All plugin files have been copied"