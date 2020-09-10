Set-StrictMode -Version Latest

Describe "PoshProject" {
    BeforeAll {
        # Import module
        $root = Split-Path (Split-Path $PSCommandPath)
        $moduleName = (Get-ProjectName)
        Import-Module "$root\$moduleName"

        $results = [PSCustomObject]@{
            "ProjectName"  = "TestProject"
            "Directories"  = "dir,dir1,dir2"
            "Type"         = "Module"
            "Dependencies" = ""
            "License"      = "MIT"
            "Metadata"     = "PoshProject.Metadata"
        }
    }

    BeforeEach {
        $Path = "C:\TEMP"
        $Template = "PoshProjectTemplate.xml"
        $TemplatePath = "$Path\$Template"
        $ProjectName = "TestProject"

        if (!(Test-Path $Path)) {
            New-Item -Path $Path -ItemType Directory > $null
        }

        #find and remove template and create new one.
        New-PoshTemplate `
            -ProjectName $ProjectName `
            -FilePath $TemplatePath `
            -ProjectType Module `
            -Author "Test" `
            -Directories @("dir", "dir1", "dir2") `
            -Description "This is a test project" `
            -Tags @("TestTag") `
            -License MIT
    }

    Context "Get-PoshTemplate" {
        It "Should be same as result" {
            Mock Get-PoshTemplate { return $results } -ParameterFilter { $TemplatePath -eq $TemplatePath }
        }

        It "Should be of type PSCustomObject" {
            Get-PoshTemplate -TemplatePath $TemplatePath | Should -BeOfType [PSCustomObject]
        }

        It "Should return null" {
            (Get-PoshTemplate -TemplatePath $TemplatePath).Dependencies | Should -Be ""
        }

        It "Should not be null" {            
            $temp = Get-PoshTemplate -TemplatePath $TemplatePath
            $temp.ProjectName = "TestProject"
            Assert ( $temp.ProjectName -eq "TestProject" ) "ProjectName name should be 'TestProject'"
        }
    }
}