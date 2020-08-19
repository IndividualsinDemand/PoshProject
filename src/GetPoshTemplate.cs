using System;
using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsCommon.Get, "PoshTemplate")]
    [OutputType(typeof(PoshProjectTemplate))]
    public class GetPoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string FilePath { get; set; }

        protected override void ProcessRecord()
        {
            PoshProjectTemplate template = new PoshProjectTemplate();
            WriteObject(template);
        }
    }

    public class PoshProjectTemplate
    {
        public string ProjectName { get; set; } = "PoshProjectTemplate";
        public string FilePath { get; set; } = Directory.GetCurrentDirectory() + "\\PoshProjectTemplate.xml";
        public string Author { get; set; } = Environment.UserName;
        public string[] Directories { get; set; } = new string[] { "Classes", "Private", "Public", "docs", "en-US", "Tests"  };
        public string Description { get; set; } = "PowerShell project template creation module";
        public Guid Id { get; set; } = Guid.NewGuid();
        public string[] Tags { get; set; } = new string[] { "PowerShell", "Project", "ProjectTemplate", "PowerShellTemplate", "PSProject", "PoshProject" };
        public string Version { get; set; } = "0.1.0";

    }
}