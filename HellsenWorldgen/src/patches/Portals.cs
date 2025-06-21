using HarmonyLib;
using Klei.AI;
using ProcGenGame;
using RexLib;
using TemplateClasses;
using UnityEngine;

namespace HellsenWorldgen
{
    public static partial class Patches
    {
        public static void DumpPortals()
        {
            if (HellsenConfig.DUMP_PORTAL_INFO) {
#pragma warning disable CS0162 // Unreachable code detected
                RexLogger.Log("HELL: Transmitters:");
                WarpPortal[] transmitterArray = Object.FindObjectsOfType<WarpPortal>();
                foreach (WarpPortal transmitter in transmitterArray) {
                    int id = transmitter.GetMyWorldId();
                    WorldContainer world = ClusterManager.Instance.GetWorld(id);
                    RexLogger.Log($"HELL:\t - id: {id}, world: {world}, name: {world.worldName}, type: {world.worldType}");
                }
                RexLogger.Log("HELL: Receivers:");
                WarpReceiver[] receiverArray = Object.FindObjectsOfType<WarpReceiver>();
                foreach (WarpReceiver receiver in receiverArray) {
                    int id = receiver.GetMyWorldId();
                    WorldContainer world = ClusterManager.Instance.GetWorld(id);
                    RexLogger.Log($"HELL:\t - id: {id}, world: {world}, name: {world.worldName}, type: {world.worldType}");
                }
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        [HarmonyPatch(typeof(WarpPortal), nameof(WarpPortal.GetTargetWorldID))]
        public class WarpPortal_GetTargetWorldID_Patch
        {
            private static int HellsenGetTargetWorldID(WarpPortal self)
            {
                DumpPortals();

                SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);

                int fallbackID = -1;
                WarpReceiver[] array = Object.FindObjectsOfType<WarpReceiver>();
                foreach (WarpReceiver receiver in array) {
                    if (receiver.GetType() != typeof(WarpReceiver)) {
                        continue;
                    }
                    int otherID = receiver.GetMyWorldId();
                    if (otherID != self.GetMyWorldId()) {
                        return otherID;
                    }
                    fallbackID = otherID;
                }

                if (fallbackID >= 0) {
                    RexLogger.LogWarning("No remote receiver world found for warp portal sender");
                } else {
                    RexLogger.LogWarning("No receiver at all found for warp portal sender");
                }
                return fallbackID;
            }

            public static bool Prefix(WarpPortal __instance, ref int __result)
            {
                if (__instance.GetType() != typeof(WarpPortal)) {
                    return true;
                }
                __result = HellsenGetTargetWorldID(__instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(WarpPortal), nameof(WarpPortal.Warp))]
        public class WarpPortal_Warp_Patch
        {
            public static bool Prefix(WarpPortal __instance)
            {
                if (__instance.GetType() != typeof(WarpPortal)) {
                    return true;
                }
                if (__instance.worker.IsNull()) {
                    return false;
                }

                DumpPortals();

                WarpReceiver? warpReceiver = null;
                WarpReceiver[] array = Object.FindObjectsOfType<WarpReceiver>();
                foreach (WarpReceiver receiver in array) {
                    if (receiver.GetType() != typeof(WarpReceiver)) {
                        continue;
                    }
                    if (receiver.GetMyWorldId() != __instance.GetMyWorldId()) {
                        warpReceiver = receiver;
                        break;
                    }
                }

                if (warpReceiver.IsNull()) {
                    SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(WarpReceiverConfig.ID);
                    warpReceiver = Object.FindObjectOfType<WarpReceiver>();
                }

                if (warpReceiver.IsNull()) {
                    RexLogger.LogWarning("No warp receiver found - maybe POI stomping or failure to spawn?");
                    return false;
                }

                __instance.delayWarpRoutine = __instance.StartCoroutine(__instance.DelayedWarp(warpReceiver));

                if (SelectTool.Instance.selected == __instance.GetComponent<KSelectable>()) {
                    SelectTool.Instance.Select(null, skipSound: true);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(WarpPortal), nameof(WarpPortal.Discover))]
        public class WarpPortal_Discover_Patch
        {
            public static bool Prefix(WarpPortal __instance)
            {
                if (!__instance.discovered) {
                    DumpPortals();

                    int targetID = __instance.GetTargetWorldID();
                    if (targetID < 0) {
                        return false;
                    }
                    WorldContainer world = ClusterManager.Instance.GetWorld(targetID);
                    if (world.IsNull()) {
                        return false;
                    }
                    world.SetDiscovered(reveal_surface: true);
                    if (GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.WarpWorldReveal).smi is not SimpleEvent.StatesInstance statesInstance) {
                        return false;
                    }
                    if (Components.LiveMinionIdentities.Count > 0) {
                        statesInstance.minions = [Components.LiveMinionIdentities[0].gameObject];
                        statesInstance.callback = delegate {
                            ManagementMenu.Instance.OpenClusterMap();
                            ClusterMapScreen.Instance.SetTargetFocusPosition(ClusterManager.Instance.GetWorld(targetID).GetMyWorldLocation());
                        };
                        statesInstance.ShowEventPopup();
                    }
                    __instance.discovered = true;
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(GameSpawnData), nameof(GameSpawnData.IsWarpTeleporter))]
        public class GameSpawnData_IsWarpTeleporter_Patch
        {
            public static bool Postfix(bool result, Prefab prefab) => prefab.id switch {
                HellsenWarpPortalConfig.ID or HellsenWarpReceiverConfig.ID => true,
                _ => result,
            };
        }
    }
}
