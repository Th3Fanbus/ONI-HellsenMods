using UnityEngine;

namespace HellsenPowerTweaks
{
	public class SmolBatterySmartConfig : BaseBatteryConfig
	{
		public const string ID = "SmolBatterySmart";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				hitpoints: 30,
				anim: "smolbatterysmart_kanim",
				construction_time: 60f,
				construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
				construction_materials: TUNING.MATERIALS.REFINED_METALS,
				melting_point: 800f,
				exhaust_temperature_active: 0.0f,
				self_heat_kilowatts_active: 0.5f,
				decor: TUNING.BUILDINGS.DECOR.NONE,
				noise: TUNING.NOISE_POLLUTION.NOISY.TIER1);

			SoundEventVolumeCache.instance.AddVolume(
				animFile: "smolbatterysmart_kanim",
				eventName: "Battery_med_rattle",
				vals: TUNING.NOISE_POLLUTION.NOISY.TIER2);

			buildingDef.LogicOutputPorts = [
				LogicPorts.Port.OutputPort(
					id: BatterySmart.PORT_ID,
					cell_offset: new CellOffset(0, 0),
					description: STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT,
					activeDescription: STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_ACTIVE,
					inactiveDescription: STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_INACTIVE,
					show_wire_missing_icon: true)
			];
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BatterySmart batterySmart = go.AddOrGet<BatterySmart>();
			batterySmart.capacity = 5000f;
			batterySmart.joulesLostPerSecond = 6.666667E-29f;
			batterySmart.powerSortOrder = 1000;
			base.DoPostConfigureComplete(go);
		}
	}
}
