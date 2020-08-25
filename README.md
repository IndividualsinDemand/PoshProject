# POSHPROJECT

**PoshProject** is a simple PowerShell project scaffolding module which is inspired from `Plaster`. It works on a predefined xml contents and layouts
the project structure as defined in the template.

## Table of Contents

- Getting Started
- Examples
- Notes

### Getting Started

**PoshProject** can be installed from [PowerShellGallery](https://www.powershellgallery.com/packages/PoshProject/0.1.0) if you are using PowerShell version 5.0 and above.

```powershell
PS C:\> Install-Module -Name PoshProject -Scope CurrentUser -Force
```

### Examples

**PoshProject** has only 4 cmdlets as of the initial version `0.1.0`.

```powershell
PS C:\> Get-Command -Module PoshProject

CommandType     Name                                               Version    Source
-----------     ----                                               -------    ------
Cmdlet          Get-PoshTemplate                                   0.1.0      PoshProject
Cmdlet          Invoke-PoshTemplate                                0.1.0      PoshProject
Cmdlet          New-PoshTemplate                                   0.1.0      PoshProject
Cmdlet          Test-PoshTemplate                                  0.1.0      PoshProject
```

```powershell
# Create a template for your project
PS C:\> New-PoshTemplate -ProjectName "Azure-Health-Check" -ProjectType Module -License MIT -DependsOn ("Az.Accounts", "Az.KeyVault")

# Get the template object
PS C:\> Get-PoshTemplate ".\Azure-Health-Check.xml"
ProjectName  : Azure-Health-Check
Directories  : Classes,Private,Public,docs,en-US,Tests
Type         : Module
Dependencies : Az.Accounts,Az.KeyVault
License      : MIT
Metadata     : PoshProject.Metadata

# Test the template
PS C:\> Test-PoshTemplate ".\Azure-Health-Check.xml"
[+] Error Count: 0
[+] Valid Template

# Create the project structure
PS C:\> Invoke-PoshTemplate ".\Azure-Health-Check.xml"
[+] Creating Project
[+] Creating Project Directory
[+] Creating Module Manifest
[+] Creating Root Module
[+] Creating Project Directories
[+] Creating Classes
[+] Creating Private
[+] Creating Public
[+] Creating docs
[+] Creating en-US
[+] Creating Tests
[+] Adding License
[+] Installing Dependencies
[+] Installing Az.Accounts
[+] Successfully installed: Az.Accounts
[+] Installing Az.KeyVault
[+] Successfully installed: Az.KeyVault
```

### Notes

This is a simple module which allows you to create the project structure and yet useful. Any updated or suggestions are welcome.