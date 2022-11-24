using Configuration;
using DeepL;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XSource.Domain;
using XSource.Helpers;
using XSource.Services;

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
        /// Gets or sets a flag indicating if deepl can be used or not.
        /// </summary>
        public bool IsTradServiceOk
        {
            get => GetProperty(() => IsTradServiceOk);
            set => SetProperty(() => IsTradServiceOk, value);
        }

        long _charLimit { get; set; }
        long _charCount { get; set; }

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

        /// <summary>
        /// Gets or sets the deepl translator.
        /// </summary>
        public Translator DeeplTranslator { get; set; }

        #endregion

        protected override async void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            
            DeeplTranslator = new Translator(ConfigurationManager.AppSettings.Get("DeeplAPI"));

            var usage = await DeeplTranslator.GetUsageAsync();
            IsTradServiceOk = !usage.AnyLimitReached;
            _charLimit = usage.Character.Limit;
            _charCount = usage.Character.Count;

            Messenger.Default.Send<float>((_charCount ) / (float)_charLimit);

        }

        #region Commands & Events

        /// <summary>
        /// On combobox selection changed event. Updates the current item according to the new project.
        /// </summary>
        /// <param name="args"></param>
        public void ProjectChanged(XProject args)
        {
            CurrentItem.Project = CurrentProject?.Name;
            CurrentItem.ParentProject = CurrentProject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetLang"></param>
        [Command]
        public async void GetTranslationFor(string targetLang)
        {
            XWaitIndicator.Show(100);

            var (srcLang, text) = CurrentItem.GetFirstNonEmptyLanguageValue();

            if (!string.IsNullOrWhiteSpace(text))
            {
                var result = await DeeplTranslator.TranslateTextAsync(text, srcLang, targetLang);
                CurrentItem.SetVal(targetLang, result.Text);
                _charCount += text.Length;
                Messenger.Default.Send<float>((_charCount ) / (float)_charLimit);

            }
            XWaitIndicator.Close();

        }

        /// <summary>
        /// Overwrites the resource. And navigates back.
        /// </summary>
        [Command]
        public void Overwrite()
        {
            XWaitIndicator.Show(500);
            if (IsNewMode)
            {
                var type = CurrentItem.Type;
                var filePaths = Types.First(t => t.Type == type).FilePaths;
                CurrentItem.FilePath = filePaths;
                XHelper.OverwriteResource(CurrentItem);
                XWaitIndicator.Close();

                NavigationService.Navigate("MainView", CurrentItem);
            }
            else
            {

                XHelper.OverwriteResource(CurrentItem);
                XWaitIndicator.Close();

                NavigationService.Navigate("MainView");
            }
        }

        /// <summary>
        /// Navigates back (MainView).
        /// </summary>
        [Command]
        public void NavigateBack()
        {
            NavigationService.Navigate("MainView");
        }

        #endregion

        #region INavigationAware implementation

        /// <summary>
        /// Triggered when EditView is displayed.
        /// </summary>
        /// <param name="e">NavigationEventArgs</param>
        public void NavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as dynamic;

            CurrentItem = param.CurrentItem;
            IsNewMode = CurrentItem.IsNew;
            Projects = param.Projects;
        
            CurrentProject = Projects.FirstOrDefault(p => p.Name == CurrentItem.Project);
            if (CurrentProject == null)
            {
                CurrentProject = Projects.FirstOrDefault();
                CurrentItem.Type = Types.FirstOrDefault()?.Type;
            }
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
