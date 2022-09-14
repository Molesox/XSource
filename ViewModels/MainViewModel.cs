using Configuration;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.WindowsUI.Navigation;
using System;
using System.ComponentModel;
using System.Linq;
using XSource.Domain;
using XSource.Helpers;
using XSource.Services;

namespace XSource.ViewModels
{
    /// <summary>
    /// The main view model class handling.
    /// </summary>
    public class MainViewModel : ViewModelBase, INavigationAware
    {
        #region Ctor

        /// <summary>
        /// A ctor.
        /// </summary>
        public MainViewModel()
        { }


        #endregion

        #region Properties

        /// <summary>
        /// Gets the grid service from the view.
        /// </summary>
        private IGridService GridService => GetService<IGridService>();

        /// <summary>
        /// Gets the navigation service from the view.
        /// </summary>
        private INavigationService NavigationService => this.GetService<INavigationService>();

        /// <summary>
        /// Gets or sets the current selected item of the grid.
        /// </summary>
        public XResource CurrentItem
        {
            get => GetProperty(() => CurrentItem);
            set => SetProperty(() => CurrentItem, value);
        }

        /// <summary>
        /// Gets or sets the list of projects
        /// </summary>
        public BindingList<XProject> Projects
        {
            get => GetProperty(() => Projects);
            set => SetProperty(() => Projects, value);
        }

        /// <summary>
        /// Gets or sets the list of all the project resources aggregated as one big list.
        /// </summary>
        public BindingList<XResource> ItemsSource
        {
            get => GetProperty(() => ItemsSource);
            set => SetProperty(() => ItemsSource, value);
        }

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
        /// Standard initialize members routine.
        /// </summary>
        protected override void OnInitializeInRuntime()
        {
            Projects = new BindingList<XProject>();
            ItemsSource = new BindingList<XResource>();
            base.OnInitializeInRuntime();
        }

        #endregion

        #region Commands & Events

        /// <summary>
        /// Triggered for validating the deletion of a row. However, it just overwrittes the file 
        /// without the resource to delete.
        /// </summary>
        /// <param name="args">A ValidateRowDeletionArgs</param>
        [Command]
        public void ValidateRowDeletion(ValidateRowDeletionArgs args)
        {
            var toDelete = args.Items[0] as XResource;
            XHelper.DeleteResource(toDelete);
        }

        /// <summary>
        /// Navigates to the edit view with a new instance initialized.
        /// </summary>
        /// <param name="args">RowClickArgs</param>
        [Command]
        public void NewResource(RowClickArgs args)
        {
            var resx = new XResource()
            {
                Project = CurrentItem?.Project,
                Type = CurrentItem?.Type,
                ParentProject = CurrentItem?.ParentProject,
                IsNew = true,
            };

            NavigationService.Navigate("EditView", new { Projects, CurrentItem = resx }, this, true);
        }

        /// <summary>
        /// Navigates to the edit view.
        /// </summary>
        /// <param name="args">RowClickArgs</param>
        [Command]
        public void EditResource(RowClickArgs args)
        {
            NavigationService.Navigate("EditView", new { Projects, CurrentItem }, this, true);
        }

        /// <summary>
        /// If the resource is editable or not.
        /// </summary>
        /// <returns>true if CurrentItem is not null.</returns>
        public bool CanEditResource(RowClickArgs args) => CurrentItem != null;

        /// <summary>
        /// Refresh the data source.
        /// </summary>
        [Command]
        public void RefreshDataSource(DataSourceRefreshArgs args)
        {
            if (args != null)
                args.Handled = true;

            Projects.Clear();

            foreach (var projectConfig in AppSettings.ProjectConfigurations.Where(conf => conf.IsActive))
            {
                var proj = XHelper.LoadProject(projectConfig.ProjectName, projectConfig.ProjectPath);
                if (proj != null)
                    Projects.Add(proj);
            }

            ItemsSource?.Clear();

            foreach (var projet in Projects)
                foreach (XResource resx in projet.Resources)
                    ItemsSource.Add(resx);

            GridService.ExpandFirstLevel();
        }

        #endregion

        #region INavigationAware interface implementation

        /// <summary>
        /// If we get here from the settings view, we update the settings.
        /// If we get here from the EditView, we set the new item as currently selected.
        /// </summary>
        /// <param name="e">NavigationEventArgs</param>
        public void NavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter is AppSettings)
                {
                    AppSettings = e.Parameter as AppSettings;
                    RefreshDataSource(null);
                }
                else
                {
                    var newItem = e.Parameter as XResource;
                    newItem.IsNew = false;
                    ItemsSource.Add(newItem);
                    CurrentItem = newItem;
                }
            }
        }

        public void NavigatingFrom(NavigatingEventArgs e)
        {
            if ((string)e.Source == "SettingsView")
            {
                var appSettings = e.Parameter as AppSettings;

                appSettings.ProjectConfigurations = AppSettings.ProjectConfigurations;
            }
        }

        public void NavigatedFrom(NavigationEventArgs e)
        {

        }

        #endregion
    }
}
