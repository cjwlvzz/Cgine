using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using CgineEditor.Utils;

namespace CgineEditor.GameProject
{
    //defines what data is serialized(turned into XML) to be exchanged
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        //project type the user choose to create
        public string ProjectType { get; set; }

        [DataMember]
        //name of the file that is going to be our game project
        public string ProjectFile { get; set;  }

        [DataMember]
        //list of folders that would be create for the game project
        public List<string> ProjectFolders { get; set;  }


    }

    class NewProject : ViewModelBase
    {
        // TODO: get the path from the installation location
        private readonly string _templatePath = @"..\..\CgineEditor\ProjectTemplates";
        private string _name = "NewProject";
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\CgineProject\";
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        public NewProject()
        {
            try
            {
                var templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templateFiles.Any());
                foreach (var file in templateFiles)
                {
                    var template = new ProjectTemplate()
                    {
                        ProjectType = "Empty Project",
                        ProjectFile = "project.Cgine",
                        ProjectFolders = new List<string>() {".Cgine","Content","GameScript" }
                    };

                    Serializer.ToFile(template, file);

                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TODO :log error in engine editor
            }
            }

    }

}
