using System;
using System.IO;
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
                throw HandleFileLoadException();
            }
        }

        public static FileNotFoundException HandleFileNotFoundException()
        {
            throw new FileNotFoundException("Couldn't find the part of file from given file path");
        }

        public static FileLoadException HandleFileLoadException()
        {
            throw new FileLoadException("Invalid Template; Load the correct template and try again");
        }

        public static PoshTemplate GetTemplate(string path)
        {
            try
            {
                return DeserializeTemplate(path);
            }

            catch
            {
                throw HandleFileLoadException();
            }
        }        

        public static bool TestTemplate(string path)
        {
            bool _valid = false;

            if (! (string.IsNullOrEmpty(DeserializeTemplate(path).ToString())))
            {
                _valid = true;
            }

            return _valid;
        }
    }

    public class XmlTemplate
    {
        public string XmlRootAttributeValue { get; set; } = "Configuration";
        public string Metadata { get; set; } = "Metadata";
        public string ProjectName { get; set; } = "ProjectName";
        public string Directories { get; set; } = "Directories";
        public string Type { get; set; } = "Type";
        public string Dependencies { get; set; } = "Dependencies";
        public string Author { get; set; } = "Author";
        public string Path { get; set; } = "Metadata";
        public string RootModule { get; set; } = "RootModule";
        public string Description { get; set; } = "Description";
        public string Guid { get; set; } = "Guid";
        public string Tags { get; set; } = "Tags";
        public string ModuleVersion { get; set; } = "ModuleVersion";
    }

    public class PoshTemplate
    {
        public string ProjectName { get; set; }
        public string Directories { get; set; }
        public string Type { get; set; }
        public string Dependencies { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public string Author { get; set; }
        public string Path { get; set; }
        public string RootModule { get; set; }
        public string Description { get; set; }
        public Guid Guid { get; set; }
        public string Tags { get; set; }
        public string ModuleVersion { get; set; }
    }
}
