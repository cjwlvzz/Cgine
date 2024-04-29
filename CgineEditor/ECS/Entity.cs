﻿using CgineEditor.GameProject;
using CgineEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;

namespace CgineEditor.ECS
{
    [DataContract]
    [KnownType(typeof(TransformComponent))]
    class Entity : ViewModelBase
    {

        private bool _isEnabled = true;
        [DataMember]
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        private string _name;
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [DataMember]
        public Scene parentScene { get; private set; }

        [DataMember(Name = nameof(Components))]
        private readonly ObservableCollection<ComponentBase> _components = new ObservableCollection<ComponentBase>();

        public ReadOnlyObservableCollection<ComponentBase> Components { get; private set; }

        public ICommand RenameCommand { set; private get; }

        public ICommand IsEnabledCommand { set; private get; }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(_components != null)
            {
                Components = new ReadOnlyObservableCollection<ComponentBase>(_components);
                OnPropertyChanged(nameof(Components));
            }

            RenameCommand = new RelayCommand<string>(x =>
            {
                var oldName = _name;
                Name = x;

                Project.UndoRedo.Add(new UndoRedoAction(nameof(Name),this,oldName,x,$"Rename Entity'{oldName}' to '{x}'"));

            },x => x != _name);

            IsEnabledCommand = new RelayCommand<bool>(x =>
            {
                var oldValue = _isEnabled;
                _isEnabled = x;

                Project.UndoRedo.Add(new UndoRedoAction(nameof(IsEnabled), this, oldValue, x, x ? $"Enable {Name}" : $"Disable {Name}"));

            });
        }

        public Entity(Scene scene)
        {
            Debug.Assert(scene != null);
            parentScene = scene;
            _components.Add(new TransformComponent(this));
            OnDeserialized(new StreamingContext());
        }

        abstract class MSEntityBase : ViewModelBase
        {
            private bool? _isEnabled = true;
            [DataMember]
            public bool? IsEnabled
            {
                get => _isEnabled;
                set
                {
                    if (_isEnabled != value)
                    {
                        _isEnabled = value;
                        OnPropertyChanged(nameof(IsEnabled));
                    }
                }
            }

            private string _name;
            [DataMember]
            public string Name
            {
                get => _name;
                set
                {
                    if (value != _name)
                    {
                        _name = value;
                        OnPropertyChanged(nameof(Name));
                    }
                }
            }

            private readonly ObservableCollection<IMSComponent> _components = new ObservableCollection<IMSComponent>();
            public ReadOnlyObservableCollection<IMSComponent> Components { get; }

            public List<Entity> SelectedEntities { get; }

            public MSEntityBase(List<Entity> entities)
            {
                Debug.Assert(entities?.Any() == true);
                Components = new ReadOnlyObservableCollection<IMSComponent>(_components);
                SelectedEntities = entities;
                PropertyChanged += (s, e) => { UpdateEntities(e.PropertyName); };
            }

            protected virtual void UpdateEntities(string propertyName)
            {
                
            }
        }

        class MSEntity : MSEntityBase
        {

        }

    }
}
