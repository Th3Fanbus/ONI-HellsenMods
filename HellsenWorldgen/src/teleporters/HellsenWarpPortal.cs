using HarmonyLib;
using HellsenWorldgen;
using KSerialization;
using RexLib;
using System;
using System.Linq;
using UnityEngine;

public static partial class HellsenComponents
{
	public static Components.Cmps<HellsenWarpPortal> HellsenWarpPortals = new();
}

public class HellsenWarpPortal : WarpPortal
{
	[Serialize]
	[SerializeField]
	public int targetID = -1;

	public override void OnSpawn()
	{
		base.OnSpawn();
		HellsenComponents.HellsenWarpPortals.Add(this);
	}

	public override void OnCleanUp()
	{
		base.OnCleanUp();
		HellsenComponents.HellsenWarpPortals.Remove(this);
	}

	public static void DumpPortals()
	{
#if false
        Debug.Log("HELL: Hellsen Transmitters:");
        HellsenWarpPortal[] portalArray = FindObjectsOfType<HellsenWarpPortal>();
        foreach (HellsenWarpPortal portal in portalArray) {
            int id = portal.GetMyWorldId();
            WorldContainer world = ClusterManager.Instance.GetWorld(id);
            Debug.Log($"HELL:\t - id: {id}, world: {world}, name: {world.worldName}, type: {world.worldType}");

        }
        Debug.Log("HELL: Hellsen Receivers:");
        HellsenWarpReceiver[] receiverArray = FindObjectsOfType<HellsenWarpReceiver>();
        foreach (HellsenWarpReceiver receiver in receiverArray) {
            int id = receiver.GetMyWorldId();
            WorldContainer world = ClusterManager.Instance.GetWorld(id);
            Debug.Log($"HELL:\t - id: {id}, world: {world}, name: {world.worldName}, type: {world.worldType}");

        }
#endif
	}

	public bool IsLinked => targetID >= 0 && targetID != this.GetMyWorldId();

	private HellsenWarpReceiver? GetTargetReceiver_Internal()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(HellsenWarpReceiverConfig.ID);
		int myID = this.GetMyWorldId();
		//HellsenWarpReceiver[] receiverList = FindObjectsOfType<HellsenWarpReceiver>();
		Components.Cmps<HellsenWarpReceiver> receiverList = HellsenComponents.HellsenWarpReceivers;
		if (receiverList.Count() == 0) {
			Debug.LogWarning("HELL: No hellsen receiver at all found");
			return null;
		}
		/* World ID is valid, try to find any receiver in said world */
		if (IsLinked) {
			try {
				return receiverList.First(r => r.GetMyWorldId() == targetID);
			} catch (InvalidOperationException) {
				Debug.LogWarning("HELL: Invalid target hellsen receiver");
				/* Break the link */
				targetID = -1;
			}
		}
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag(HellsenWarpPortalConfig.ID);
		/* Complete the link in the other direction */
		//HellsenWarpPortal[] otherPortals = FindObjectsOfType<HellsenWarpPortal>();
		Components.Cmps<HellsenWarpPortal> otherPortals = HellsenComponents.HellsenWarpPortals;
		try {
			/* Try to find an existing portal ---> receiver link in the other direction */
			HellsenWarpPortal otherP = otherPortals.Where(p => p.GetMyWorldId() != myID).First(p => p.targetID == myID);
			int otherID = otherP.GetMyWorldId();
			/* Then find a corresponding receiver in the same world as the portal */
			HellsenWarpReceiver receiver = receiverList.First(r => r.GetMyWorldId() == otherID);
			targetID = otherID;
			return receiver;
		} catch (InvalidOperationException) {
			Debug.LogWarning("HELL: Paired hellsen receiver not found");
		}
		/* Find an unlinked portal with a receiver in the same world */
		try {
			/* Find an unlinked portal */
			HellsenWarpPortal otherP = otherPortals.Where(p => p.GetMyWorldId() != myID).OrderBy(p => p.GetMyWorldId()).First(p => !p.IsLinked);
			int otherID = otherP.GetMyWorldId();
			/* Then find a corresponding receiver in the same world as the portal */
			HellsenWarpReceiver receiver = receiverList.First(r => r.GetMyWorldId() == otherID);
			targetID = otherID;
			/* Also link back the other portal, even if our world doesn't have any receiver */
			otherP.targetID = myID;
			return receiver;
		} catch (InvalidOperationException) {
			Debug.LogWarning("HELL: No remote hellsen portal/receiver pair found");
		}
		try {
			HellsenWarpReceiver receiver = receiverList.First(r => r.GetMyWorldId() == myID);
			targetID = myID;
			return receiver;
		} catch (InvalidOperationException) {
			Debug.LogWarning("HELL: No hellsen receiver found???");
			return null;
		}
	}

	public HellsenWarpReceiver? GetTargetReceiver() => GetTargetReceiver_Internal().GetValid();

	public bool TryGetTargetWorldId(out int worldId)
	{
		DumpPortals();
		HellsenWarpReceiver? warpReceiver = GetTargetReceiver();
		worldId = warpReceiver?.GetMyWorldId() ?? -1;
		Debug.Log($"HELL: {this.GetMyWorldId()} ---> {worldId}");
		return worldId > 0;
	}

	[HarmonyPatch(typeof(WarpPortal), nameof(WarpPortal.GetTargetWorldID))]
	public class WarpPortal_GetTargetWorldID_Patch
	{
		public static bool Prefix(WarpPortal __instance, ref int __result)
			=> (__instance as HellsenWarpPortal).GetValid() switch {
				HellsenWarpPortal portal => portal.TryGetTargetWorldId(out __result) && false,
				_ => true,
			};
	}

	[HarmonyPatch(typeof(WarpPortal), nameof(WarpPortal.Warp))]
	public class WarpPortal_Warp_Patch
	{
		public static bool Prefix(WarpPortal __instance) => (__instance as HellsenWarpPortal).GetValid()?.TryWarp(false) ?? true;
	}

	public bool TryWarp(bool successResult = true)
	{
		if (worker.IsNull()) {
			return false;
		}
		DumpPortals();

		HellsenWarpReceiver? warpReceiver = GetTargetReceiver();
		if (warpReceiver is null) {
			Debug.LogWarning("HELL: No warp receiver found - maybe POI stomping or failure to spawn?");
			return false;
		}

		delayWarpRoutine = StartCoroutine(DelayedWarp(warpReceiver));
		if (SelectTool.Instance.selected == GetComponent<KSelectable>()) {
			SelectTool.Instance.Select(null, skipSound: true);
		}
		return successResult;
	}
}