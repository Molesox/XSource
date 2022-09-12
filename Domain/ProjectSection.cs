using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Configuration
{

    public class AppSettings
    {
        public AppSettings()
        {
            ProjectConfigurations = new BindingList<ProjectConfig>();
        }

        public BindingList<ProjectConfig> ProjectConfigurations { get; set; }
    }
}
