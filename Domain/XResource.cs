using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSource.Domain
{
    /// <summary>
    /// Language constants
    /// </summary>
    public static class Languages
    {
        public const string FR = "fr";
        public const string EN = "en";
        public const string DE = "de";
        public const string IT = "it";
    }

    /// <summary>
    /// The project entity class handling.
    /// </summary>
    public class XProject : ViewModelBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the project name.
        /// ex: Framework
        /// </summary>
        public string Name
        {
            get => GetProperty(() => Name);
            set => SetProperty(() => Name, value);
        }

        /// <summary>
        /// Gets or sets the project path.
        /// </summary>
        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        /// <summary>
        /// Gets or sets the collection of resources.
        /// </summary>
        public BindingList<XResource> Resources
        {
            get => GetProperty(() => Resources);
            set => SetProperty(() => Resources, value);
        }

        #endregion
    }

    /// <summary>
    /// The resource entity class handling.
    /// </summary>
    public class XResource : ViewModelBase, IDataErrorInfo
    {

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Key":
                        return string.IsNullOrEmpty(Key) ? "La clef d'une resource est requise !" : null;
                    case "Project":
                        return string.IsNullOrEmpty(Project) ? "Une resource appartient à un projet !" : null;
                    case "Type":
                        return string.IsNullOrEmpty(Type) ? "Il faut selectionner un type!" : null;

                    default:
                        return null;
                }
            }
        }


        public string Error
        {
            get
            {
                return this["Key"] != null ? "Hélas, on n'est pas ok, faut corriger les valeurs renseignées et après on en reparle." : null;
            }
        }


        #region Properies

        /// <summary>
        /// Gets or sets a flag indicating if it's a new instance.
        /// </summary>
        public bool IsNew
        {
            get => GetProperty(() => IsNew);
            set => SetProperty(() => IsNew, value);
        }

        /// <summary>
        /// Gets or sets the Project.
        /// </summary>
        public string Project
        {
            get => GetProperty(() => Project);
            set => SetProperty(() => Project, value);
        }

        /// <summary>
        /// Gets or sets the file path
        /// </summary>
        public Dictionary<string, string> FilePath
        {
            get => GetProperty(() => FilePath);
            set => SetProperty(() => FilePath, value);
        }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public string Type
        {
            get => GetProperty(() => Type);
            set => SetProperty(() => Type, value);
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key
        {
            get => GetProperty(() => Key);
            set => SetProperty(() => Key, value);
        }

        /// <summary>
        /// Gets or sets the en_val.
        /// </summary>
        public string En_val
        {
            get => GetProperty(() => En_val);
            set => SetProperty(() => En_val, value);
        }

        /// <summary>
        /// Gets or sets the fr_val.
        /// </summary>
        public string Fr_val
        {
            get => GetProperty(() => Fr_val);
            set => SetProperty(() => Fr_val, value);
        }

        /// <summary>
        /// Gets or sets the de_val.
        /// </summary>
        public string De_val
        {
            get => GetProperty(() => De_val);
            set => SetProperty(() => De_val, value);
        }

        /// <summary>
        /// Gets or sets the it_val.
        /// </summary>
        public string It_val
        {
            get => GetProperty(() => It_val);
            set => SetProperty(() => It_val, value);
        }

        /// <summary>
        /// Gets or sets the parent project reference.
        /// </summary>
        public XProject ParentProject
        {
            get => GetProperty(() => ParentProject);
            set => SetProperty(() => ParentProject, value);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Sets the value according to the given language. 
        /// </summary>
        /// <param name="lang">the language to use.</param>
        /// <param name="value">the value to set.</param>
        /// <returns>the ref of the xresource.</returns>
        public XResource SetVal(string lang, string value)
        {
            lang = lang.ToLower().Trim();
            switch (lang)
            {
                case Languages.FR:
                    Fr_val = value;
                    break;

                case Languages.DE:
                    De_val = value;
                    break;

                case Languages.EN:
                    En_val = value;
                    break;

                case Languages.IT:
                    It_val = value;
                    break;

                default:
                    break;
            }
            return this;
        }

        #endregion
    }
}
