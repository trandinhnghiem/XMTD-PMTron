using System;
using System.IO;
using System.Text.Json;

namespace QuanLyTram.Properties
{
    internal sealed class Settings
    {
        private static Settings defaultInstance = new Settings();
        public static Settings Default => defaultInstance;

        // Các biến lưu login
        public string SavedUsername { get; set; } = "";
        public string SavedPassword { get; set; } = "";
        public bool RememberMe { get; set; } = false;

        private string configFile = "user_config.json";

        // Lưu ra file JSON
        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFile, json);
        }

        // Load từ file JSON
        public void Load()
        {
            if (File.Exists(configFile))
            {
                var json = File.ReadAllText(configFile);
                var loaded = JsonSerializer.Deserialize<Settings>(json);
                if (loaded != null)
                {
                    SavedUsername = loaded.SavedUsername;
                    SavedPassword = loaded.SavedPassword;
                    RememberMe = loaded.RememberMe;
                }
            }
        }
    }
}
