using CgineEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;

namespace CgineEditor.GameProject
{
    [DataContract(Name = "Game")]
    public class Project : ViewModelBase
    {
        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string Path { get; private set; }

        public static string Extension { get; } = ".cgine";

        public string FullPath => $"{Path}{Name}{Extension}";

        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();

        //The Collection for the xml to read
        public ReadOnlyObservableCollection<Scene> Scenes { get; }

        public static Project currentProject => Application.Current.MainWindow.DataContext as Project;

        public void Unload()
        {

        }

        public static void Save(Project project)
        {

            Serializer.ToFile(project, project.FullPath);

        }

        public static Project Load(string filePath)
        {
            Debug.Assert(File.Exists(filePath));

            return Serializer.FromFile<Project>(filePath);

        }

        public Project(string name , string path)
        {
            Name = name;
            Path = path;

            _scenes.Add(new Scene(this, "default scene"));
        }

        //TODO Lists of game entities 

    }

   

}