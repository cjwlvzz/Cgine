﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace CgineEditor.GameProject
{
    [DataContract]
    public class Scene :ViewModelBase
    {
        private string _name;
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(Name);
                }
            }
        }

        [DataMember]
        public Project Project { get; private set; }

        private bool _isActive;

        [DataMember]

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public Scene(Project project,string name)
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;
        }

    }
}
