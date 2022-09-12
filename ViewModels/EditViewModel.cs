using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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
    public class TypesPaths : ViewModelBase
    {
        public string Type
        {
            get => GetProperty(() => Type);
            set => SetProperty(() => Type, value);
        }

        public Dictionary<string, string> FilePaths
        {
            get => GetProperty(() => FilePaths);
            set => SetProperty(() => FilePaths, value);
        }
    }

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
        /// Gets or sets the list of projects
        /// </summary>
        public BindingList<XProject> Projects
        {
            get => GetProperty(() => Projects);
            set => SetProperty(() => Projects, value);
        }

        public XProject CurrentProject
        {
            get => GetProperty(() => CurrentProject);
            set => SetProperty(() => CurrentProject, value, () => RaisePropertyChanged(nameof(Types)));
        }

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

        public void ProjectChanged(XProject args)
        {

            CurrentItem.Project = CurrentProject.Name;
            CurrentItem.ParentProject = CurrentProject;
        }

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
            if((string)e.Source == "SettingsView")
                e.Cancel = true;

        }

        public void NavigatedFrom(NavigationEventArgs e)
        {
            // CurrentItem = null;
        }

        #endregion
    }
}
