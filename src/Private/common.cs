using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace PoshProject
{
    public class ProjectTemplate
    {
        public int _errorCount { get; set; } = 0;

        public static void NewTemplate(string projectName, string path, string type, string author, string[] directories, string description,
            string id, string[] tags, string version, string[] dependsOn, string license)
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
                writer.WriteElementString(projectTemplate.License, license);
                writer.WriteStartElement(projectTemplate.Metadata);
                writer.WriteElementString(projectTemplate.Author, author);
                writer.WriteElementString(projectTemplate.Path, $"{Path.GetDirectoryName(path)}\\{projectName}.psd1");
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

        public static void InvalidFileName()
        {
            WriteMessage(GetSign("err"), "Invalid file name, expected is 'PoshProjectTemplate.xml'");
        }

        public static string GetUserName()
        {
            string _userName = null;

            try
            {
                string configPath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\.gitconfig";
                string[] contents = File.ReadAllLines(configPath);
                string pattern = ".*name.*";

                Regex regex = new Regex(pattern);
                
                foreach (string content in contents)
                {
                    if (regex.Match(content).Success)
                    {
                        _userName = regex.Match(content).Value;
                    }
                }

                return _userName.Split('=')[1].Trim();
            }

            catch
            {
                return _userName;
            }
            
        }

        public static bool TemplateNameValidator(string path)
        {
            bool _valid = false;

            if (Path.GetFileName(path) == "PoshProjectTemplate.xml")
            {
                _valid = true;
            }

            return _valid;
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
            string[] dependencies = null; 

            if (! string.IsNullOrEmpty(template.Dependencies))
            {
                dependencies = template.Dependencies.Split(',');
            }

            // Creating Module Manifest
            PowerShell ps = PowerShell.Create().AddCommand("New-ModuleManifest")
                                               .AddParameter(projectTemplate.Author, template.Metadata.Author)
                                               .AddParameter(projectTemplate.Description, template.Metadata.Description)
                                               .AddParameter(projectTemplate.Guid, template.Metadata.Guid)
                                               .AddParameter(projectTemplate.ModuleVersion, template.Metadata.ModuleVersion)
                                               .AddParameter(projectTemplate.Path, $"{path}\\{template.ProjectName}.psd1")
                                               .AddParameter(projectTemplate.Tags, template.Metadata.Tags.Split(','))
                                               .AddParameter(projectTemplate.RequiredModules, dependencies)
                                               .AddParameter(projectTemplate.RootModule, template.Metadata.RootModule);

            ps.Invoke();
        }

        public static bool FindModule(string moduleName)
        {
            bool _present = false;

            try
            {
                var result = PowerShell.Create().AddCommand("Find-Module")
                                                .AddParameter("Name", moduleName)
                                                .AddParameter("Repository", "PSGallery")
                                                .Invoke();

                _present = true ? result.Count != 0 : false;

                return _present;
            }

            catch
            {
                return _present;
            }
        }

        public static bool IsAlreadyInstalled(string moduleName)
        {
            bool _isAlreadyPresent = false;

            string[] modulePaths = Environment.GetEnvironmentVariable("PSModulePath").Split(';');

            foreach(string path in modulePaths)
            {
                string[] directories = Directory.GetDirectories(path);

                foreach(string dir in directories)
                {
                    if (Path.GetFileName(dir) == moduleName)
                    {
                        _isAlreadyPresent = true;
                    }
                }
            }

            return _isAlreadyPresent;
        }

        public static void InstallDependencies(PoshTemplate template)
        {
            // Installing dependencies
            if (!string.IsNullOrEmpty(template.Dependencies))
            {
                string[] dependencies = template.Dependencies.Split(',');

                foreach (string module in dependencies)
                {
                    if (!string.IsNullOrWhiteSpace(module))
                    {
                        WriteMessage(GetSign("info"), $"Installing {module}");

                        if (IsAlreadyInstalled(module))
                        {

                            WriteMessage(GetSign("info"), $"Already installed: {module}");
                        }

                        else if (!FindModule(module))
                        {
                            WriteMessage(GetSign("err"), $"Couldn't find the module {module} in PSGallery");
                        }

                        else
                        {
                            try
                            {
                                PowerShell ps = PowerShell.Create().AddCommand("Install-Module")
                                                                       .AddParameter("Name", module)
                                                                       .AddParameter("Scope", "CurrentUser")
                                                                       .AddParameter("Force", true)
                                                                       .AddParameter("AllowClobber", true);

                                ps.Invoke();

                                WriteMessage(GetSign("info"), $"Successfully installed: {module}");
                            }

                            catch
                            {
                                WriteMessage(GetSign("err"), $"Installation failed: {module}");
                            }
                        }
                    }

                    else
                    {
                        WriteMessage(GetSign("err"), $"Invalid module name");
                    }
                }
            }

            else
            {
                WriteMessage(GetSign("info"), "No dependencies found");
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

        public static string ReadContents(string path, string replaceString = null, string replaceValue = null)
        {
            var contents = File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(replaceString) && !string.IsNullOrWhiteSpace(replaceValue))
            {
                return contents.Replace(replaceString, replaceValue);
            }

            return contents;
        }

        public static void CreateProject(PoshTemplate template, bool installDependencies = false)
        {
            var projectPath = template.Metadata.Path.Replace(".psd1", "");
            TemplateContents contents = new TemplateContents();
            string _path = contents.RootPath;
            string _currentPath = $"{_path}{contents.ContentsPath}";
            string modulePath = $"{_currentPath}{contents.ModulePath}";
            string classPath = $"{_currentPath}{contents.ClassPath}";
            string testsPath = $"{_currentPath}{contents.TestsPath}";
            string mitPath = $"{_currentPath}{contents.MITPath}";
            string apachePath = $"{_currentPath}{contents.ApachePath}";
            string aboutMD = $"{_currentPath}{contents.AboutMDPath}";
            string aboutText = $"{_currentPath}{contents.AboutTextPath}";

            if (Directory.Exists(projectPath))
            {
                WriteMessage(GetSign("err"), $"Project '{projectPath}' already exists");
            }

            else
            {
                string sign = GetSign("info");

                // Creating Project
                WriteMessage(sign, "Creating Project");

                // Creating Project Directory
                WriteMessage(sign, "Creating Project Directory");
                Directory.CreateDirectory(projectPath);

                // Creating manifest
                WriteMessage(sign, "Creating Module Manifest");
                CreateManifest(template, projectPath);

                // Creating module file
                WriteMessage(sign, "Creating Root Module");
                File.WriteAllText($"{projectPath}\\{template.Metadata.RootModule}", ReadContents(modulePath));

                // Creating README
                WriteMessage(sign, "Creating README.md");
                File.WriteAllText($"{projectPath}\\README.md", $"# {template.ProjectName}");

                // Adding License
                if (!string.IsNullOrEmpty(template.License))
                {
                    WriteMessage(sign, $"Adding License");

                    if (template.License == "MIT")
                    {
                        File.WriteAllText($"{projectPath}\\LICENSE", ReadContents(mitPath, "@AuthorName", template.Metadata.Author));
                    }

                    else if (template.License == "Apache")
                    {
                        File.WriteAllText($"{projectPath}\\LICENSE", ReadContents(apachePath, "@AuthorName", template.Metadata.Author));
                    }
                }

                if (template.Type == "Script")
                {
                    // Creating Directories (in this case it will be a .tests file)
                    WriteMessage(sign, "Creating Pester Tests File");
                    File.WriteAllText($"{projectPath}\\{template.Directories}", ReadContents(testsPath));
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

                        if (dir == "Classes" || dir == "Class")
                        {
                            File.WriteAllText($"{projectPath}\\{dir}\\{template.ProjectName}.Class.ps1", ReadContents(classPath));
                        }

                        else if (dir == "Public")
                        {
                            File.WriteAllText($"{projectPath}\\{dir}\\{template.ProjectName}.ps1", ReadContents(modulePath));
                        }

                        else if (dir == "Tests" || dir == "Test" || dir == "tests" || dir == "test")
                        {
                            File.WriteAllText($"{projectPath}\\{dir}\\{template.ProjectName}.Tests.ps1", ReadContents(testsPath));
                        }

                        else if (dir == "docs" || dir == "doc" || dir == "documents")
                        {
                            File.WriteAllText($"{projectPath}\\{dir}\\about_Module.help.md", ReadContents(aboutMD, "@ProjectName", template.ProjectName));
                        }

                        else if (dir == "en-US")
                        {
                            File.WriteAllText($"{projectPath}\\{dir}\\about_Module.help.txt", ReadContents(aboutText, "@ProjectName", template.ProjectName));
                        }
                    }
                }

                if (installDependencies)
                {
                    // Installing Dependencies
                    WriteMessage(sign, "Installing Dependencies");
                    InstallDependencies(template);
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
                string guid = template.Metadata.Guid.ToString();

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

                if (!string.IsNullOrEmpty(template.Type))
                {
                    if (template.Type != "Script" & template.Type != "Module" & template.Type != "Binary")
                    {
                        projectTemplate._errorCount += 1;
                    }
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

                if (string.IsNullOrEmpty(guid))
                {
                    projectTemplate._errorCount += 1;
                }

                if (! string.IsNullOrEmpty(guid))
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
            string guid = templateObject.Metadata.Guid.ToString();

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

            if (!string.IsNullOrEmpty(templateObject.Type))
            {
                if (templateObject.Type != "Script" & templateObject.Type != "Module" & templateObject.Type != "Binary")
                {
                    projectTemplate._errorCount += 1;
                }
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

            if (string.IsNullOrEmpty(guid))
            {
                projectTemplate._errorCount += 1;
            }

            if (!string.IsNullOrEmpty(guid))
            {
                if (!IsGuid(templateObject.Metadata.Guid))
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
