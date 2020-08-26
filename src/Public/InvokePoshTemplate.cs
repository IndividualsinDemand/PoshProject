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
                        if (ProjectTemplate.ValidateTemplate(TemplatePath) == 0)
                        {
                            PoshTemplate template = ProjectTemplate.GetTemplate(TemplatePath);
                            ProjectTemplate.CreateProject(template);
                        }

                        else
                        {
                            ProjectTemplate.InvalidTemplate();
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Run Test-PoshTemplate cmdlet to validate the template");
                        }                        
                    }
                    
                    else
                    {
                        ProjectTemplate.InvalidTemplate();
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Run Test-PoshTemplate cmdlet to validate the template");
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
                    if (ProjectTemplate.ValidateTemplateObject(TemplateObject) == 0)
                    {
                        ProjectTemplate.CreateProject(TemplateObject);
                    }

                    else
                    {
                        ProjectTemplate.InvalidTemplate();
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Run Test-PoshTemplate cmdlet to validate the template");
                    }
                }
            }
        }
    }
}
