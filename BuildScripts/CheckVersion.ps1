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
cd $rootPath

$version = Get-Content -Path "$rootPath\version.txt" -Raw
$tagName = "v$version"

try {
    git rev-parse --verify --quiet "refs/tags/$tagName"
    if($LASTEXITCODE -eq 0) {
        Write-Host "Tag $tagName already exists"
        cd $oldCd
        exit 1
    } else {
        Write-Host "Tag $tagName does not exist"
        cd $oldCd
        exit 0
    }
} catch {
    Write-Error $_
    cd $oldCd
    exit 1
}
