using DevExpress.Xpf.Ribbon.Customization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Resources;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Configuration
{

    /// <summary>
    /// The app setting model class.
    /// </summary>
    public class AppSettings
    {

        public AppSettings()
        {
            ProjectConfigurations = new BindingList<ProjectConfig>();
            ProjectConfigurations.RaiseListChangedEvents = true;
        }
        /// <summary>
        /// A ctor. Inits the ProjectConfig.
        /// </summary>
        public AppSettings(Action onDirty, AppSettings appSettings)
        {
            var addHandler = new AddingNewEventHandler((object source, AddingNewEventArgs args) => onDirty());
            var changedHandler = new ListChangedEventHandler((object source, ListChangedEventArgs args) => onDirty());

            ProjectConfigurations = new BindingList<ProjectConfig>(appSettings.ProjectConfigurations);
            ProjectConfigurations.ListChanged += changedHandler;
            ProjectConfigurations.AddingNew += addHandler;
            ProjectConfigurations.RaiseListChangedEvents = true;
        }

        /// <summary>
        /// The list of Project configs.
        /// </summary>
        public BindingList<ProjectConfig> ProjectConfigurations { get; }
    }
}
