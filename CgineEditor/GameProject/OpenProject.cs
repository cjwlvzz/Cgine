using System;
using System.Collections.Generic;
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
        static OpenProject()
        {

        }
    }
}
