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
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t"
            };

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartElement("Configuration");
                writer.WriteElementString("ProjectName", projectName);
                writer.WriteElementString("Directories", string.Join(",", directories));
                writer.WriteElementString("Type", type);
                writer.WriteElementString("Dependencies", string.Join(",", dependsOn));
                writer.WriteStartElement("Metadata");
                writer.WriteElementString("Author", author);
                writer.WriteElementString("Path", path.Replace(".xml", ".psd1"));
                writer.WriteElementString("RootModule", $"{projectName}.psm1");
                writer.WriteElementString("Description", description);
                writer.WriteElementString("Guid", id);
                writer.WriteElementString("Tags", string.Join(",", tags));
                writer.WriteElementString("ModuleVersion", version);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        public static PoshTemplate GetTemplate(string path)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PoshTemplate), new XmlRootAttribute("Configuration"));

                StringReader stringReader = new StringReader(File.ReadAllText(path));

                PoshTemplate poshTemplate = (PoshTemplate)xmlSerializer.Deserialize(stringReader);

                return poshTemplate;
            }

            catch
            {
                throw new FileLoadException("Invalid Template; Load the correct template and try again");
            }
            
        }

        public static FileNotFoundException HandleFileNotFoundException()
        {
            throw new FileNotFoundException("Couldn't find the part of file from given file path");
        }
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
