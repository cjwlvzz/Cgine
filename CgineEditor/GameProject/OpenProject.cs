using CgineEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CgineEditor.GameProject
{

    [DataContract]
    public class ProjectData
    {
        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public string ProjectPath { get; set; }

        [DataMember]
        public DateTime Data { get; set; }

        public string FullPath { get => $"{ProjectPath}{ProjectName}{Project.Extension}"; }

        public byte[] Icon { get; set; }

        public byte[] Screenshot { get; set; }

    }

    [DataContract]
    public class ProjectDataList
    {
        [DataMember]
        public List<ProjectData> Projects { get; set; }
    }

    //to remember where project and sava that data into the file
    class OpenProject
    {
        private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\CgineEditor\";
        private static readonly string _projecDataPath;
        private static readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();

        public static ReadOnlyObservableCollection<ProjectData> Projects { get;}

        private static void ReadProjectData()
        {
            if(File.Exists(_projecDataPath))
            {
                var projects = Serializer.FromFile<ProjectDataList>(_projecDataPath).Projects.OrderByDescending(x => x.Data);
                _projects.Clear();
                foreach (var project in projects)
                {
                    if(File.Exists(project.FullPath))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.Cgine\Icon.png");
                        project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\.Cgine\Screenshot.png");
                        _projects.Add(project);
                    }
                }
            }
        }
        private static void WriteProjectData()
        {
            var projects = _projects.OrderBy(x => x.Data).ToList();
            Serializer.ToFile(new ProjectDataList() { Projects = projects }, _projecDataPath);
        }

        public static Project Open(ProjectData projectData)
        {
            ReadProjectData();
            var project = _projects.FirstOrDefault(x => x.FullPath == projectData.FullPath);
            if(project != null)
            {
                project.Data = DateTime.Now;
            }
            else
            {
                project = projectData;
                project.Data = DateTime.Now;
                _projects.Add(project);
            }
            WriteProjectData();

            return null;
        }

       

        static OpenProject()
        {
            try
            {
                if (!Directory.Exists(_applicationDataPath)) 
                {
                    Directory.CreateDirectory(_applicationDataPath);
                }
                _projecDataPath = $@"{_applicationDataPath}ProjectData.xml";
                Projects = new ReadOnlyObservableCollection<ProjectData>(_projects);
                ReadProjectData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //TODO Egine log errors
            }
        }
    }
}
