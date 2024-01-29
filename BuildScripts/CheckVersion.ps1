<#
.Synopsis
    Checks existing version
.Description
    Checks that there is no tag with the version from version.txt to make sure we updated the version before creating a release
.Example
    CheckVersion.ps1
#>

. $PSScriptRoot\Common.ps1

$oldCd = pwd
$oldEcd = [Environment]::CurrentDirectory

Write-Host $oldCd
Write-Host $([Environment]::CurrentDirectory)
Write-Host $([Environment]::CommandLine)

cd $rootPath
[Environment]::CurrentDirectory = $rootPath

Write-Host -----------

Write-Host $(pwd)
Write-Host $([Environment]::CurrentDirectory)

$version = Get-Content -Path "$rootPath\version.txt" -Raw
$tagName = "v$version"

try {
    git rev-parse --verify --quiet "refs/tags/$tagName"
    if($LASTEXITCODE -eq 0) {
        Write-Host "Tag $tagName already exists"
        cd $oldCd
        [Environment]::CurrentDirectory = $oldEcd
        exit 1
    } else {
        Write-Host "Tag $tagName does not exist"
        cd $oldCd
        [Environment]::CurrentDirectory = $oldEcd
        exit 0
    }
} catch {
    Write-Error $_
    cd $oldCd
    [Environment]::CurrentDirectory = $oldEcd
    exit 1
}
