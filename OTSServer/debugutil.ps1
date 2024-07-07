param([string]$command)

if (command -eq "watch"){
	dotnet watch -p OTSServer/OTSServer run
}