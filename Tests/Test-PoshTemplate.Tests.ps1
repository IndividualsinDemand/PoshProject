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

    Context "Test-PoshTemplate" {
        It "Should return null" {
            Test-PoshTemplate -TemplatePath $TemplatePath | Should -Be $null
        }
    }
}