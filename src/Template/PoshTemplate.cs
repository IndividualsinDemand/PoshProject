using System;

namespace PoshProject
{
    public class PoshTemplate
    {
        public string ProjectName { get; set; }
        public string Directories { get; set; }
        public string Type { get; set; }
        public string Dependencies { get; set; }
        public string License { get; set; }
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
