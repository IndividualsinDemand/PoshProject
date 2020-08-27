---
external help file: PoshProject.dll-Help.xml
Module Name: PoshProject
online version:
schema: 2.0.0
---

# New-PoshTemplate

## SYNOPSIS
`New-PoshTemplate` cmdlet creates an xml template file for passed configuration or for a project name. 

## SYNTAX

```
New-PoshTemplate [-ProjectName] <String> [[-FilePath] <String>] [-ProjectType <String>] [-Author <String>]
 [-Directories <String[]>] [-Description <String>] [-Tags <String[]>] [-Guid <Guid>] [-Version <String>]
 [-DependsOn <String[]>] [-License <String>] [<CommonParameters>]
```

## DESCRIPTION
`New-PoshTemplate` cmdlet creates an xml template file for passed configuration or for a project name. This cmdlet helps you to define 
your project name, PowerShell manifest file mandatory details and custom directories for your project. It allows three types of project types,
`Script`, `Module` and `Binary`. All the defined files and folders are created based on this.

You can use this for creating any folder structure and you have to specify `Module` to create the folders. By default it will create a set of predefined
folders and files for all these types.

The template name should be `PoshProjectTemplate.xml` and any change to this name will return error. 

## EXAMPLES

### Example 1
```powershell
PS C:\> "MyNewModule" | New-PoshTemplate
```

Creates a PoshTemplate file populated with project settings.

## PARAMETERS

### -Author
Module author name. This will be used to created module manifest file.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DependsOn
Project dependencies. Provide the external module names that your project depends on.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Description
Provide the description of your project.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Directories
Array of directories to be created as a part of the project. By default it will create a predefined folders and files based on the Project Type.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FilePath
Provide the path to create template file. If not provided the template file will be created in present working directory.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Guid
Guid of the project/module.

```yaml
Type: Guid
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -License
Type of License.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: MIT, Apache

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ProjectName
Name of your project.

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

### -ProjectType
Type of the project.

```yaml
Type: String
Parameter Sets: (All)
Aliases:
Accepted values: Script, Module, Binary

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tags
Define tags to identify your project.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Version
Provide the version number for your module.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.Object
## NOTES

Note that the template name should be `PoshProjectTemplate.xml` otherwise you will receive an error.

## RELATED LINKS

[Invoke-PoshTemplate](https://github.com/IndividualsinDemand/PoshProject/blob/master/docs/Invoke-PoshTemplate.md)