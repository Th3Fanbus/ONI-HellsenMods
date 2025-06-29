using Newtonsoft.Json;
using System;
using System.IO;

namespace TeleStorage
{
	public static class ConfigManager
	{
		private static string? GetConfigPath(string executingAssemblyPath, string folderName, string inConfigFileName)
		{
			string configFileName = Path.ChangeExtension(inConfigFileName, "json");
			string? directory = Path.GetDirectoryName(executingAssemblyPath);
			if (directory is null) {
				return null;
			}
			string fullDirectory = Path.Combine(directory, folderName);
			Directory.CreateDirectory(fullDirectory);
			return Path.Combine(fullDirectory, configFileName);
		}

		public static T LoadConfig<T>(string executingAssemblyPath, string folderName = "", string inConfigFileName = "Config.json") where T : notnull, new()
		{
			string? configPath = GetConfigPath(executingAssemblyPath, folderName, inConfigFileName);
			if (configPath == null) {
				Debug.LogWarning($"HELL: Could not read save data from provided executing assembly path: {executingAssemblyPath}. Using default values instead.");
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

		public static void SaveConfig<T>(T data, string executingAssemblyPath, string folderName = "", string inConfigFileName = "Config.json")
		{
			string? configPath = GetConfigPath(executingAssemblyPath, folderName, inConfigFileName);
			if (configPath == null) {
				Debug.LogWarning($"HELL: Could not save data to location from provided executing assembly path: {executingAssemblyPath}.");
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
