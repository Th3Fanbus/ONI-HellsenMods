using HarmonyLib;
using KMod;
using RexLib;
using System.Collections.Generic;
using UnityEngine;

using static ComplexRecipe;

namespace HellsenWorldgen
{
    public static partial class Patches
    {
        [HarmonyPatch(typeof(Localization), nameof(Localization.Initialize))]
        public static class Localization_Initialize_Patch
        {
            public static void Postfix() => LocalisationUtil.Translate(typeof(MOD_STRINGS));
        }

        [HarmonyPatch(typeof(Mod), nameof(Mod.SetCrashed))]
        public static class Mod_SetCrashed_Patch
        {
            public static bool Prefix(Mod __instance)
            {
                __instance.SetCrashCount(__instance.crash_count + 1);
                return false;
            }
        }

        [HarmonyPatch(typeof(ReportErrorDialog), nameof(ReportErrorDialog.BuildModsList))]
        public static class DontDisableModOnCrash3
        {
            public static bool Prefix(ReportErrorDialog __instance)
            {
                DebugUtil.Assert(Global.Instance != null && Global.Instance.modManager != null);
                Manager? manager = Global.Instance.GetValid()?.modManager;
                if (manager is null) {
                    return false;
                }
                List<Mod> allCrashableMods = manager.GetAllCrashableMods();
                allCrashableMods.Sort((x, y) => y.foundInStackTrace.CompareTo(x.foundInStackTrace));
                foreach (Mod mod in allCrashableMods) {
                    HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(__instance.modEntryPrefab, __instance.modEntryParent.gameObject);
                    LocText reference = hierarchyReferences.GetReference<LocText>("Title");
                    reference.text = mod.title;
                    reference.color = mod.foundInStackTrace ? Color.red : Color.white;
                    MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
                    toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
                    Label mod_label = mod.label;
                    toggle.onClick += () => {
                        bool enabled = !manager.IsModEnabled(mod_label);
                        toggle.ChangeState(enabled ? 1 : 0);
                        manager.EnableMod(mod_label, enabled, __instance);
                    };
                    toggle.GetComponent<ToolTip>().OnToolTip = () =>
                        manager.IsModEnabled(mod_label)
                            ? STRINGS.UI.FRONTEND.MODS.TOOLTIPS.ENABLED
                            : STRINGS.UI.FRONTEND.MODS.TOOLTIPS.DISABLED;
                    hierarchyReferences.gameObject.SetActive(true);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(SupermaterialRefineryConfig), nameof(SupermaterialRefineryConfig.ConfigureBuildingTemplate))]
        public static class SupermaterialRefineryConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix()
            {
                RecipeElement[] input = new RecipeElement[] {
                    new(SimHashes.Sulfur.CreateTag(), 500f),
                    new(SimHashes.Aluminum.CreateTag(), 100f),
                    new(SimHashes.Cobalt.CreateTag(), 100f),
                };

                RecipeElement[] output = new RecipeElement[]
                {
                    new(DreamJournalConfig.ID, 1f)
                };

                string recipeID = ComplexRecipeManager.MakeRecipeID(SupermaterialRefineryConfig.ID, input, output);
                new ComplexRecipe(recipeID, input, output) {
                    time = 40f,
                    description = "Because sleeper dupes are overrated.",
                    nameDisplay = RecipeNameDisplay.Result,
                    fabricators = new List<Tag> { SupermaterialRefineryConfig.ID }
                }.requiredTech = "DurableLifeSupport";
            }
        }

        [HarmonyPatch(typeof(PermitItems), nameof(PermitItems.GetOwnedCount))]
        public static class PermitItems_GetOwnedCount_Patch
        {
            public static void Postfix(ref int __result) => __result = 1;
        }

        [HarmonyPatch(typeof(RotPile), nameof(RotPile.TryCreateNotification))]
        public class RotPile_TryCreateNotification_Patch
        {
#if false
            public static bool Prefix(RotPile __instance)
            {
                string name = __instance.smi.master.gameObject.GetProperName();
                return !(name.Contains("COMPOST") || name.Contains("Rot Pile")); // <link="COMPOST">Rot Pile</link>
            }
#else
            public static bool Prefix(RotPile __instance) => !__instance.smi.master.gameObject.GetProperName().Contains("Rot Pile"); // <link="COMPOST">Rot Pile</link>
#endif
        }

        [HarmonyPatch(typeof(KCrashReporter), nameof(KCrashReporter.OnEnable))]
        public static class KCrashReporter_OnEnable_Patch
        {
            public static void Postfix() => KCrashReporter.terminateOnError = false;
        }

        [HarmonyPatch(typeof(Db), nameof(Db.Initialize))]
        public static class Db_Initialize_Patch
        {
            public static void BrainTankPostfix(GameObject go)
            {
                ElementConverter.ConsumedElement[] consumedElements = {
                    new(ElementLoader.FindElementByHash(SimHashes.Oxygen).tag, 0.5f),
                    new(DreamJournalConfig.ID, 0f),
                };
                go.GetComponent<ElementConverter>().consumedElements = consumedElements;
            }

            public static void Postfix()
            {
                KCrashReporter.terminateOnError = false;

                var m_TargetMethod = AccessTools.Method(typeof(MegaBrainTankConfig), nameof(MegaBrainTankConfig.DoPostConfigureComplete));
                var m_Postfix = AccessTools.Method(typeof(Db_Initialize_Patch), nameof(BrainTankPostfix));

                Debug.Assert(HellsenWorldgenMod.harmonyInstance != null, "HellsenWorldgenMod.harmonyInstance is null");
                HellsenWorldgenMod.harmonyInstance?.Patch(m_TargetMethod, null, new HarmonyMethod(m_Postfix));
            }
        }
    }
}
