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

    Context "New-PoshTemplate" {
        It "Should create a new template" {
            New-PoshTemplate -ProjectName $ProjectName -FilePath $TemplatePath
            $TemplatePath | Should -Exist
        }

        It "Should create template  with default values" {
            New-PoshTemplate -ProjectName $ProjectName -FilePath $TemplatePath -ProjectType Module -DependsOn ("VSTeam", "Az.Accounts")
            $Directories = [string]::Join(",","Classes","Private","Public","docs","en-US","Tests")
            $t = Get-PoshTemplate -TemplatePath $TemplatePath
            Assert ( $t.Directories -eq $Directories ) "$Directories should exist in the template"
        }

        It "Should create template for passed values" {
            New-PoshTemplate `
                -ProjectName $ProjectName `
                -FilePath $TemplatePath `
                -ProjectType Module `
                -Author "Test" `
                -Directories @("dir", "dir1", "dir2") `
                -Description "This is a test project" `
                -Tags @("TestTag") `
                -License MIT
            
            $tem = Get-PoshTemplate -TemplatePath $TemplatePath
            $tem.Directories | Should -BeOfType [string]
        }

        It "Check author name" {            
            $temp = Get-PoshTemplate -TemplatePath $TemplatePath
            Assert ( $temp.Metadata.Author -eq "Test" ) "Author name should be 'Test'"
        }
    }
}