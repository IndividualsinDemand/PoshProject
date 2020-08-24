using System;
using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsLifecycle.Invoke, "PoshTemplate", DefaultParameterSetName = "Path")]
    public class InvokePoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Path")]
        [ValidateNotNullOrEmpty()]
        public string TemplatePath { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Object")]
        [ValidateNotNullOrEmpty()]
        public PoshTemplate TemplateObject { get; set; }

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
                    PoshTemplate template = ProjectTemplate.GetTemplate(TemplatePath);
                    
                    // Creating Project
                    var projectPath = template.Metadata.Path.Replace(".psd1", "");

                    // Creating Project Directory
                    Directory.CreateDirectory(projectPath);

                    // Creating module file
                    File.Create($"{projectPath}\\{template.Metadata.RootModule}");

                    // Creating manifest
                    ProjectTemplate.CreateManifest(template, projectPath);

                    if (template.Type == "Script")
                    {
                        // Creating Directories (in this case it will be a .tests file)
                        File.Create($"{projectPath}\\{template.Directories}");
                    }

                    else
                    {
                        // Creating Directories
                        string[] directories = template.Directories.Split(',');

                        foreach (string dir in directories)
                        {
                            Directory.CreateDirectory($"{projectPath}\\{dir}");
                        }
                    }
                }
            }

            else
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

                    // Creating manifest
                    ProjectTemplate.CreateManifest(TemplateObject, projectPath);

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
        }
    }
}
