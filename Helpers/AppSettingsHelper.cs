using Configuration;
using System;
using System.IO;
using System.Text.Json;

namespace XSource.Helpers
{
    /// <summary>
    /// A static helper class for AppSettings.
    /// </summary>
    public static class AppSettingsHelper
    {
        /// <summary>
        /// The settings file path
        /// </summary>
        public static readonly string FilePath = @".\Settings.json";

        /// <summary>
        /// Gets the appsettings as an AppSettings instance.
        /// </summary>
        /// <returns>An AppSettings instance</returns>
        public static AppSettings GetAppSettings()
        {
            var result = default(AppSettings);
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string json = reader.ReadToEnd();
                    result = JsonSerializer.Deserialize<AppSettings>(json);
                }
            }
            catch (Exception e)
            {
                XHelper.Logger.Error(e, nameof(AppSettingsHelper.GetAppSettings));
            }

            return result;
        }

        /// <summary>
        /// Writes an AppSettings instance in Settings.json file.
        /// </summary>
        /// <param name="appSettings"></param>
        public static void WriteAppSettings(AppSettings appSettings)
        {
            string json = JsonSerializer.Serialize(appSettings);
            try
            {
                using (var writer = new StreamWriter(FilePath))
                    writer.Write(json);
            }
            catch (Exception e)
            {
                XHelper.Logger.Error(e, nameof(AppSettingsHelper.WriteAppSettings));
            }
        }
    }
}
