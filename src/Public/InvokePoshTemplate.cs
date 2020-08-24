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
                    ProjectTemplate.FileNotFound();
                }

                else
                {
                    if (ProjectTemplate.TestTemplate(TemplatePath))
                    {
                        PoshTemplate template = ProjectTemplate.GetTemplate(TemplatePath);
                        ProjectTemplate.CreateProject(template);
                    }
                    
                    else
                    {
                        ProjectTemplate.InvalidTemplate();
                    }
                }
            }

            else
            {
                if (TemplateObject.GetType() != typeof(PoshTemplate))
                {
                    ProjectTemplate.InvalidTemplate();
                }

                else
                {
                    ProjectTemplate.CreateProject(TemplateObject);
                }
            }
        }
    }
}
