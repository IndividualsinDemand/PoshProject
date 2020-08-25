# PoshProject
## about_PoshProject

# SHORT DESCRIPTION
**PoshProject** is a simple PowerShell scaffolding module inspired from `Plaster`.

# LONG DESCRIPTION
**PoshProject** is a simple PowerShell scaffolding module inspired from `Plaster`. It keeps a small predefined layout for the xml template with two major sections.
One is `Configuration` which holds the `Project Name`, `Directories` to be created and `Type` of project. Second is `Metadata` which holds the basic and mandatory
fields to create manifest file.

It allows you to create a template, get the template as object, test it and invoke it to create your desired project folder and file structure.

# EXAMPLES
```powershell
# Create a template for your project
PS C:\> New-PoshTemplate -ProjectName "Azure-Health-Check"

# Get the template object
PS C:\> Get-PoshTemplate ".\Azure-Health-Check.xml"
ProjectName  : Azure-Health-Check
Directories  : Azure-Health-Check.tests.ps1
Type         : Script
Dependencies :
Metadata     : PoshProject.Metadata

# Test the template
PS C:\> Test-PoshTemplate ".\Azure-Health-Check.xml"
[+] Error Count: 0
[+] Valid Template

# Create the project structure
PS C:\> Invoke-PoshTemplate ".\Azure-Health-Check.xml"
[+] Creating Project
[+] Creating Project Directory
[+] Creating Root Module
[+] Creating Module Manifest
[+] Creating Pester Tests File
```

# NOTE
Test the template before creating your project.

# TROUBLESHOOTING NOTE
If there is any change in `Guid` in template file you will receive invalid template error.
To overcome this generate new guid with the cmdlet `New-Guid` and add it in the template.

# SEE ALSO
Test-PoshTemplate

https://github.com/IndividualsinDemand/PoshProject

# KEYWORDS
List of cmdlets

- Get-PoshTemplate
- Test-PoshTemplate
- New-PoshTemplate
- Invoke-PoshTemplate
