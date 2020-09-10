#install psake
$Modules = @("psake", "BuildHelpers")
$Modules | ForEach-Object {
    if (-not (Get-Module -Name $_ -ListAvailable)) {
        Write-Output "Installing module [$_]"
        Install-Module -Name $_ -SkipPublisherCheck -Scope CurrentUser -Force -Repository PSGallery -AllowClobber
    }
}

Import-Module $Modules -Verbose

Invoke-psake -buildFile .\build.ps1 -taskList Init,Build,Test -nologo