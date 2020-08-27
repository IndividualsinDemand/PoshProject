using System;
using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsCommon.New, "PoshTemplate")]
    public class NewPoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string ProjectName { get; set; }

        [Parameter(Mandatory = false, Position = 1, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string FilePath { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        [ValidateSet("Script", "Module", "Binary")]
        public string ProjectType { get; set; } = "Script";

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public string Author { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public string[] Directories { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public string Description { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public string[] Tags { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public Guid Guid { get; set; } = Guid.NewGuid();

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public string Version { get; set; } = "0.1.0";

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        public string[] DependsOn { get; set; } = new string[] { null };

        [Parameter(Mandatory = false)]
        [ValidateNotNullOrEmpty()]
        [ValidateSet("MIT", "Apache")]
        public string License { get; set; } = null;

        protected override void ProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey("FilePath") & (!ProjectTemplate.TemplateNameValidator(FilePath)))
            {
                ProjectTemplate.InvalidFileName();
            }

            else
            {

                if (!(MyInvocation.BoundParameters.ContainsKey("FilePath")))
                {
                    FilePath = Directory.GetCurrentDirectory() + $"\\PoshProjectTemplate.xml";
                }

                if (!(MyInvocation.BoundParameters.ContainsKey("Author")))
                {
                    if (ProjectTemplate.GetUserName() != null)
                    {
                        Author = ProjectTemplate.GetUserName();
                    }

                    else
                    {
                        Author = Environment.UserName;
                    }

                }

                if (!(MyInvocation.BoundParameters.ContainsKey("Directories")))
                {
                    if (ProjectType == "Script")
                    {
                        Directories = new string[] { $"{ProjectName}.tests.ps1" };
                    }

                    else if (ProjectType == "Module")
                    {
                        Directories = new string[]
                        {
                            "Classes", "Private", "Public", "docs", "en-US", "Tests"
                        };
                    }

                    else
                    {
                        Directories = new string[]
                        {
                            "Output", ProjectName, "src", "docs", "en-US", "Tests"
                        };
                    }

                }

                if (!(MyInvocation.BoundParameters.ContainsKey("Description")))
                {
                    Description = $"Module for {ProjectName}";
                }

                if (!(MyInvocation.BoundParameters.ContainsKey("Tags")))
                {
                    Tags = new string[]
                    {
                        "PowerShell", $"{ProjectName}Module", ProjectName
                    };
                }

                ProjectTemplate.NewTemplate(ProjectName, FilePath, ProjectType, Author, Directories, Description, Guid.ToString(), Tags, Version, DependsOn, License);
            }
        }
    }
}