using Configuration;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XSource.Domain;
using XSource.Helpers;

namespace XSource.ViewModels
{
    /// <summary>
    /// Helper class for holding the type and corresponding filepaths.
    /// </summary>
    public class TypesPaths : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type
        {
            get => GetProperty(() => Type);
            set => SetProperty(() => Type, value);
        }

        /// <summary>
        /// Gets or sets the file paths dictionnary.
        /// </summary>
        public Dictionary<string, string> FilePaths
        {
            get => GetProperty(() => FilePaths);
            set => SetProperty(() => FilePaths, value);
        }
    }

    /// <summary>
    /// The edit view model class handling.
    /// </summary>
    public class EditViewModel : ViewModelBase, INavigationAware
    {

        #region Properties

        /// <summary>
        /// Gets the navigation service from the view.
        /// </summary>
        private INavigationService NavigationService => this.GetService<INavigationService>();

        /// <summary>
        /// Gets or sets the current item.
        /// </summary>
        public XResource CurrentItem
        {
            get => GetProperty(() => CurrentItem);
            set => SetProperty(() => CurrentItem, value, () => RaisePropertyChanged(nameof(IsNewMode)));
        }

        /// <summary>
        /// A flag indicating if it's an edition or a new.
        /// </summary>
        public bool IsNewMode
        {
            get => GetProperty(() => IsNewMode);
            set => SetProperty(() => IsNewMode, value);
        }

        /// <summary>
        /// Gets or sets the list of projects.
        /// </summary>
        public BindingList<XProject> Projects
        {
            get => GetProperty(() => Projects);
            set => SetProperty(() => Projects, value);
        }

        /// <summary>
        /// Gets or sets the currentyl selected project.
        /// </summary>
        public XProject CurrentProject
        {
            get => GetProperty(() => CurrentProject);
            set => SetProperty(() => CurrentProject, value, () => RaisePropertyChanged(nameof(Types)));
        }

        /// <summary>
        /// Gets the list of TypesPaths according to the currently selected project.
        /// </summary>
        public IList<TypesPaths> Types
        {
            get
            {
                if (CurrentProject != null)
                {

                    return CurrentProject.Resources.GroupBy(r => r.Type)
                                           .Select(r => new TypesPaths() { Type = r.Key, FilePaths = r.First().FilePath })
                                           .ToList();
                }
                return null;
            }
        }



        #endregion



        #region Commands & Events

        /// <summary>
        /// On combobox selection changed event. Updates the current item according to the new project.
        /// </summary>
        /// <param name="args"></param>
        public void ProjectChanged(XProject args)
        {

            CurrentItem.Project = CurrentProject.Name;
            CurrentItem.ParentProject = CurrentProject;
            RaisePropertyChanged(nameof(IsFormValid));
            
        }

        public void Changed() => RaisePropertyChanged(nameof(IsFormValid));
        

        /// <summary>
        /// Overwrites the resource. And navigates back.
        /// </summary>
        [Command]
        public void Overwrite()
        {
            if (IsNewMode)
            {               
                    var type = CurrentItem.Type;
                    var filePaths = Types.First(t => t.Type == type).FilePaths;
                    CurrentItem.FilePath = filePaths;
                
            }
            XHelper.OverwriteResource(CurrentItem);
            NavigationService.GoBack(CurrentItem);

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

        /// <summary>
        /// Returns true iif all the fields are filled.
        /// </summary>
        /// <returns>a bool</returns>
        public bool IsFormValid
        {
            get => !string.IsNullOrWhiteSpace(CurrentItem?.Type) &&
                   !string.IsNullOrWhiteSpace(CurrentItem?.Key) &&
                   !string.IsNullOrWhiteSpace(CurrentItem?.Project);
        }

    /// <summary>
    /// Navigates back (MainView).
    /// </summary>
    [Command]
        public void NavigateBack()
        {
            NavigationService.GoBack();
        }

        #endregion

        #region INavigationAware implementation

        public void NavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as dynamic;

            CurrentItem = param.CurrentItem;
            IsNewMode = CurrentItem.IsNew;
            Projects = param.Projects;
        }

        public void NavigatingFrom(NavigatingEventArgs e)
        {
            if ((string)e.Source == "SettingsView")
                e.Cancel = true;

        }

        public void NavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion
    }
}
