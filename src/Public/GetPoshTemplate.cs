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
        public string TemplatePath { get; set; } = Directory.GetCurrentDirectory() + "\\PoshProjectTemplate.xml";

        protected override void ProcessRecord()
        {
            if (!(File.Exists(TemplatePath)))
            {
                ProjectTemplate.FileNotFound();
            }

            else
            {
                if (!ProjectTemplate.TemplateNameValidator(TemplatePath))
                {
                    ProjectTemplate.InvalidFileName();
                }

                else
                {
                    if (ProjectTemplate.TestTemplate(TemplatePath))
                    {
                        WriteObject(ProjectTemplate.GetTemplate(TemplatePath));
                    }

                    else
                    {
                        ProjectTemplate.InvalidTemplate();
                    }
                }
            }
        }
    }
}