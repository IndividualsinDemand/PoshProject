using System;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace PoshProject
{
    public class ProjectTemplate
    {
        public int _errorCount { get; set; } = 0;

        public static void NewTemplate(string projectName, string path, string type, string author, string[] directories, string description,
            string id, string[] tags, string version, string[] dependsOn)
        {
            XmlTemplate projectTemplate = new XmlTemplate();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t"
            };

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartElement(projectTemplate.XmlRootAttributeValue);
                writer.WriteElementString(projectTemplate.ProjectName, projectName);
                writer.WriteElementString(projectTemplate.Directories, string.Join(",", directories));
                writer.WriteElementString(projectTemplate.Type, type);
                writer.WriteElementString(projectTemplate.Dependencies, string.Join(",", dependsOn));
                writer.WriteStartElement(projectTemplate.Metadata);
                writer.WriteElementString(projectTemplate.Author, author);
                writer.WriteElementString(projectTemplate.Path, path.Replace(".xml", ".psd1"));
                writer.WriteElementString(projectTemplate.RootModule, $"{projectName}.psm1");
                writer.WriteElementString(projectTemplate.Description, description);
                writer.WriteElementString(projectTemplate.Guid, id);
                writer.WriteElementString(projectTemplate.Tags, string.Join(",", tags));
                writer.WriteElementString(projectTemplate.ModuleVersion, version);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        public static PoshTemplate DeserializeTemplate(string path)
        {
            try
            {
                XmlTemplate template = new XmlTemplate();

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PoshTemplate), new XmlRootAttribute(template.XmlRootAttributeValue));

                StringReader stringReader = new StringReader(File.ReadAllText(path));

                PoshTemplate poshTemplate = (PoshTemplate)xmlSerializer.Deserialize(stringReader);

                return poshTemplate;
            }

            catch
            {
                return null;
            }
        }

        public static void FileNotFound()
        {
            WriteMessage(GetSign("err"), "File not Found");
        }

        public static void InvalidTemplate()
        {
            WriteMessage(GetSign("err"), "Invalid Template");
        }

        public static PoshTemplate GetTemplate(string path)
        {

            if (DeserializeTemplate(path) != null)
            {
                return DeserializeTemplate(path);
            }

            else
            {
                return null;
            }
        }        

        public static bool TestTemplate(string path)
        {
            bool _valid = false;

            if (DeserializeTemplate(path) != null)
            {
                _valid = true;
            }

            return _valid;
        }

        private static void CreateManifest(PoshTemplate template, string path)
        {
            XmlTemplate projectTemplate = new XmlTemplate();

            // Creating Module Manifest
            PowerShell ps = PowerShell.Create().AddCommand("New-ModuleManifest")
                                               .AddParameter(projectTemplate.Author, template.Metadata.Author)
                                               .AddParameter(projectTemplate.Description, template.Metadata.Description)
                                               .AddParameter(projectTemplate.Guid, template.Metadata.Guid)
                                               .AddParameter(projectTemplate.ModuleVersion, template.Metadata.ModuleVersion)
                                               .AddParameter(projectTemplate.Path, $"{path}\\{template.ProjectName}.psd1")
                                               .AddParameter(projectTemplate.Tags, template.Metadata.Tags.Split(','))
                                               .AddParameter(projectTemplate.RootModule, template.Metadata.RootModule);

            ps.Invoke();
        }

        public static void InstallDependencies(PoshTemplate template)
        {
            // Installing dependencies
            string[] dependencies = template.Dependencies.Split(',');

            foreach (string module in dependencies)
            {
                PowerShell ps = PowerShell.Create().AddCommand("Install-Module")
                                                   .AddParameter("Name", module)
                                                   .AddParameter("Scope", "CurrentUser")
                                                   .AddParameter("Force", true)
                                                   .AddParameter("AllowClobber", true);

                ps.Invoke();
            }
        }

        public static void WriteMessage(string sign, string message)
        {
            // Saving the current settings
            ConsoleColor currentForeground = Console.ForegroundColor;

            if (sign == GetSign("err"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            else if (sign == GetSign("info"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            string consoleMessage = $"{sign} {message}";
            
            Console.WriteLine(consoleMessage);

            // setting back to same color.
            Console.ForegroundColor = currentForeground;
        }

        public static void CreateProject(PoshTemplate template)
        {
            string sign = GetSign("info");

            // Creating Project
            WriteMessage(sign, "Creating Project");
            var projectPath = template.Metadata.Path.Replace(".psd1", "");

            // Creating Project Directory
            WriteMessage(sign, "Creating Project Directory");
            Directory.CreateDirectory(projectPath);

            // Creating module file
            WriteMessage(sign, "Creating Root Module");
            File.Create($"{projectPath}\\{template.Metadata.RootModule}");

            // Creating manifest
            WriteMessage(sign, "Creating Module Manifest");
            CreateManifest(template, projectPath);

            if (template.Type == "Script")
            {
                // Creating Directories (in this case it will be a .tests file)
                WriteMessage(sign, "Creating Pester Tests File");
                File.Create($"{projectPath}\\{template.Directories}");
            }

            else
            {
                // Creating Directories
                WriteMessage(sign, "Creating Project Directories");
                string[] directories = template.Directories.Split(',');

                foreach (string dir in directories)
                {
                    WriteMessage(sign, $"Creating {dir}");
                    Directory.CreateDirectory($"{projectPath}\\{dir}");
                }
            }

            // Installing Dependencies
            InstallDependencies(template);
        }

        public static string GetSign(string message)
        {
            switch (message)
            {
                case "err":
                    return "[-]";
                case "info":
                    return "[+]";
                default:
                    return "[-]";
            }
        }

        public static bool IsGuid(Guid Guid)
        {
            Regex regex = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

            return regex.IsMatch(Guid.ToString());
        }

        public static int ValidateTemplate(string path)
        {
            ProjectTemplate projectTemplate = new ProjectTemplate();

            if (TestTemplate(path))
            {                
                var template = GetTemplate(path);
                bool _isManifest = template.Metadata.Path.Contains(".psd1");

                if (string.IsNullOrEmpty(template.ProjectName))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Directories))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Type))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Metadata.Author))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Metadata.Description))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Metadata.Path))
                {
                    projectTemplate._errorCount += 1;
                }

                if (!(_isManifest))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Metadata.Guid.ToString()))
                {
                    projectTemplate._errorCount += 1;
                }

                if (! string.IsNullOrEmpty(template.Metadata.Guid.ToString()))
                {
                    if(! IsGuid(template.Metadata.Guid))
                    {
                        projectTemplate._errorCount += 1;
                    }                    
                }

                if (string.IsNullOrEmpty(template.Metadata.ModuleVersion))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Metadata.RootModule))
                {
                    projectTemplate._errorCount += 1;
                }

                if (!template.Metadata.RootModule.Contains(".psm1"))
                {
                    projectTemplate._errorCount += 1;
                }

                if (string.IsNullOrEmpty(template.Metadata.Tags))
                {
                    projectTemplate._errorCount += 1;
                }

                return projectTemplate._errorCount;
            }
            
            return projectTemplate._errorCount;
        }

        public static int ValidateTemplateObject(PoshTemplate templateObject)
        {
            ProjectTemplate projectTemplate = new ProjectTemplate();

            bool _isManifest = templateObject.Metadata.Path.Contains(".psd1");

            if (string.IsNullOrEmpty(templateObject.ProjectName))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Directories))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Type))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.Author))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.Description))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.Path))
            {
                projectTemplate._errorCount += 1;
            }

            if (!(_isManifest))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.Guid.ToString()))
            {
                projectTemplate._errorCount += 1;
            }

            if (!string.IsNullOrEmpty(templateObject.Metadata.Guid.ToString()))
            {
                if (!IsGuid(template.Metadata.Guid))
                {
                    projectTemplate._errorCount += 1;
                }
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.ModuleVersion))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.RootModule))
            {
                projectTemplate._errorCount += 1;
            }

            if (!templateObject.Metadata.RootModule.Contains(".psm1"))
            {
                projectTemplate._errorCount += 1;
            }

            if (string.IsNullOrEmpty(templateObject.Metadata.Tags))
            {
                projectTemplate._errorCount += 1;
            }

            return projectTemplate._errorCount;
        }
    }
}
