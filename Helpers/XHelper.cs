using DevExpress.Mvvm.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using XSource.Domain;

namespace XSource.Helpers
{
    /// <summary>
    /// A static helper class.
    /// </summary>
    public static class XHelper
    {
        /// <summary>
        /// The loger
        /// </summary>
        public static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("Xlogger");

        /// <summary>
        /// Overwrite a resource in all languages.
        /// </summary>
        /// <param name="xsource">The resource to overwrite.</param>
        public static void OverwriteResource(XResource xsource)
        {
            AddOrUpdateResource(xsource.Key, xsource.En_val, xsource.FilePath[Languages.EN]);
            AddOrUpdateResource(xsource.Key, xsource.Fr_val, xsource.FilePath[Languages.FR]);
            AddOrUpdateResource(xsource.Key, xsource.De_val, xsource.FilePath[Languages.DE]);
            AddOrUpdateResource(xsource.Key, xsource.It_val, xsource.FilePath[Languages.IT]);
        }

        /// <summary>
        /// Adds or updates a resource key/value entry given a file path.
        /// </summary>
        /// <param name="key">the key of the resource</param>
        /// <param name="value">the value of the resource</param>
        /// <param name="path">the path of the file</param>
        public static void AddOrUpdateResource(string key, string value, string path)
        {
            var resx = new List<DictionaryEntry>();
            using (var reader = new ResXResourceReader(path))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();
                var existingResource = resx.FirstOrDefault(r => r.Key.ToString() == key);
                if (existingResource.Key == null && existingResource.Value == null) // new!
                {
                    resx.Add(new DictionaryEntry() { Key = key, Value = value });
                }
                else
                {
                    var modifiedResx = new DictionaryEntry() { Key = existingResource.Key, Value = value };
                    resx.Remove(existingResource);
                    resx.Add(modifiedResx);
                }
            }
            try
            {

            using (var writer = new ResXResourceWriter(path))
            {
                resx.ForEach(r =>
                {
                    writer.AddResource(r.Key.ToString(), r.Value?.ToString() ?? "");
                });
                writer.Generate();
            }
            }catch(Exception e)
            {
                Logger.Error(e, nameof(XHelper.AddOrUpdateResource));
            }
        }

        /// <summary>
        /// Deletes a resource from all files.
        /// </summary>
        /// <param name="resx">The resource to delete.</param>
        public static void DeleteResource(XResource resx)
        {
            DeleteResource(resx.Key, resx.En_val, resx.FilePath[Languages.EN]);
            DeleteResource(resx.Key, resx.Fr_val, resx.FilePath[Languages.FR]);
            DeleteResource(resx.Key, resx.De_val, resx.FilePath[Languages.DE]);
            DeleteResource(resx.Key, resx.It_val, resx.FilePath[Languages.IT]);
        }

        /// <summary>
        /// Deletes a resource key/value entry given a file path.
        /// </summary>
        /// <param name="key">the key of the resource</param>
        /// <param name="value">the value of the resource</param>
        /// <param name="path">the path of the file</param>
        public static void DeleteResource(string key, string value, string path)
        {
            var resx = new List<DictionaryEntry>();
            using (var reader = new ResXResourceReader(path))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();
                resx.Remove(resx.First(r => r.Key.ToString() == key));
            }
            try
            {

                using (var writer = new ResXResourceWriter(path))
                {
                    resx.ForEach(r =>
                    {
                        writer.AddResource(r.Key.ToString(), r.Value?.ToString() ?? "");
                    });
                    writer.Generate();
                }
            }catch(Exception e)
            {
                Logger.Error(e, nameof(XHelper.DeleteResource));
            }
        }

        /// <summary>
        /// Build the XRpoject instance given a project name and filepath. All the
        /// *.resx from the path are extracted.
        /// </summary>
        /// <param name="projName">the project name to use.</param>
        /// <param name="projPath">the filepath containing the resources files .resx.</param>
        /// <returns>A XProject instance.</returns>
        public static XProject LoadProject(string projName, string projPath)
        {

            var result = new XProject()
            {
                Name = projName,
                Path = projPath,
                Resources = new BindingList<XResource>()
            };

            if (!Directory.Exists(projPath))
            {
                Logger.Warn($"The directory {projPath} does not exist.");
                return null;
            }

            try
            {
                var temp = new Dictionary<string,Tuple<string,string>>();
                var fileNames = Directory.EnumerateFiles(projPath, "*.resx");

                foreach (var file in fileNames.OrderBy(n => n.Length, ListSortDirection.Ascending))
                {
                    var fileName = Path.GetFileNameWithoutExtension(file).Split('.');

                    var fileType = fileName[0];
                    var language = "en";

                    if (fileName.Length > 1)
                        language = fileName[1];

                    using (var resx = new ResXResourceReader(file))
                    {
                        foreach (DictionaryEntry keyVal in resx)
                        {
                            var key = (string)keyVal.Key;
                            var value = (string)keyVal.Value;

                            if (string.IsNullOrWhiteSpace(value))
                                value = "";

                            var existingResource = result.Resources
                                                                 .FirstOrDefault(r => r.Key == key && r.Type == fileType);
                            if (existingResource != null)
                                existingResource.SetVal(language, value);

                            else
                            {
                                var resource = new XResource()
                                {
                                    ParentProject = result,
                                    Project = projName,
                                    Type = fileType,
                                    Key = key,
                                    FilePath = new Dictionary<string, string>()
                                };

                                result.Resources.Add(resource.SetVal(language, value));
                            }
                        }
                    }
                    temp.Add(file, new Tuple<string, string>(language, fileType));
                }
                foreach (var lookup in temp)
                {
                    result.Resources.Where(r => r.Type == lookup.Value.Item2)
                        .ForEach(res => res.FilePath.Add(lookup.Value.Item1, lookup.Key));
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, nameof(XHelper.LoadProject));
            }

            return result;
        }
    }
}