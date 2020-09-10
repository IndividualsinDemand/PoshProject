#install psake
if (-not (Get-Module -Name "psake" -ListAvailable)) {
    Write-Output "Installing module [psake]"
    Install-Module -Name "psake" -SkipPublisherCheck -Scope CurrentUser -Force -Repository PSGallery -AllowClobber
}

Invoke-psake -buildFile .\build.ps1 -taskList Init,Build,Test -nologo