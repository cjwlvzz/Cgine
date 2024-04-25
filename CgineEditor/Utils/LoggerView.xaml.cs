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

namespace CgineEditor.Utils
{
    /// <summary>
    /// LoggerView.xaml 的交互逻辑
    /// </summary>
    public partial class LoggerView : UserControl
    {
        public LoggerView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                Logger.Log(MessageType.Error, "error log");
                Logger.Log(MessageType.Info, "error log");
                Logger.Log(MessageType.Warning, "error log");


            };

        }

        private void OnClear_Button_Click(object sender, RoutedEventArgs e)
        {
            Logger.Clear();
        }

        private void OnMessageFilter_Button_Click(object sender, RoutedEventArgs e)
        {
            var filter = 0x0;
            if (ToggleInfo.IsChecked == true) filter |= (int)MessageType.Info;
            if (ToggleWarnings.IsChecked == true) filter |= (int)MessageType.Warning;
            if (ToggleErrors.IsChecked == true) filter |= (int)MessageType.Error;
            Logger.SetMessageFilter(filter);
        }
    }
}
