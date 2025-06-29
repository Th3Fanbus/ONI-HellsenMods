using STRINGS;
using TUNING;
using UnityEngine;

namespace HellsenWorldgen
{
	public class HellsenWarpReceiverConfig : IHasDlcRestrictions, IEntityConfig
	{
		public const string ID = "HellsenWarpReceiver";

		public string[]? GetRequiredDlcIds() => DlcManager.EXPANSION1;

		public string[]? GetForbiddenDlcIds() => null;

		public GameObject CreatePrefab()
		{
			GameObject placedEntity = EntityTemplates.CreatePlacedEntity(
				id: ID,
				name: MOD_STRINGS.HELLSEN.BUILDINGS.PREFABS.WARPRECEIVER.NAME,
				desc: MOD_STRINGS.HELLSEN.BUILDINGS.PREFABS.WARPRECEIVER.DESC,
				mass: 2000f,
				anim: Assets.GetAnim("hellsen_warp_portal_receiver_kanim"),
				initialAnim: "idle",
				sceneLayer: Grid.SceneLayer.Building,
				width: 3,
				height: 3,
				decor: TUNING.BUILDINGS.DECOR.BONUS.TIER0,
				noise: NOISE_POLLUTION.NOISY.TIER0);
			placedEntity.AddTag(GameTags.NotRoomAssignable);
			placedEntity.AddTag(GameTags.WarpTech);
			placedEntity.AddTag(GameTags.Gravitas);
			PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
			component.SetElement(SimHashes.Unobtanium);
			component.Temperature = 294.15f;
			placedEntity.AddOrGet<Operational>();
			placedEntity.AddOrGet<Notifier>();
			placedEntity.AddOrGet<HellsenWarpReceiver>();
			placedEntity.AddOrGet<UserNameable>();
			placedEntity.AddOrGet<LoopingSounds>();
			placedEntity.AddOrGet<Prioritizable>();
			LoreBearerUtil.AddLoreTo(placedEntity, LoreBearerUtil.UnlockSpecificEntry("notes_AI", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_RECEIVER));
			KBatchedAnimController kbatchedAnimController = placedEntity.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
			kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
			return placedEntity;
		}

		public void OnPrefabInit(GameObject inst)
		{
			inst.GetComponent<HellsenWarpReceiver>().workLayer = Grid.SceneLayer.Building;
			inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[] { ObjectLayer.Building };
			inst.GetComponent<Deconstructable>();
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}