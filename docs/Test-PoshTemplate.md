---
external help file: PoshProject.dll-Help.xml
Module Name: PoshProject
online version:
schema: 2.0.0
---

# Test-PoshTemplate

## SYNOPSIS
`Test-PoshTemplate` cmdlet is intended to test the PoshTemplate file.

## SYNTAX

```
Test-PoshTemplate [-TemplatePath] <String> [<CommonParameters>]
```

## DESCRIPTION
`Test-PoshTemplate` cmdlet helps you to test the PoshTemplate file and reports if the template is valid or not. An invalid template error will be thrown 
if the `Guid` in the template is invalid.

## EXAMPLES

### Example 1
```powershell
PS C:\> Test-PoshTemplate -TemplatePath .\MyModule.xml
[+] Error Count: 0
[+] Valid Template
```

### Example 2
```powershell
PS C:\> "\MyNewModule.xml" | Test-PoshTemplate
[-] <ProjectName> is empty
[-] <Directories> are empty
[-] <Type> is empty
[-] Invalid path: 'C:\MyNewModule.ps1'
[-] Invalid root module name: 'MyNewModule.psm'
[-] Error Count: 5
[-] Template validation failed
```

If there is any empty field except `Dependencies` this cmdlet will fail the validation. It checks for each tag in the xml and validates `Guid` too. If `Guid` is found
to be invalid, an **Invalid Template** error will be thrown.

## PARAMETERS

### -TemplatePath
Provide the path to template file.

```yaml
Type: String
Parameter Sets: (All)
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

## OUTPUTS

### PoshProject.PoshTemplate

## NOTES

## RELATED LINKS
[Get-PoshTemplate](https://github.com/IndividualsinDemand/PoshProject/blob/master/docs/Get-PoshTemplate.md)