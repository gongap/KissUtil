# apiKey
$apiKey = $args[0]

# Paths
$rootFolder = (Get-Item -Path "./" -Verbose).FullName
$packFolder = Join-Path $rootFolder "output"
$hasPath = Test-Path($packFolder)

 if (-Not $hasPath) {
      new-item -path $rootFolder -name "output" -type directory
}
Write-Host ("PackFolder: " + $packFolder)

# List of projects
$projects = (
    "src/KissUtil"
)

# Remove item
Set-Location $packFolder
Remove-Item (Join-Path $packFolder "*.nupkg")

# Rebuild solution
Set-Location $rootFolder
& dotnet restore

# Create all packages
foreach($project in $projects) {
    $projectFolder = Join-Path $rootFolder $project
    $i += 1
	$projectName = ($project -split '/')[-1]

    # Create nuget pack
    Write-Host ("-----===[ $i / " + $projects.count  + " - " + $projectName + " ]===-----")
    Set-Location $projectFolder
    & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true

    if (-Not $?) {
        Write-Host ("Packaging failed for the project: " + $projectFolder)
        exit $LASTEXITCODE
    }
    
    # Copy nuget package
    $projectName = $project.Substring($project.LastIndexOf("/") + 1)
    $projectPackPath = Join-Path $rootFolder ("/output/release/" + $projectName + ".*.nupkg")
    Move-Item $projectPackPath $packFolder
}

# Go back to the pack folder
Set-Location $packFolder

# Publish all packages
$projectPacks = Get-ChildItem  (Join-Path $packFolder "*.nupkg")
foreach($pack in $projectPacks) {
    & dotnet nuget push ($pack) -s https://nexus.sycdev.com/repository/nuget-hosted/ --skip-duplicate --api-key "$apiKey"
}

# Go back to the root folder
Set-Location $rootFolder
