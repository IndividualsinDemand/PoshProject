# 0.2.2

- Bug fix for `Author` name in `New-PoshTemplate`. It wouldn't display `Author` name in the template if the .gitconfig file has more entries.
This is fixed in this version.
- Added functionality to validate the project type in the template
- Update for `Get-PoshTemplate` and `Test-PoshTemplate`, template path is not mandatory parameters as it seeks for the template in current location.
- Moved hardcoded template contents to individual files.
- Added logic to create README file.

# 0.2.1

- Bug fix for path seperator.
- Added function to get the username from .gitconfig file.
- Change to `Invoke-PoshTemplate` cmdlet; Now the dependencies will be installed only after specifying `InstalledDependencies` switch.
- `DependsOn` parameter in `New-PoshTemplate` no longer accepts null value.
- Project template to use a consistent name so that a single template can be re-used.
- Now `Invoke-poshTemplate` throws error if the project already exists in the path.
- Added `CustomInstall` parameter to `Invoke-PoshTemplate` where you can pass the template object and install the dependencies.

# 0.1.0

Initial module release