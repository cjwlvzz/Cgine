using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CgineEditor.GameProject
{
    class Project : ViewModelBase
    {
        public string Name { get; private set ; }

        public string Path { get; private set; }

        public static string Extension { get; } = ".cgine";

        public string FullPath => $"{Path}{Name}{Extension}";

        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();

        //The Collection for the xml to read
        public ReadOnlyObservableCollection<Scene> Scenes { get; }

    }

   

}