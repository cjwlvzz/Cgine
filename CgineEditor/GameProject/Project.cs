﻿using CgineEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CgineEditor.GameProject
{
    [DataContract(Name = "Game")]
    public class Project : ViewModelBase
    {
        [DataMember]
        public string Name { get; private set; } = "New Project";

        [DataMember]
        public string Path { get; private set; }

        public static string Extension { get; } = ".cgine";

        public string FullPath => $@"{Path}{Name}\{Name}{Extension}";

        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> _scenes = new ObservableCollection<Scene>();

        //The Collection for the xml to read
        public ReadOnlyObservableCollection<Scene> Scenes { get; private set; }

        private Scene _activeScene;

        public Scene ActiveScene
        {
            get => _activeScene;
            set
            {
                if(_activeScene != value)
                {
                    _activeScene = value;
                    OnPropertyChanged(nameof(ActiveScene));
                }
            }
        }

        public static Project currentProject => Application.Current.MainWindow.DataContext as Project;

        public static UndoRedo UndoRedo { get; } = new UndoRedo();

        public ICommand UndoCommand { get; private set; }

        public ICommand RedoCommand { get; private set; }

        public ICommand AddSceneCommand { get; private set; }

        public ICommand RemoveSceneCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }


        private void AddSceneInternal(string sceneName)
        {
            Debug.Assert(!string.IsNullOrEmpty(sceneName.Trim()));
            _scenes.Add(new Scene(this, sceneName));
        }

        private void RemoveSceneInternal(Scene scene)
        {
            Debug.Assert(_scenes.Contains(scene));
            _scenes.Remove(scene);
        }

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

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if(_scenes != null)
            {
                Scenes = new ReadOnlyObservableCollection<Scene>(_scenes);
                OnPropertyChanged(nameof(Scenes));
            }
            ActiveScene = Scenes.FirstOrDefault(x => x.IsActive);

            AddSceneCommand = new RelayCommand<object>(x =>
            {
                AddSceneInternal($"New Scene {_scenes.Count}");
                var newScene = _scenes.Last();
                var sceneIndex = _scenes.Count - 1;
                UndoRedo.Add(new UndoRedoAction(
                    () => RemoveSceneInternal(newScene),
                    () => _scenes.Insert(sceneIndex,newScene),
                    $"Add{newScene.Name}"
                    ));
            });

            RemoveSceneCommand = new RelayCommand<Scene>(x =>
            {
                var sceneIndex = _scenes.IndexOf(x);
                RemoveSceneInternal(x);

                UndoRedo.Add(new UndoRedoAction(
                    () => _scenes.Insert(sceneIndex,x),
                    () => RemoveSceneInternal(x),
                    $"Remove {x.Name}" ));

            }, x=> !x.IsActive);

            UndoCommand = new RelayCommand<object>(x => UndoRedo.Undo());
            RedoCommand = new RelayCommand<object>(x => UndoRedo.Redo());
            SaveCommand = new RelayCommand<object>(x => Save(this));

        }

        public Project(string name , string path)
        {
            Name = name;
            Path = path;

            OnDeserialized(new StreamingContext());
        }

        //TODO Lists of game entities 

    }

   

}