using UnityEngine;

namespace TeleStorage
{
	public abstract class TeleStorageBaseConfig : IBuildingConfig
	{
		public abstract string Id { get; }
		public abstract string Anim { get; }
		public abstract int Width { get; }
		public abstract int Height { get; }
		public abstract ConduitType ConduitType { get; }
		public abstract CellOffset UtilityInputOffset { get; }
		public abstract CellOffset UtilityOutputOffset { get; }

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: Width,
				height: Height,
				anim: Anim,
				hitpoints: TUNING.BUILDINGS.HITPOINTS.TIER2,
				construction_time: TUNING.BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				construction_mass: [
					TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
					TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
					TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
				],
				construction_materials: [
					SimHashes.Steel.ToString(),
					TUNING.MATERIALS.PLASTIC,
					SimHashes.Diamond.ToString(),
				],
				melting_point: TUNING.BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER1,
				noise: TUNING.NOISE_POLLUTION.NOISY.TIER0
			);
			def.PermittedRotations = PermittedRotations.R360;
			def.InputConduitType = ConduitType;
			def.OutputConduitType = ConduitType;
			def.Floodable = false;
			def.ViewMode = TeleStorageUtils.GetViewMode(ConduitType);
			def.AudioCategory = "HollowMetal";
			def.UtilityInputOffset = UtilityInputOffset;
			def.UtilityOutputOffset = UtilityOutputOffset;
			GeneratedBuildings.RegisterWithOverlay(TeleStorageUtils.GetOverlayTags(ConduitType), Id);
			def.AddSearchTerms(STRINGS.SEARCH_TERMS.STORAGE);
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<Filterable>().filterElementState = TeleStorageUtils.GetElementState(ConduitType);
			go.AddOrGet<CopyBuildingSettings>();
			go.AddOrGet<TeleStorageFlowControl>();
			go.AddOrGet<TeleStorage>().Type = ConduitType;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => GeneratedBuildings.RegisterSingleLogicInputPort(go);
		public override void DoPostConfigureUnderConstruction(GameObject go) => GeneratedBuildings.RegisterSingleLogicInputPort(go);

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterSingleLogicInputPort(go);
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGet<Operational>();
			go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);

			Object.DestroyImmediate(go.GetComponent<RequireInputs>());
			Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
			Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
		}
	}
}
