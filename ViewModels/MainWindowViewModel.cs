using Configuration;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI.Navigation;
using XSource.Helpers;

namespace XSource.ViewModels
{
    /// <summary>
    /// The main window view model class handling.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
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
        /// Triggered on settings button click. Navigates to the settings view.
        /// </summary>
        [Command]
        public void OnSettingsClick()
        {
            NavigationService.Navigate("SettingsView", AppSettings, this,false);
        }

        /// <summary>
        /// As soon as the window is loaded, the main view is displayed.
        /// </summary>
        [Command]
        public void OnViewLoadedCommand()
        {
            NavigationService.Navigate("MainView", AppSettings, this, false);
        }

        #endregion

    }
}
