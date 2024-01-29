. $PSScriptRoot\Common.ps1

$version = Get-Content -Path "$rootPath\version.txt" -Raw
$tagName = "v$version"

$oldCd = pwd

cd $rootPath

Write-Host [Environment]::CurrentDirectory
Write-Host pwd

dotnet tool update -g vpk
dotnet publish -c Release --self-contained -r win-x64 -o $rootPath\WebpWhack\bin\publish
vpk pack -u WebpWhack -v $version -p $rootPath\WebpWhack\bin\publish -o $rootPath\WebpWhack\bin\Releases -e WebpWhack.exe

git tag $tagName
git push origin $tagName

cd $oldCd
