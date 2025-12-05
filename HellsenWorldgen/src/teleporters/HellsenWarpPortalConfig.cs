using STRINGS;
using TUNING;
using UnityEngine;

namespace HellsenWorldgen
{
	public class HellsenWarpPortalConfig : IHasDlcRestrictions, IEntityConfig
	{
		public const string ID = "HellsenWarpPortal";

		public string[]? GetRequiredDlcIds() => DlcManager.EXPANSION1;

		public string[]? GetForbiddenDlcIds() => null;

		public GameObject CreatePrefab()
		{
			GameObject placedEntity = EntityTemplates.CreatePlacedEntity(
				id: ID,
				name: MOD_STRINGS.HELLSEN.BUILDINGS.PREFABS.WARPPORTAL.NAME,
				desc: MOD_STRINGS.HELLSEN.BUILDINGS.PREFABS.WARPPORTAL.DESC,
				mass: 2000f,
				anim: Assets.GetAnim("hellsen_warp_portal_sender_kanim"),
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
			placedEntity.AddOrGet<HellsenWarpPortal>();
			placedEntity.AddOrGet<UserNameable>();
			placedEntity.AddOrGet<LoopingSounds>();
			placedEntity.AddOrGet<Ownable>().tintWhenUnassigned = false;
			LoreBearerUtil.AddLoreTo(placedEntity, LoreBearerUtil.UnlockSpecificEntry("notes_teleportation", UI.USERMENUACTIONS.READLORE.SEARCH_TELEPORTER_SENDER));
			placedEntity.AddOrGet<Prioritizable>();
			KBatchedAnimController kbatchedAnimController = placedEntity.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
			kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
			return placedEntity;
		}

		public void OnPrefabInit(GameObject inst)
		{
			inst.GetComponent<HellsenWarpPortal>().workLayer = Grid.SceneLayer.Building;
			inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.WarpPortal.Id;
			inst.GetComponent<OccupyArea>().objectLayers = [ObjectLayer.Building];
			inst.GetComponent<Deconstructable>();
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}