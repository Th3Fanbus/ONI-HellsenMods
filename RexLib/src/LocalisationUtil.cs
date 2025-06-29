using KMod;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RexLib
{
	public static class LocalisationUtil
	{
		/*
         * Code borrowed from the game, and adapted to be more convenient to use
         */
		#region Klei_Code
		private static void CreateModLocStringKeys(Type type, string? parentPath = null)
		{
			string prefix = parentPath is null ? $"STRINGS." : $"{parentPath}{type.Name}.";

			foreach (FieldInfo fieldInfo in type.GetFields(LocString.data_member_fields).Where(fi => fi.FieldType == typeof(LocString))) {
				string translationKey = prefix + fieldInfo.Name;
				if (!fieldInfo.IsStatic) {
					Debug.LogError($"LocString fields must be static, skipping '{translationKey}'");
					continue;
				}
				if (fieldInfo.GetValue(null) is not LocString locString) {
					Debug.LogError($"LocString value is null, skipping '{translationKey}'");
					continue;
				}
				locString.SetKey(translationKey);
				Strings.Add(translationKey, locString.text);
				fieldInfo.SetValue(null, locString);
			}

			foreach (Type subtype in type.GetNestedTypes(LocString.data_member_fields)) {
				CreateModLocStringKeys(subtype, prefix);
			}
		}
		#endregion Klei_Code

		/*
         * Code originally borrowed from Sgt_Imalas
         */
		#region Sgt_Imalas_Code
		private static void OverloadModStrings()
		{
			string? code = Localization.GetLocale()?.Code;
			if (code.IsNullOrWhiteSpace())
				return;

			string path = Path.Combine(RexUtils.ModPath, "translations", code + ".po");
			if (File.Exists(path)) {
				Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
				Debug.Log($"Found translation file for {code}.");
			}
		}

		public static void Translate(Type root, bool generateTemplate = true)
		{
			Localization.RegisterForTranslation(root);
			OverloadModStrings();
			CreateModLocStringKeys(root);

			if (generateTemplate) {
				Localization.GenerateStringsTemplate(root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
			}
		}
		#endregion Sgt_Imalas_Code
	}
}