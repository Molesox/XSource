using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSource.Domain
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class PathExistsAttribute : ValidationAttribute
    {
        /// <summary>
        /// The property is valid iif the path exists.
        /// </summary>
        /// <param name="value">the path</param>
        /// <returns>true if valid, false otherwise.</returns>
        public override bool IsValid(object value)
        {

            //if (string.IsNullOrWhiteSpace((string)value))
            //    return true;

            if (value is string)
                return Directory.Exists((string)value);

            return false;
        }
    }
}
