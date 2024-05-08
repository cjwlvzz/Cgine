using CgineEditor.ECS;
using CgineEditor.GameProject;
using CgineEditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CgineEditor.Editors
{
    /// <summary>
    /// ProjectLayoutView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectLayoutView : UserControl
    {
        public ProjectLayoutView()
        {
            InitializeComponent();
        }

        private void OnAddEntity_Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var vm = btn.DataContext as Scene;
            vm.AddEntityCommand.Execute(new Entity(vm) { Name = "Empty Entity"});
        }

        private void OnGameEntites_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EntityView.Instance.DataContext = null;
            var listBox = sender as ListBox;
            
            var newSelection = listBox.SelectedItems.Cast<Entity>().ToList();
            var previousSelection = newSelection.Except(e.AddedItems.Cast<Entity>()).Concat ( e.RemovedItems.Cast<Entity>()).ToList();

            Project.UndoRedo.Add(new UndoRedoAction(
                () => {
                    listBox.UnselectAll();
                    previousSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                },
                () => {
                    listBox.UnselectAll();
                    newSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                },
                "Selection Changed"
                ));
            MSEntity msEntity = null;
            if(newSelection.Any())
            {
                msEntity = new MSEntity(newSelection);
            }
            EntityView.Instance.DataContext = msEntity;
        }
    }
}
