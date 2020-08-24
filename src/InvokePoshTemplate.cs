using System;
using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsLifecycle.Invoke, "PoshTemplate")]
    public class InvokePoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Path")]
        [ValidateNotNullOrEmpty()]
        public string TemplatePath { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Object")]
        [ValidateNotNullOrEmpty()]
        public PoshTemplate poshTemplate { get; set; }
        protected override void ProcessRecord()
        {
            if (ParameterSetName == "Path")
            {
                if (!(File.Exists(TemplatePath)))
                {
                    ProjectTemplate.HandleFileNotFoundException();
                }

                else
                {
                    PoshTemplate poshTemplate = ProjectTemplate.GetTemplate(TemplatePath);

                    // Creating Project
                    var projectPath = poshTemplate.Metadata.Path.Replace(".psd1", "");

                    // Creating Project Directory
                    Directory.CreateDirectory(projectPath);

                    // Creating module file
                    File.Create($"{projectPath}\\{poshTemplate.Metadata.RootModule}");

                    // Creating Module Manifest
                    PowerShell ps = PowerShell.Create().AddCommand("New-ModuleManifest")
                                                       .AddParameter("Author", poshTemplate.Metadata.Author)
                                                       .AddParameter("Description", poshTemplate.Metadata.Description)
                                                       .AddParameter("Guid", poshTemplate.Metadata.Guid)
                                                       .AddParameter("ModuleVersion", poshTemplate.Metadata.ModuleVersion)
                                                       .AddParameter("Path", $"{projectPath}\\{poshTemplate.ProjectName}.psd1")
                                                       .AddParameter("Tags", poshTemplate.Metadata.Tags.Split(','))
                                                       .AddParameter("RootModule", poshTemplate.Metadata.RootModule);

                    ps.Invoke();

                    if (poshTemplate.Type == "Script")
                    {
                        // Creating Directories (in this case it will be a .tests file)
                        File.Create($"{projectPath}\\{poshTemplate.Directories}");
                    }

                    else
                    {
                        // Creating Directories
                        string[] directories = poshTemplate.Directories.Split(',');

                        foreach (string dir in directories)
                        {
                            Directory.CreateDirectory($"{projectPath}\\{dir}");
                        }
                    }
                }
            }

            else if (ParameterSetName == "Object")
            {
                if (poshTemplate.GetType() != typeof(PoshTemplate))
                {
                    throw new TypeLoadException("Invalid template object; Run Get-PoshTemplate to obtain the correct type and re-run the cmdlet again");
                }

                else
                {
                    // Creating Project
                    var projectPath = poshTemplate.Metadata.Path.Replace(".psd1", "");

                    // Creating Project Directory
                    Directory.CreateDirectory(projectPath);

                    // Creating module file
                    File.Create($"{projectPath}\\{poshTemplate.Metadata.RootModule}");

                    // Creating Module Manifest
                    PowerShell ps = PowerShell.Create().AddCommand("New-ModuleManifest")
                                                       .AddParameter("Author", poshTemplate.Metadata.Author)
                                                       .AddParameter("Description", poshTemplate.Metadata.Description)
                                                       .AddParameter("Guid", poshTemplate.Metadata.Guid)
                                                       .AddParameter("ModuleVersion", poshTemplate.Metadata.ModuleVersion)
                                                       .AddParameter("Path", $"{projectPath}\\{poshTemplate.ProjectName}.psd1")
                                                       .AddParameter("Tags", poshTemplate.Metadata.Tags.Split(','))
                                                       .AddParameter("RootModule", poshTemplate.Metadata.RootModule);

                    ps.Invoke();

                    if (poshTemplate.Type == "Script")
                    {
                        // Creating Directories (in this case it will be a .tests file)
                        File.Create($"{projectPath}\\{poshTemplate.Directories}");
                    }

                    else
                    {
                        // Creating Directories
                        string[] directories = poshTemplate.Directories.Split(',');

                        foreach (string dir in directories)
                        {
                            Directory.CreateDirectory($"{projectPath}\\{dir}");
                        }
                    }
                }
            }

            else
            {
                WriteObject("Pass the template file or template object to create the project!!");
            }
        }
    }
}
