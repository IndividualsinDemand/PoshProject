[CmdletBinding()]
param (
    [Parameter(Mandatory = $false, Position = 0)]
    [string] $ModuleName = (Get-ProjectName),

    [Parameter(Mandatory = $false, Position = 1)]
    [ValidateSet("Major", "Minor", "Patch", "Build")]
    [string] $Version = "Patch"
)

$ProjectRoot = (Split-Path $PSCommandPath)

Task Init {
    #Install dependencies
    $Dependencies = @("platyPS", "Pester")
    $Dependencies | ForEach-Object {
        if (-not (Get-Module -ListAvailable $_)) {
            Write-Output "Installing module [$_]"
            Install-Module -Name $_ -SkipPublisherCheck -Scope CurrentUser -Force -Repository PSGallery -AllowClobber
        }
    }
}

Task UpdateManifest {
    # Import module and update cmlets to export
    Write-Output "Import $ProjectRoot\$ModuleName\bin\$($ModuleName).dll"
    Import-Module "$ProjectRoot\$ModuleName\bin\$($ModuleName).dll"
    Import-Module BuildHelpers
    
    # identify and update cmdlets
    Write-Output "Exporting cmdlets"
    $CmdletsToExport = (Get-Command -Module $ModuleName).Name

    Write-Output "Found: $($CmdletsToExport -join ", ")"
    Write-Output "Updating manifest file $ProjectRoot\src\$($ModuleName).psd1"
    Update-Metadata -Path $ProjectRoot\src\$($ModuleName).psd1 -PropertyName CmdletsToExport -Value $CmdletsToExport

    # Bump the version of the module
    Step-ModuleVersion -Path (Get-PSModuleManifest) -By $Version
}

Task UpdateMarkdownHelp {
    Write-Output "Updating Markdown help files"
    Update-MarkdownHelp -Path "$ProjectRoot\docs" | Out-Null
}

Task GenerateExternalHelp {
    Write-Output "Generating External help files"
    New-ExternalHelp -Path "$ProjectRoot\docs" -OutputPath "$ProjectRoot\en-US" -Force | Out-Null
}

Task Test {
    Write-Output "Running Pester tests"
    Invoke-Pester "$ProjectRoot\Tests" -OutputFormat NUnitXml -OutputFile "$ProjectRoot\Tests\result\$($ModuleName)Tests.xml" -Show All -WarningAction SilentlyContinue
}

Task Build {
    # Run dotnet-install only if you run in CI environment
    ./dotnet-install.ps1

    # build module
    Write-Output "Building module"
    if (!(Test-Path "$ProjectRoot\$ModuleName")) {
        New-Item $ProjectRoot\$ModuleName -ItemType Directory > $null
    }
    
    dotnet build -o "$ProjectRoot\$ModuleName\bin"

    Write-Output "Copying files to module folder"
    Write-Output "Copying: $ProjectRoot\src\$($ModuleName).psd1"
    Copy-Item -Path $ProjectRoot\src\$($ModuleName).psd1 -Destination "$ProjectRoot\$ModuleName" -Force

    Write-Output "Copying: $ProjectRoot\en-US"
    Copy-Item -Path "$ProjectRoot\en-US" -Destination "$ProjectRoot\$ModuleName" -Recurse -Force

    Write-Output "Copying: $ProjectRoot\src\Private\Contents"
    Copy-Item -Path "$ProjectRoot\src\Private\Contents" -Destination "$ProjectRoot\$ModuleName" -Recurse -Force

    #optional
    Copy-Item -Path "$ProjectRoot\LICENSE" -Destination "$ProjectRoot\$ModuleName" -Force
}

