---
external help file: PoshProject.dll-Help.xml
Module Name: PoshProject
online version:
schema: 2.0.0
---

# Invoke-PoshTemplate

## SYNOPSIS
`Invoke-PoshTemplate` cmdlet creates the project folders and files defined in the `PoshTemplate` generated by `New-PoshTemplate` cmdlet.

## SYNTAX

### Path (Default)
```
Invoke-PoshTemplate [-TemplatePath] <String> [-InstallDependencies] [<CommonParameters>]
```

### Object
```
Invoke-PoshTemplate -TemplateObject <PoshTemplate> [-InstallDependencies] [<CommonParameters>]
```

### Custom
```
Invoke-PoshTemplate -TemplateObject <PoshTemplate> [-CustomInstall] [<CommonParameters>]
```

## DESCRIPTION
`Invoke-PoshTemplate` cmdlet creates the project folders and files defined in the `PoshProjectTemplate.xml` created by `New-PoshTemplate` cmdlet. Additionaly you can also 
pass the template object from `Get-PoshTemplate` for project scaffolding.

Install dependencies when you are working in the project by passing the template object and specifying `CustomInstall` switch.

## EXAMPLES

### Example 1
```powershell
PS C:\> Invoke-PoshTemplate -TemplatePath .\PoshProjectTemplate.xml
[+] Creating Project
[+] Creating Project Directory
[+] Creating Root Module
[+] Creating Module Manifest
[+] Creating Pester Tests File
```

### Example 2
```powershell
PS C:\> Invoke-PoshTemplate -TemplateObject (Get-PoshTemplate -TemplatePath .\PoshProjectTemplate.xml)
[+] Creating Project
[+] Creating Project Directory
[+] Creating Root Module
[+] Creating Module Manifest
[+] Creating Pester Tests File
```

### Example 3
```powershell
PS C:\> ".\PoshProjectTemplate.xml" | Get-PoshTemplate | Invoke-PoshTemplate
[+] Creating Project
[+] Creating Project Directory
[+] Creating Root Module
[+] Creating Module Manifest
[+] Creating Pester Tests File
```

### Example 4
```powershell
PS C:\> $template = Get-PoshTemplate ".\PoshProjectTemplate.xml"
# This is required as dependencies is of type string. If not specified then the modules will be considered as a single name or entry
PS C:\> $template.Dependencies = @("Az.Accounts", "Az.KeyVault") -join ","
PS C:\> Invoke-PoshTemplate -TemplateObject $template -CustomInstall
[+] Installing Az.Accounts
[+] Successfully installed: Az.Accounts
[+] Installing Az.KeyVault
[+] Successfully installed: Az.KeyVault
```

Run `Test-PoshTemplate` to validate the template before invoking this function.

## PARAMETERS

### -CustomInstall
Specify this parameter and pass the template object from `Get-PoshTemplate` cmdlet to install the dependencies.

```yaml
Type: SwitchParameter
Parameter Sets: Custom
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InstallDependencies
Pass the template or template object to install the dependencies.

```yaml
Type: SwitchParameter
Parameter Sets: Path, Object
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TemplateObject
Pass the template object from `Get-PoshTemplate` cmdlet.

```yaml
Type: PoshTemplate
Parameter Sets: Object
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

```yaml
Type: PoshTemplate
Parameter Sets: Custom
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -TemplatePath
Pass the template file to invoke the project folder structure.

```yaml
Type: String
Parameter Sets: Path
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

### PoshProject.PoshTemplate

## OUTPUTS

### System.Object
## NOTES

Note that the template name should be `PoshProjectTemplate.xml` otherwise you will receive an error.

## RELATED LINKS

[Invoke-PoshTemplate](https://github.com/IndividualsinDemand/PoshProject/blob/master/docs/Invoke-PoshTemplate.md)