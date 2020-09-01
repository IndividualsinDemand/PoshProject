using System.Reflection;
using System.IO;

namespace PoshProject
{
    public class TemplateContents
    {
        public string RootPath { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\bin", "");
        public string AssemblyRootPath { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\\bin", "").Replace("\\Debug", "").Replace("\\netstandard2.0", "");

        public string CurrentContentsPath { get; set; } = "\\src\\Private\\Contents";
        public string ContentsPath { get; set; } = "\\Contents";

        public string ModulePath { get; set; } = "\\Get-Something.ps1";
        public string ClassPath { get; set; } = "\\Class.ps1";
        public string TestsPath { get; set; } = "\\Get-Something.Tests.ps1";
        public string MITPath { get; set; } = "\\MIT.txt";
        public string ApachePath { get; set; } = "\\Apache.txt";
        public string AboutMDPath { get; set; } = "\\about_Module.help.md";
        public string AboutTextPath { get; set; } = "\\about_Module.help.txt";
    }
}
