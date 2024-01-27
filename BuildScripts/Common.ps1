function VerifyPath( $path )
{
    if( !(Test-Path $path) )
    {
        Write-Host "Error: Cannot find $path"
        exit 1
    }
}

$rootPath = (New-Object IO.DirectoryInfo $PSScriptRoot).Parent.FullName