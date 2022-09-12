using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace XSource.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {

            InitializeComponent();
        }

        private void TableView_Loaded(object sender, RoutedEventArgs e)
        {

            int groupRowHandle = -1;
            while (xResourcesGrid.IsValidRowHandle(groupRowHandle))
            {
                if (xResourcesGrid.GetRowLevelByRowHandle(groupRowHandle) <= 0)
                {
                    xResourcesGrid.ExpandGroupRow(groupRowHandle);
                }
                groupRowHandle--;
            }

        }

    }
}
