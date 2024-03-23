using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using CgineEditor.Utils;
using System.Collections.ObjectModel;

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

        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }

        public string IconFilePath { get; set; }
        public string ScreenshotFilePath { get; set; }
        public string ProjectFilePath { get; set; }

    }

    class NewProject : ViewModelBase
    {
        // TODO: get the path from the installation location
        private readonly string _templatePath = @"..\..\CgineEditor\ProjectTemplates";
        private string _projectName = "NewProject";
        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\CgineProject\";
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

      
        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        //ReadOnlyObsetvableCollection is for the xml configration to read
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
        { get ;}

        public NewProject()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);
            try
            {
                var templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templateFiles.Any());
                foreach (var file in templateFiles)
                {

                    var template = Serializer.FromFile<ProjectTemplate>(file);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ScreenshotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Screenshot.png"));
                    template.Screenshot = File.ReadAllBytes(template.ScreenshotFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
                    _projectTemplates.Add(template);

                    //var template = new ProjectTemplate()
                    //{
                        //    ProjectType = "Empty Project",
                        //    ProjectFile = "project.Cgine",
                        //    ProjectFolders = new List<string>() {".Cgine","Content","GameScript" }
                    //};

                //Serializer.ToFile(template, file);

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
