﻿using System.IO;
using System.Management.Automation;

namespace PoshProject
{
    [Cmdlet(VerbsDiagnostic.Test, "PoshTemplate")]
    [OutputType(typeof(PoshTemplate))]
    public class TestPoshTemplate : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string TemplatePath { get; set; } = Directory.GetCurrentDirectory() + "\\PoshProjectTemplate.xml";

        private int _errorCount { get; set; } = 0;

        protected override void ProcessRecord()
        {
            if ((File.Exists(TemplatePath)))
            {
                if (!ProjectTemplate.TemplateNameValidator(TemplatePath))
                {
                    ProjectTemplate.InvalidFileName();
                }

                else
                {
                    if (ProjectTemplate.TestTemplate(TemplatePath))
                    {

                        var template = ProjectTemplate.GetTemplate(TemplatePath);
                        XmlTemplate xmlTemplate = new XmlTemplate();
                        bool _isManifest = template.Metadata.Path.Contains(".psd1");
                        string guid = template.Metadata.Guid.ToString();

                        if (string.IsNullOrEmpty(template.ProjectName))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.ProjectName}> is empty");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(template.Directories))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Directories}> are empty");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(template.Type))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Type}> is empty");
                            _errorCount += 1;
                        }

                        if (!string.IsNullOrEmpty(template.Type))
                        {
                            if (template.Type != "Script" & template.Type != "Module" & template.Type != "Binary")
                            {
                                ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Type}> invalid project type; Accepted values are 'Script', 'Module' and 'Binary'");
                                _errorCount += 1;
                            }                            
                        }

                        if (string.IsNullOrEmpty(template.Metadata.Author))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Author}> is empty");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(template.Metadata.Description))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Description}> is empty");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(template.Metadata.Path))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Path}> is empty");
                            _errorCount += 1;
                        }

                        if (!(_isManifest))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"Invalid path: '{template.Metadata.Path}'");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(guid))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Guid}> is empty");
                            _errorCount += 1;
                        }

                        if (!string.IsNullOrEmpty(guid))
                        {
                            if (!ProjectTemplate.IsGuid(template.Metadata.Guid))
                            {
                                ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), "Invalid Guid");
                                _errorCount += 1;
                            }
                        }

                        if (string.IsNullOrEmpty(template.Metadata.ModuleVersion))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.ModuleVersion}> is empty");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(template.Metadata.RootModule))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.RootModule}> is empty");
                            _errorCount += 1;
                        }

                        if (!template.Metadata.RootModule.Contains(".psm1"))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"Invalid root module name: '{template.Metadata.RootModule}'");
                            _errorCount += 1;
                        }

                        if (string.IsNullOrEmpty(template.Metadata.Tags))
                        {
                            ProjectTemplate.WriteMessage(ProjectTemplate.GetSign("err"), $"<{xmlTemplate.Tags}> are empty");
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
            }

            else
            {
                ProjectTemplate.FileNotFound();
            }
        }
    }        
}
