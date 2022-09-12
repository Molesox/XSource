using Configuration;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSource.Helpers;

namespace XSource.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INavigationAware
    {

        #region Properties

        /// <summary>
        /// Gets the navigation service from the view.
        /// </summary>
        private INavigationService NavigationService => this.GetService<INavigationService>();

        /// <summary>
        /// Gets or sets the appsettings.
        /// </summary>
        public AppSettings AppSettings
        {
            get => GetProperty(() => AppSettings);
            set => SetProperty(() => AppSettings, value);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Standard initialization routine.
        /// </summary>
        protected override void OnInitializeInRuntime()
        {
            AppSettings = AppSettingsHelper.GetAppSettings();
            base.OnInitializeInRuntime();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Triggered on settings buton click. Navigates to the settings view.
        /// </summary>
        [Command]
        public void OnSettingsClick()
        {
            NavigationService.Navigate("SettingsView", AppSettings, this, true);
        }

        [Command]
        public void OnViewLoadedCommand()
        {
            NavigationService.Navigate("MainView", AppSettings, this, saveToJournal: true);
        }

        public void NavigatedTo(NavigationEventArgs e)
        {
        }

        public void NavigatingFrom(NavigatingEventArgs e)
        {
        }

        public void NavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion
    }
}
