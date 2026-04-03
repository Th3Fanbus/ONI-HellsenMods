using KSerialization;
using Newtonsoft.Json;
using RexLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TeleStorage
{
	public struct StoredItem
	{
		public float mass = 0.0f;
		public float temperature = 273.15f;
		public byte diseaseIdx = byte.MaxValue;
		public int diseaseCount = 0;

		public StoredItem()
		{
		}
	}

	public struct SaveData
	{
		public Dictionary<SimHashes, StoredItem> storedGases = [];
		public Dictionary<SimHashes, StoredItem> storedLiquids = [];
		public Dictionary<SimHashes, StoredItem> storedSolids = [];

		public SaveData()
		{
		}
	}

	[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
	public class TeleStorageData : KMonoBehaviour
	{
		private static TeleStorageData? instance = null;
		public static TeleStorageData? Instance {
			get {
				if (instance == null) {
					Debug.LogError("TeleStorageData: instance is null!");
				}
				return instance.GetValid();
			}
		}

		private ConcurrentDictionary<SimHashes, StoredItem> storedGases = new();
		private ConcurrentDictionary<SimHashes, StoredItem> storedLiquids = new();
		private ConcurrentDictionary<SimHashes, StoredItem> storedSolids = new();

		public List<TeleStorage> storageContainers = [];

		private SaveData MySaveData {
			get => new() {
				storedGases = TeleStorageUtils.FilterByType(storedGases, TeleStorageUtils.IsGas),
				storedLiquids = TeleStorageUtils.FilterByType(storedLiquids, TeleStorageUtils.IsLiquid),
				storedSolids = TeleStorageUtils.FilterByType(storedSolids, TeleStorageUtils.IsSolid),
			};
			set {
				storedGases = new(value.storedGases.Where(TeleStorageUtils.IsGas));
				storedLiquids = new(value.storedLiquids.Where(TeleStorageUtils.IsLiquid));
				storedSolids = new(value.storedSolids.Where(TeleStorageUtils.IsSolid));
			}
		}

		[Serialize]
		public int dataVersion = 0;

		[Serialize]
		public string SerialisedData {
			get {
				try {
					Debug.Log($"HELL: Serialising save data!");
					return JsonConvert.SerializeObject(MySaveData);
				} catch (Exception ex) {
					Debug.LogWarning($"HELL: Could not serialise save data: {ex}");
					return "";
				}
			}
			set {
				try {
					Debug.Log($"HELL: Deserialising save data!");
					MySaveData = JsonConvert.DeserializeObject<SaveData>(value);
				} catch (Exception ex) {
					Debug.LogWarning($"HELL: Could not deserialise save data: {ex}");
				}
			}
		}

		public override void OnPrefabInit()
		{
			instance = this;

			GameObject childGo = new("Th3Fanbus_TeleStorage_SaveData");
			DontDestroyOnLoad(childGo);
			childGo.SetActive(true);

			Debug.Log($"HELL: Initialising instance");
		}

		public override void OnSpawn()
		{
			Debug.Log($"HELL: Spawning instance, dataVersion: {dataVersion}");
			if (dataVersion == 0) {
				MySaveData = LegacyConfigManager.LoadConfig<SaveData>(SaveLoader.GetActiveSaveFilePath());
				dataVersion++;
			}
		}

		public override void OnCleanUp()
		{
			Debug.Log($"HELL: Removing instance");
			if (instance == this) {
				instance = null;
			}
		}

		public void FireRefresh()
		{
			foreach (TeleStorage storageContainer in storageContainers) {
				storageContainer.FireRefresh();
			}
		}

		private static ConcurrentDictionary<SimHashes, StoredItem> GetStoredInvalid(ConduitType type)
		{
			Debug.LogWarning($"HELL: Invalid ConduitType {type}, returning throwaway dict");
			return new();
		}

		public ConcurrentDictionary<SimHashes, StoredItem> GetStoredElements(ConduitType type) => type switch {
			ConduitType.Gas => storedGases,
			ConduitType.Liquid => storedLiquids,
			ConduitType.Solid=> storedSolids,
			_ => GetStoredInvalid(type),
		};

		public StoredItem? GetStored(ConduitType type, SimHashes element) => GetStoredElements(type).TryGetValue(element, out StoredItem value) ? value : null;

		public StoredItem GetOrAddStored(ConduitType type, SimHashes element) => GetStoredElements(type).GetOrAdd(element, new StoredItem());

		public void AddOrUpdateStored(ConduitType type, SimHashes element, Func<SimHashes, StoredItem, StoredItem> func)
		{
			ConcurrentDictionary<SimHashes, StoredItem> dict = GetStoredElements(type);
			dict.TryAdd(element, new StoredItem()); // TODO: Figure out a better way to go about this
			dict.AddOrUpdate(element, new StoredItem(), func);
		}
	}
}
