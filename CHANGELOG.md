# 0.1.0

Initial module release

# 0.2.1

- Bug fix for path seperator.
- Added function to get the username from .gitconfig file.
- Change to `Invoke-PoshTemplate` cmdlet; Now the dependencies will be installed only after specifying `InstalledDependencies` switch.
- `DependsOn` parameter in `New-PoshTemplate` no longer accepts null value.
- Project template to use a consistent name so that a single template can be re-used.
- Now `Invoke-poshTemplate` throws error if the project already exists in the path.
- Added `CustomInstall` parameter to `Invoke-PoshTemplate` where you can pass the template object and install the dependencies.