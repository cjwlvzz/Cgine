using CgineEditor.ECS;
using CgineEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;

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

        [DataMember(Name = nameof(Entities))]
        private readonly  ObservableCollection<Entity> _entities = new ObservableCollection<Entity>();

        public ReadOnlyObservableCollection<Entity> Entities { get; private set; }

        public ICommand AddEntityCommand { get; private set; }

        public ICommand RemoveEntityCommand { get; private set; }

        private void AddEntity(Entity entity)
        {
            Debug.Assert(!_entities.Contains(entity));
            _entities.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            Debug.Assert(_entities.Contains(entity));
            _entities.Remove(entity);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_entities != null)
            {
                Entities = new ReadOnlyObservableCollection<Entity>(_entities);
                OnPropertyChanged(nameof(Entities));
            }

            AddEntityCommand = new RelayCommand<Entity>(x =>
            {
                AddEntity(x);
                var entityIndex = _entities.Count - 1;
                Project.UndoRedo.Add(new UndoRedoAction(
                    () => RemoveEntity(x),
                    () =>_entities.Insert(entityIndex,x),
                    $"Add{x.Name} to {Name}"
                    ));
            });

            RemoveEntityCommand = new RelayCommand<Entity>(x =>
            {
                var entityIndex = _entities.IndexOf(x);
                RemoveEntity(x);

                Project.UndoRedo.Add(new UndoRedoAction(
                    () => _entities.Insert(entityIndex, x),
                    () => RemoveEntity(x),
                    $"Remove {x.Name}"));

            });


        }

        public Scene(Project project,string name)
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;

            OnDeserialized(new StreamingContext());

        }

    }
}
