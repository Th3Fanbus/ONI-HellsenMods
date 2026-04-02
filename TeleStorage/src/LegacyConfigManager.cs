using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace TeleStorage
{
	public static class LegacyConfigManager
	{
		public static readonly string SAVE_FOLDER_NAME = "saved";

		public static string? GetConfigPath(string saveName)
		{
			string configFileName = Path.ChangeExtension(Path.GetFileName(saveName), "json");
			string? directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (directory is null) {
				Debug.LogWarning($"HELL: Could not get config path from provided executing assembly path: {Assembly.GetExecutingAssembly().Location}");
				return null;
			}
			string fullDirectory = Path.Combine(directory, SAVE_FOLDER_NAME);
			Directory.CreateDirectory(fullDirectory);
			return Path.Combine(fullDirectory, configFileName);
		}

		public static T LoadConfig<T>(string saveName) where T : notnull, new()
		{
			string? configPath = GetConfigPath(saveName);
			if (configPath == null) {
				Debug.LogWarning($"HELL: Failed to load config, using default values instead");
				return new();
			}
			Debug.Log($"HELL: Attempt load from {configPath}");
			try {
				using StreamReader r = new(configPath);
				string json = r.ReadToEnd();
				return JsonConvert.DeserializeObject<T>(json);
			} catch (Exception ex) {
				Debug.LogWarning($"HELL: Could not read save data from config file: {ex}");
				return new();
			}
		}

		public static void SaveConfig<T>(string saveName, T data)
		{
			string? configPath = GetConfigPath(saveName);
			if (configPath == null) {
				Debug.LogWarning($"HELL: Failed to save config");
				return;
			}
			Debug.Log($"HELL: Attempt save to {configPath}");
			try {
				using StreamWriter w = new(configPath);
				w.Write(JsonConvert.SerializeObject(data));
			} catch (Exception ex) {
				Debug.LogWarning($"HELL: Could not save data to config file: {ex}");
			}
		}
	}
}
