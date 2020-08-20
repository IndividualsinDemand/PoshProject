using System;
using System.Xml;

namespace PoshProject
{
    public class ProjectTemplate
    {
        public static void NewTemplate(string projectName, string path, string type, string author, string[] directories, string description, string id, string[] tags, string version)
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
                writer.WriteStartElement("Metadata");
                writer.WriteElementString("Author", author);
                writer.WriteElementString("Path", path.Replace(".xml", ".psd1"));
                writer.WriteElementString("Root", $"{projectName}.psm1");
                writer.WriteElementString("Description", description);
                writer.WriteElementString("Guid", id);
                writer.WriteElementString("Tags", string.Join(",", tags));
                writer.WriteElementString("Version", version);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
