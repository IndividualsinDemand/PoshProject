using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsDiagnostic.Test, "PoshTemplate")]
    [OutputType(typeof(PoshTemplate))]
    public class TestPoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string TemplatePath { get; set; }

        protected override void ProcessRecord()
        {
            if ((File.Exists(TemplatePath)))
            {
                if (ProjectTemplate.TestTemplate(TemplatePath))
                {
                    ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("info"), "Valid Template");
                }

                else
                {
                    ProjectTemplate.InvalidTemplate();
                }
            }

            else
            {
                ProjectTemplate.FileNotFound();
            }
        }
    }        
}
