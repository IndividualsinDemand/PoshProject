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
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string TemplatePath { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 1,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public PoshTemplate TemplateObject { get; set; }
        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey("TemplatePath"))
            {
                if (!(File.Exists(TemplatePath)))
                {
                    ProjectTemplate.HandleFileNotFoundException();
                }

                else
                {
                    // Creating Project
                    var projectPath = TemplateObject.Metadata.Path.Replace(".psd1", "");

                    // Creating Project Directory
                    Directory.CreateDirectory(projectPath);

                    // Creating module file
                    File.Create($"{projectPath}\\{TemplateObject.Metadata.RootModule}");

                    // Creating Module Manifest
                    PowerShell ps = PowerShell.Create().AddCommand("New-ModuleManifest")
                                                       .AddParameter("Author", TemplateObject.Metadata.Author)
                                                       .AddParameter("Description", TemplateObject.Metadata.Description)
                                                       .AddParameter("Guid", TemplateObject.Metadata.Guid)
                                                       .AddParameter("ModuleVersion", TemplateObject.Metadata.ModuleVersion)
                                                       .AddParameter("Path", $"{projectPath}\\{TemplateObject.ProjectName}.psd1")
                                                       .AddParameter("Tags", TemplateObject.Metadata.Tags.Split(','))
                                                       .AddParameter("RootModule", TemplateObject.Metadata.RootModule);

                    ps.Invoke();

                    if (TemplateObject.Type == "Script")
                    {
                        // Creating Directories (in this case it will be a .tests file)
                        File.Create($"{projectPath}\\{TemplateObject.Directories}");
                    }

                    else
                    {
                        // Creating Directories
                        string[] directories = TemplateObject.Directories.Split(',');

                        foreach (string dir in directories)
                        {
                            Directory.CreateDirectory($"{projectPath}\\{dir}");
                        }
                    }
                }
            }

            else if (MyInvocation.BoundParameters.ContainsKey("TemplateObject"))
            {
                if (TemplateObject.GetType() != typeof(PoshTemplate))
                {
                    throw new TypeLoadException("Invalid template object; Run Get-PoshTemplate to obtain the correct type and re-run the cmdlet again");
                }

                else
                {
                    // Creating Project
                    var projectPath = TemplateObject.Metadata.Path.Replace(".psd1", "");

                    // Creating Project Directory
                    Directory.CreateDirectory(projectPath);

                    // Creating module file
                    File.Create($"{projectPath}\\{TemplateObject.Metadata.RootModule}");

                    // Creating Module Manifest
                    PowerShell ps = PowerShell.Create().AddCommand("New-ModuleManifest")
                                                       .AddParameter("Author", TemplateObject.Metadata.Author)
                                                       .AddParameter("Description", TemplateObject.Metadata.Description)
                                                       .AddParameter("Guid", TemplateObject.Metadata.Guid)
                                                       .AddParameter("ModuleVersion", TemplateObject.Metadata.ModuleVersion)
                                                       .AddParameter("Path", $"{projectPath}\\{TemplateObject.ProjectName}.psd1")
                                                       .AddParameter("Tags", TemplateObject.Metadata.Tags.Split(','))
                                                       .AddParameter("RootModule", TemplateObject.Metadata.RootModule);

                    ps.Invoke();

                    if (TemplateObject.Type == "Script")
                    {
                        // Creating Directories (in this case it will be a .tests file)
                        File.Create($"{projectPath}\\{TemplateObject.Directories}");
                    }

                    else
                    {
                        // Creating Directories
                        string[] directories = TemplateObject.Directories.Split(',');

                        foreach (string dir in directories)
                        {
                            Directory.CreateDirectory($"{projectPath}\\{dir}");
                        }
                    }
                }
            }

            else
            {
                WriteWarning("Pass the template file or template object from Get-PoshTemplate cmdlet to create the project!!");
            }
        }
    }
}
