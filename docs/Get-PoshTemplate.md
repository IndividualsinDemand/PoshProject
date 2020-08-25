---
external help file: PoshProject.dll-Help.xml
Module Name: PoshProject
online version:
schema: 2.0.0
---

# Get-PoshTemplate

## SYNOPSIS
`Get-PoshTemplate` cmdlet returns the object of `PoshTemplate` structure that can be used with `Invoke-PoshTemplate` cmdlet.

## SYNTAX

```
Get-PoshTemplate [[-TemplatePath] <String>] [<CommonParameters>]
```

## DESCRIPTION
`Get-PoshTemplate` cmdlet returns the object of PoshTemplate structure that can be used with `Invoke-PoshTemplate` cmdlet. It returns a default template object if template path is not provided.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-PoshTemplate
ProjectName  : PoshProjectTemplate
Directories  : PoshProjectTemplate.tests.ps1
Type         : Script
Dependencies :
Metadata     : PoshProject.Metadata
```

### Example 2
```powershell
PS C:\> ".\MyModule.xml" | Get-PoshTemplate
ProjectName  : MyModule
Directories  : MyModule.tests.ps1
Type         : Script
Dependencies : psAzD,PSDB
Metadata     : PoshProject.Metadata
```

Template file which is created with the `New-PoshTemplate` can be passed to this cmdlet. 

## PARAMETERS

### -TemplatePath
Pass the PoshTemplate file created with `New-PoshTemplate` cmdlet.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
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
[Test-PoshTemplate](https://github.com/IndividualsinDemand/PoshProject/blob/master/docs/Test-PoshTemplate.md)