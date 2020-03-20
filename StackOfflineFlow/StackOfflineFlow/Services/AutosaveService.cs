using Newtonsoft.Json;
using StackOfflineFlow.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StackOfflineFlow.Services
{
    public static class AutosaveService
    {
        private static string _AutosaveFilePath = "autosave.json";

        public static AppSettings GetAppSettings()
        {
            if(!File.Exists(_AutosaveFilePath))
            {
                return new AppSettings();
            }
            else
            {
                var autosaveText = File.ReadAllText(_AutosaveFilePath);
                var deserializedAutosave = JsonConvert.DeserializeObject<AppSettings>(autosaveText);
                return deserializedAutosave;
            }
        }

        public static void SaveAppSettings(AppSettings settings)
        {
            var serializedText = JsonConvert.SerializeObject(settings);
            File.WriteAllText(_AutosaveFilePath, serializedText);
        }
    }
}
