using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RexLib
{
	public static partial class Extensions
	{
		/* Reason: null check cannot be simplified because Unity is cursed (overrides equals) */
#pragma warning disable IDE0029
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetValid<T>(this T? obj) where T : class? => obj == null ? null : obj;
#pragma warning restore IDE0029

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNull(this UnityEngine.Object? obj) => obj == null;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValid(this UnityEngine.Object? obj) => !IsNull(obj);

		public static bool IsNull(this WorkerBase? worker) => worker == null || worker.HasTag(GameTags.Dying) || worker.HasTag(GameTags.Dead);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsValid(this WorkerBase? worker) => !IsNull(worker);

		/* When this function returns true, the annotated output parameter is not null */
		public static bool TryGetDef<DefType>(this GameObject go, [NotNullWhen(true)] out DefType? def) where DefType : StateMachine.BaseDef
			=> (def = go.TryGetComponent(out StateMachineController smc) ? smc.GetDef<DefType>() : null) != null;
	}

	public static class RexUtils
	{
		public static string AssemblyName => Assembly.GetExecutingAssembly().GetName().Name;
		public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] FindObjectsOfType<T>(FindObjectsSortMode mode = FindObjectsSortMode.None) where T : UnityEngine.Object => UnityEngine.Object.FindObjectsByType<T>(mode);

		public static IEnumerable<MethodBase> MethodFromTypes(string method_name, params Type?[] targets)
			=> targets.Select(tgt => AccessTools.Method(tgt, method_name)).Where(method => method is not null);

		public static HashedString GetCategoryForBuilding(string building_id, HashedString fallback_category)
		{
			try {
				return TUNING.BUILDINGS.PLANORDER.First(info => info.buildingAndSubcategoryData.Exists(pair => pair.Key == building_id)).category;
			} catch (InvalidOperationException) {
				return fallback_category;
			}
		}

		public static void AddBuildingToPlanScreenBehindNext(HashedString fallback_category, string building_id, string other_building_id)
		{
			HashedString category = GetCategoryForBuilding(other_building_id, fallback_category);
			if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.TryGetValue(other_building_id, out string subcategory_id)) {
				TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[building_id] = subcategory_id;
				ModUtil.AddBuildingToPlanScreen(category, building_id, subcategory_id, other_building_id);
			} else {
				ModUtil.AddBuildingToPlanScreen(category, building_id);
			}
		}

		private static Texture2D LoadTexture(string texture_path)
		{
			string full_path = Path.Combine(ModPath, texture_path);
			Texture2D texture = new(1, 1);
			if (File.Exists(full_path)) {
				Debug.Log($"Found texture at: {full_path}");
				texture.LoadImage(File.ReadAllBytes(full_path));
			} else {
				Debug.LogWarning($"Could not find texture at: {full_path}");
			}
			return texture;
		}

		public static void RegisterSprite(string name)
		{
			Texture2D texture = LoadTexture($"assets/sprites/{name}.png");
			Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero);
			Assets.Sprites.Add((HashedString)name, sprite);
		}
	}
}
