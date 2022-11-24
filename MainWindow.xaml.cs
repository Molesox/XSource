using DevExpress.Xpf.Core;
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

namespace XSource
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }



        private void OnClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var result = ThemedMessageBox.Show(title: "XSource", text: "T'es sûr de vouloir quitter ?", messageBoxButtons: MessageBoxButton.OKCancel, icon: MessageBoxImage.None);

            e.Cancel = result != MessageBoxResult.OK;
        }
    }
}
