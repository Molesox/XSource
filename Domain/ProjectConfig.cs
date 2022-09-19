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
using XSource.Domain;

namespace Configuration
{
    /// <summary>
    /// A Project configuration class model.
    /// </summary>
    public class ProjectConfig
    {
        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pis quoi encore ? Faut nommer les choses, voyons!")]
        public string ProjectName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Porject path.
        /// </summary>
        [PathExists(ErrorMessage = "Es-tu perdu ? car le chemin spécifié n'existe pas...")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Faut donner le path, pétole!")]
        public string ProjectPath
        {
            get;
            set;
        }

        /// <summary>
        /// Tells if the project is activ. I.e. the resources are displayed.
        /// </summary>
        [Required]
        public bool IsActive
        {
            get;
            set;
        }

    }
}
