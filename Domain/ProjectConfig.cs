using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Configuration
{
    public class ProjectConfig
    {
        [Required]
        public string ProjectName
        {
            get; set;
        }
        [Required]
        public string ProjectPath
        {
            get;
            set;
        }
        [Required]
        public bool IsActive
        {
            get;
            set;
        }
    }
}
