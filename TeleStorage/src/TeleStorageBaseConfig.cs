using TUNING;
using UnityEngine;

namespace TeleStorage
{
	public abstract class TeleStorageBaseConfig : IBuildingConfig
	{
		public readonly struct TeleStorageProperties
		{
			public readonly string id;
			public readonly string anim;
			public readonly int width;
			public readonly int height;
			public readonly ConduitType conduitType;
			public readonly CellOffset utilityInputOffset;
			public readonly CellOffset utilityOutputOffset;

			public TeleStorageProperties(
				string id,
				string anim,
				int width,
				int height,
				ConduitType conduitType,
				CellOffset utilityInputOffset,
				CellOffset utilityOutputOffset)
			{
				this.id = id;
				this.anim = anim;
				this.width = width;
				this.height = height;
				this.conduitType = conduitType;
				this.utilityInputOffset = utilityInputOffset;
				this.utilityOutputOffset = utilityOutputOffset;
			}
		};

		public abstract TeleStorageProperties GetProperties();

		public override BuildingDef CreateBuildingDef()
		{
			TeleStorageProperties tele = GetProperties();

			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: tele.id,
				width: tele.width,
				height: tele.height,
				anim: tele.anim,
				hitpoints: BUILDINGS.HITPOINTS.TIER2,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				construction_mass: [
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
					BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
				],
				construction_materials: [
					"Steel",
					"Plastic",
					"Diamond",
				],
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER0,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: BUILDINGS.DECOR.PENALTY.TIER1,
				noise: NOISE_POLLUTION.NOISY.TIER0,
				0.2f
			);
			def.PermittedRotations = PermittedRotations.R360;
			def.InputConduitType = tele.conduitType;
			def.OutputConduitType = tele.conduitType;
			def.Floodable = false;
			def.ViewMode = TeleStorageUtils.GetViewMode(tele.conduitType);
			def.AudioCategory = "HollowMetal";
			def.UtilityInputOffset = tele.utilityInputOffset;
			def.UtilityOutputOffset = tele.utilityOutputOffset;
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			TeleStorageProperties tele = GetProperties();

			go.AddOrGet<Filterable>().filterElementState = TeleStorageUtils.GetElementState(tele.conduitType);
			go.AddOrGet<TeleStorageFlowControl>();
			go.AddOrGet<TeleStorage>().Type = tele.conduitType;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go) => GeneratedBuildings.RegisterSingleLogicInputPort(go);
		public override void DoPostConfigureUnderConstruction(GameObject go) => GeneratedBuildings.RegisterSingleLogicInputPort(go);

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterSingleLogicInputPort(go);
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGet<Operational>();

			Object.DestroyImmediate(go.GetComponent<RequireInputs>());
			Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
			Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());

			BuildingTemplates.DoPostConfigure(go);
		}
	}
}
