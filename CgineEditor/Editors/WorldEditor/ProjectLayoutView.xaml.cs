﻿using CgineEditor.GameProject;
using System;
using System.Collections.Generic;
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

        private void OnAddScene_Button_Clicked(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as Project;
            vm.addScene("New Scene" + vm.Scenes.Count);
        }

    }
}
