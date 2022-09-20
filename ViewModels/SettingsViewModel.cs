using Configuration;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI.Navigation;
using System.Linq;
using System.Threading.Tasks;
using XSource.Domain;
using XSource.Helpers;
using XSource.Services;

namespace XSource.ViewModels
{
    /// <summary>
    /// The Settings view model class handling.
    /// </summary>
    public class SettingsViewModel : ViewModelBase, INavigationAware
    {
        #region Properties

        /// <summary>
        /// Gets or sets the validation row delegate command.
        /// </summary>
        public DelegateCommand<RowValidationArgs> ValidateRowCommand { get; private set; }

        /// <summary>
        /// Gets the grid service from the view.
        /// </summary>
        private IGridService GridService => GetService<IGridService>();

        /// <summary>
        /// Gets the navigations service.
        /// </summary>
        private INavigationService NavigationService => this.GetService<INavigationService>();

        /// <summary>
        /// Gets or sets the AppSettings
        /// </summary>
        public AppSettings AppSettings
        {
            get => GetProperty(() => AppSettings);
            set => SetProperty(() => AppSettings, value);
        }


        #endregion

        #region Overrides

        protected override void OnInitializeInRuntime()
        {
            ValidateRowCommand = new DelegateCommand<RowValidationArgs> (ValidateRow);
            base.OnInitializeInRuntime();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Validates a row.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public void ValidateRow(RowValidationArgs args)
        {
            var newProjConfi = args.Item as ProjectConfig;

            var repetedNames = AppSettings.ProjectConfigurations.Where(proj => proj.ProjectName == newProjConfi.ProjectName).Count();
            if (repetedNames > 1)
            {
                args.Result = new ValidationErrorInfo("Le même nom de projet n'est pas authorisé !", ValidationErrorType.Default);
                return;
            }

            var projsWithSamePath = AppSettings.ProjectConfigurations.Where(proj => proj.ProjectPath == newProjConfi.ProjectPath);

            var otherProj = projsWithSamePath.FirstOrDefault(proj => proj.ProjectName != newProjConfi.ProjectName);

            if (projsWithSamePath.Count() > 1)
            {
                args.Result = new ValidationErrorInfo($"Ce chemin d'accès est déjà défini pour le projet {otherProj.ProjectName}", ValidationErrorType.Default);
                return;
            }        

        }

        [Command]
        public void NavigateBack()
        {
            NavigationService.Navigate("MainView");
        }

        [Command]
        public void SaveAppSettings()
        {
            AppSettingsHelper.WriteAppSettings(AppSettings);
            NavigationService.Navigate("MainView", AppSettings);
        }

        #endregion

        #region INavigationAware interface implementation

        public void NavigatedFrom(NavigationEventArgs e)
        {
          
        }

        public void NavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
                return;
            var temp = e.Parameter as AppSettings;
            AppSettings = new AppSettings()
            {
                ProjectConfigurations = new System.ComponentModel.BindingList<ProjectConfig>(),
            };

            foreach (var conf in temp.ProjectConfigurations)
            {
                AppSettings.ProjectConfigurations.Add(new ProjectConfig()
                {
                    IsActive = conf.IsActive,
                    ProjectName = conf.ProjectName,
                    ProjectPath = conf.ProjectPath,
                });
            }
        }

        public void NavigatingFrom(NavigatingEventArgs e)
        {
        }

        #endregion
    }
}
