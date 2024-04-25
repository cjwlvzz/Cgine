using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using CgineEditor.Utils;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using Path = System.IO.Path;

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
        public string ProjectFile { get; set; }

        [DataMember]
        //list of folders that would be create for the game project
        public List<string> ProjectFolders { get; set; }

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
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\CgineProjects\";
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                if (_errorMsg != value)
                {
                    _errorMsg = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        //ReadOnlyObsetvableCollection is for the xml configration to read
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates
        { get; }

        private bool ValidateProjectPath()
        {
            var path = ProjectPath;
            if (!Path.EndsInDirectorySeparator(path))
            {
                path += @"\";
            }
            path += $@"{ProjectName}\";
            IsValid = false;

            if (string.IsNullOrEmpty(ProjectName.Trim()))
            {
                ErrorMsg = "Project name cannot be empty";
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMsg = "Project name contains invalid characters";
            }
            else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
            {
                ErrorMsg = "Please select a valid project folder";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ErrorMsg = "Project path contains invalid characters";
            }
            else if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                ErrorMsg = "Project file must be created in an empty folder";
            }
            else
            {
                ErrorMsg = string.Empty;
                IsValid = true;
            }
            return IsValid;
        }

        /// <summary>
        ///  return the project fil path created
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public string CreateProject(ProjectTemplate template)
        {
            ValidateProjectPath();

            if (!IsValid)
            {
                return string.Empty;
            }

            if (!Path.EndsInDirectorySeparator(ProjectPath))
            {
                ProjectPath += @"\";
            }

            var path = $@"{ProjectPath}{ProjectName}\";

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //create project folders for the new project
                foreach (var folder in template.ProjectFolders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
                }
                var fileDirInfo = new DirectoryInfo(path + @".Cgine\");
                fileDirInfo.Attributes |= FileAttributes.Hidden;
                File.Copy(template.IconFilePath, Path.GetFullPath(Path.Combine(fileDirInfo.FullName, "Icon.png")));
                File.Copy(template.ScreenshotFilePath, Path.GetFullPath(Path.Combine(fileDirInfo.FullName, "Screenshot.png")));

                var projectXmlConfig = File.ReadAllText(template.ProjectFilePath);
                /*The code is using the string.Format method to format the string projectXmlConfig. In this case, projectXmlConfig is a string template that may contain placeholders (such as {0}, {1}, etc.),
                and the string.Format method replaces these placeholders with the arguments provided
                (in this case, ProjectName and ProjectPath) to generate the final formatted string.*/
                projectXmlConfig = string.Format(projectXmlConfig, ProjectName, ProjectPath);
                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extension}"));
                File.WriteAllText(projectPath, projectXmlConfig);

                return path;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log(MessageType.Error,$"Failed to create {template.ProjectType}");
                return string.Empty;
            }

        }

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
                ValidateProjectPath();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log(MessageType.Error, $"Failed to read project templates");
            }
        }

    }

}
