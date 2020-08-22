using System;
using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsCommon.Get, "PoshTemplate")]
    [OutputType(typeof(PoshTemplate))]
    public class GetPoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string TemplatePath { get; set; }

        protected override void ProcessRecord()
        {
            if (!(MyInvocation.BoundParameters.ContainsKey("TemplatePath")))
            {
                PoshTemplate poshTemplate = new PoshTemplate
                {
                    ProjectName = "PoshProjectTemplate",
                    Directories = "PoshProjectTemplate.tests.ps1",
                    Type = "Script",
                    Dependencies = ""
                };

                Metadata metadata = new Metadata()
                {
                    Author = Environment.UserName,
                    RootModule = "PoshProjectTemplate.psm1",
                    Tags = string.Join(",", "PoshProject", "PoshTemplate", "PoshProjectTemplate"),
                    Description = "A simple project scaffolding module for PowerShell",
                    Guid = Guid.NewGuid(),
                    ModuleVersion = "0.1.0",
                    Path = Directory.GetCurrentDirectory() + "\\PoshProjectTemplate.psd1"
                };

                poshTemplate.Metadata = metadata;

                WriteObject(poshTemplate);
            }

            else
            {
                if (!(File.Exists(TemplatePath)))
                {
                    ProjectTemplate.HandleFileNotFoundException();
                }

                else
                {
                    if (ProjectTemplate.TestTemplate(TemplatePath))
                    {
                        WriteObject(ProjectTemplate.GetTemplate(TemplatePath));
                    }

                    else
                    {
                        ProjectTemplate.HandleFileLoadException();
                    }
                    
                }                
            }
        }
    }
}