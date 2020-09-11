Set-StrictMode -Version Latest

Describe "PoshProject" {
    BeforeAll {
        # Import module
        $root = Split-Path (Split-Path $PSCommandPath)
        $moduleName = (Get-ProjectName)
        Import-Module "$root\$moduleName"
    }

    BeforeEach {
        $Path = "C:\TEMP"
        $Template = "PoshProjectTemplate.xml"
        $TemplatePath = "$Path\$Template"
        $ProjectName = "TestProject"

        if (!(Test-Path $Path)) {
            New-Item -Path $Path -ItemType Directory > $null
        }
    }

    Context "Invoke-PoshTemplate" {
        It "Should Create a new project from template file" {
            Invoke-PoshTemplate -TemplatePath $TemplatePath
            "$Path\$ProjectName" | Should -Exist
        }
    }
}