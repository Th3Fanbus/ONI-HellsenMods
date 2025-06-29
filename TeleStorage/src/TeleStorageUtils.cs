using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TeleStorage
{
	public static class TeleStorageUtils
	{
		public static K GetKey<K, V>(KeyValuePair<K, V> kvp) => kvp.Key;
		public static V GetValue<K, V>(KeyValuePair<K, V> kvp) => kvp.Value;

		public static bool IsGas(SimHashes element) => ElementLoader.FindElementByHash(element)?.IsGas ?? false;
		public static bool IsLiquid(SimHashes element) => ElementLoader.FindElementByHash(element)?.IsLiquid ?? false;
		public static bool IsSolid(SimHashes element) => ElementLoader.FindElementByHash(element)?.IsSolid ?? false;

		public static bool IsGas<V>(KeyValuePair<SimHashes, V> kvp) => ElementLoader.FindElementByHash(kvp.Key)?.IsGas ?? false;
		public static bool IsLiquid<V>(KeyValuePair<SimHashes, V> kvp) => ElementLoader.FindElementByHash(kvp.Key)?.IsLiquid ?? false;
		public static bool IsSolid<V>(KeyValuePair<SimHashes, V> kvp) => ElementLoader.FindElementByHash(kvp.Key)?.IsSolid ?? false;

		public static Dictionary<K, V> FilterByType<K, V>(ConcurrentDictionary<K, V> input, System.Func<KeyValuePair<K, V>, bool> func)
			=> input.ToArray().Where(func).ToDictionary(GetKey, GetValue);

		public static Filterable.ElementState GetElementState(ConduitType type) => type switch {
			ConduitType.Gas => Filterable.ElementState.Gas,
			ConduitType.Liquid => Filterable.ElementState.Liquid,
			ConduitType.Solid => Filterable.ElementState.Solid,
			_ => Filterable.ElementState.None,
		};

		public static HashedString GetViewMode(ConduitType type) => type switch {
			ConduitType.Gas => OverlayModes.GasConduits.ID,
			ConduitType.Liquid => OverlayModes.LiquidConduits.ID,
			ConduitType.Solid => OverlayModes.SolidConveyor.ID,
			_ => OverlayModes.None.ID,
		};
	}
}
