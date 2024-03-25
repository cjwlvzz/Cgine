using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

namespace CgineEditor.GameProject
{
    [DataContract(Name = "Game")]
    public class Project : ViewModelBase
    {
        [DataMember]
        public string Name { get; private set ; }

        [DataMember]
        public string Path { get; private set; }

        public static string Extension { get; } = ".cgine";

        public string FullPath => $"{Path}{Name}{Extension}";

        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();

        //The Collection for the xml to read
        public ReadOnlyObservableCollection<Scene> Scenes { get; }

        public Project(string name , string path)
        {
            Name = name;
            Path = path;

            _scenes.Add(new Scene(this, "default scene"));
        }

        //TODO Lists of game entities 

    }

   

}