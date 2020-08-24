using System;
using System.IO;
using System.Management.Automation;
using System.Xml;
using System.Xml.Serialization;

namespace PoshProject
{
    public class ProjectTemplate
    {       
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
    }
}
