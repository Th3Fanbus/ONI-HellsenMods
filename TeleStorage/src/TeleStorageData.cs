using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TeleStorage
{
	public class StoredItem
	{
		[JsonProperty]
		public float mass = 0.0f;

		[JsonProperty]
		public float temperature = 273.15f;

		[JsonProperty]
		public byte diseaseIdx = byte.MaxValue;

		[JsonProperty]
		public int diseaseCount = 0;
	}

	public class TeleStorageData
	{
		internal class SaveData
		{
			[JsonProperty]
			public Dictionary<SimHashes, StoredItem> storedLiquids = [];

			[JsonProperty]
			public Dictionary<SimHashes, StoredItem> storedGases = [];
		}

		private static TeleStorageData? instance = null;
		private static readonly object _lock = new();

		//private readonly static ConcurrentDictionary<SimHashes, object> locks = new(Enum
		//    .GetValues(typeof(SimHashes))
		//    .Cast<SimHashes>()
		//    .Select<SimHashes, KeyValuePair<SimHashes, object>>(h => new(h, new())));

		[JsonIgnore]
		public ConcurrentDictionary<SimHashes, StoredItem> storedLiquids = new();

		[JsonIgnore]
		public ConcurrentDictionary<SimHashes, StoredItem> storedGases = new();

		[JsonIgnore]
		public List<TeleStorage> storageContainers = [];

		public void FireRefresh()
		{
			foreach (TeleStorage storageContainer in storageContainers) {
				storageContainer.FireRefresh();
			}
		}

		public TeleStorageData()
		{
		}

		public static ConcurrentDictionary<SimHashes, StoredItem> GetStoredLiquids() => instance?.storedLiquids ?? new();

		public static ConcurrentDictionary<SimHashes, StoredItem> GetStoredGases() => instance?.storedGases ?? new();

		private static ConcurrentDictionary<SimHashes, StoredItem> GetStoredInvalid(ConduitType type)
		{
			Debug.LogWarning($"HELL: Invalid ConduitType {type}, returning throwaway dict");
			return new();
		}

		public static ConcurrentDictionary<SimHashes, StoredItem> GetStoredElements(ConduitType type) => type switch {
			ConduitType.Gas => GetStoredGases(),
			ConduitType.Liquid => GetStoredLiquids(),
			_ => GetStoredInvalid(type),
		};

		public static StoredItem? GetStored(ConduitType type, SimHashes element) => GetStoredElements(type).TryGetValue(element, out StoredItem value) ? value : null;

		public static StoredItem GetOrAddStored(ConduitType type, SimHashes element) => GetStoredElements(type).GetOrAdd(element, new StoredItem());

		public static void AddOrUpdateStored(ConduitType type, SimHashes element, Func<SimHashes, StoredItem, StoredItem> func)
		{
			ConcurrentDictionary<SimHashes, StoredItem> dict = GetStoredElements(type);
			dict.TryAdd(element, new StoredItem()); // TODO: Figure out a better way to go about this
			dict.AddOrUpdate(element, new StoredItem(), func);
		}

		public static TeleStorageData Instance {
			get {
				lock (_lock) {
					if (instance is null) {
						Debug.Log("HELL: Attempting to access uninitialized TeleStorageData. Using default values instead.");
						instance = new();
					}
					return instance;
				}
			}
		}

		public static void Load(string filename)
		{
			SaveData data = ConfigManager.LoadConfig<SaveData>(Assembly.GetExecutingAssembly().Location, "saved", Path.GetFileName(filename));
			lock (_lock) {
				if (instance is not null) {
					Debug.LogWarning($"HELL: instance isn't null");
				}
				instance = null;
				instance = new() {
					storedLiquids = new(data.storedLiquids.Where(kvp => IsLiquid(kvp.Key))),
					storedGases = new(data.storedGases.Where(kvp => IsGas(kvp.Key))),
				};
			}
		}

		private static bool IsLiquid(SimHashes element) => ElementLoader.FindElementByHash(element)?.IsLiquid ?? false;

		private static bool IsGas(SimHashes element) => ElementLoader.FindElementByHash(element)?.IsGas ?? false;

		public static void Save(string filename)
		{
			if (instance is null) {
				return;
			}
			SaveData data = new() {
				storedLiquids = instance.storedLiquids.ToArray().Where(kvp => IsLiquid(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
				storedGases = instance.storedGases.ToArray().Where(kvp => IsGas(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
			};
			ConfigManager.SaveConfig(data, Assembly.GetExecutingAssembly().Location, "saved", Path.GetFileName(filename));
		}
	}
}
