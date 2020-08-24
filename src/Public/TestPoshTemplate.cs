using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;

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

        private int _errorCount { get; set; } = 0;

        protected override void ProcessRecord()
        {
            if ((File.Exists(TemplatePath)))
            {
                if (ProjectTemplate.TestTemplate(TemplatePath))
                {

                    var template = ProjectTemplate.GetTemplate(TemplatePath);
                    bool _isManifest = template.Metadata.Path.Contains(".psd1");

                    if (string.IsNullOrEmpty(template.ProjectName))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Project name is empty");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Directories))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Directories are empty");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Type))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Project Type is empty");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Metadata.Author))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Author name is empty");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Metadata.Description))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Description is empty");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Metadata.Path))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Path is empty");
                        _errorCount += 1;
                    }

                    if (! (_isManifest))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"Invalid path: {template.Metadata.Path}");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Metadata.Guid.ToString()))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Guid is empty");
                        _errorCount += 1;
                    }

                    if (! string.IsNullOrEmpty(template.Metadata.Guid.ToString()))
                    {
                        Regex regex = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                        if (!regex.IsMatch(template.Metadata.Guid.ToString()))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Invalid Guid");
                            _errorCount += 1;
                        }
                    }

                    if (string.IsNullOrEmpty(template.Metadata.ModuleVersion))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Module version is empty");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Metadata.RootModule))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Root Module is empty");
                        _errorCount += 1;
                    }

                    if (! template.Metadata.RootModule.Contains(".psm1"))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"Invalid root module name: {template.Metadata.RootModule}");
                        _errorCount += 1;
                    }

                    if (string.IsNullOrEmpty(template.Metadata.Tags))
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Tags are empty");
                        _errorCount += 1;
                    }

                    if (_errorCount == 0)
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("info"), $"Error Count: {_errorCount}");
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("info"), "Valid Template");
                    }
                    
                    else
                    {
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"Error Count: {_errorCount}");
                        ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"Template validation failed");
                    }
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
