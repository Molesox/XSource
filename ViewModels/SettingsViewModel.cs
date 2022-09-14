﻿using Configuration;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.WindowsUI.Navigation;
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

        #region Commands

        [Command]
        public void NavigateBack()
        {
            NavigationService.GoBack();
        }

        [Command]
        public void SaveAppSettings()
        {
            AppSettingsHelper.WriteAppSettings(AppSettings);
            NavigationService.GoBack(AppSettings);
        }

        [Command]
        public void ValidateRow(RowValidationArgs args)
        {
            args.Result = GetValidationErrorInfo((ProjectConfig)args.Item);
        }
        static ValidationErrorInfo GetValidationErrorInfo(ProjectConfig task)
        {
           return new ValidationErrorInfo("Please, the name and the file path must be filled in!");
        }
            [Command]
        public void InvalidRow(InvalidRowExceptionArgs args)
        {
            args.ExceptionMode = ExceptionMode.NoAction;
        }

        #endregion

        #region INavigationAware interface implementation

        public void NavigatedFrom(NavigationEventArgs e)
        {
        }

        public void NavigatedTo(NavigationEventArgs e)
        {
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
