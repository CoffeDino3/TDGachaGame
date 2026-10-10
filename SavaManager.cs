using System;
using System.IO;
using System.Text.Json;

namespace TDGachaGame
{
    public static class SaveManager
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public static string SaveFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TDGachaGame");

        public static string SavePath => Path.Combine(SaveFolder, "save.json");

        public static bool SaveExists => File.Exists(SavePath);
        public static PlayerProfile LoadOrCreate()
        {
            var profile = new PlayerProfile();

            if (!SaveExists)
            {
                return profile;
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                SaveData? data = JsonSerializer.Deserialize<SaveData>(json);

                if (data != null && profile.LoadFromSaveData(data))
                {
                    return profile;
                }
            }
            catch (Exception)
            {
            }

            try
            {
                File.Copy(SavePath, Path.Combine(SaveFolder, "save.broken.json"), true);
            }
            catch (Exception)
            {
            }

            return new PlayerProfile();
        }
        public static void Save(PlayerProfile profile)
        {
            try
            {
                Directory.CreateDirectory(SaveFolder);

                string json = JsonSerializer.Serialize(profile.ToSaveData(), Options);
                string temp = SavePath + ".tmp";

                File.WriteAllText(temp, json);
                File.Move(temp, SavePath, true);
            }
            catch (Exception)
            {
            }
        }

        public static void Delete()
        {
            try
            {
                if (SaveExists)
                {
                    File.Delete(SavePath);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}