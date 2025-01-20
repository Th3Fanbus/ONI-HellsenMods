using UnityEngine;

namespace HellsenPowerTweaks
{
    public class HugeBatterySmartConfig : BaseBatteryConfig
    {
        public const string ID = "HugeBatterySmart";

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef buildingDef = CreateBuildingDef(
                id: ID,
                width: 2,
                height: 4,
                hitpoints: 60,
                anim: "hugebatterysmart_kanim",
                construction_time: 120f,
                construction_mass: TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5,
                construction_materials: TUNING.MATERIALS.REFINED_METALS,
                melting_point: 800f,
                exhaust_temperature_active: 0.0f,
                self_heat_kilowatts_active: 1.0f,
                decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER3,
                noise: TUNING.NOISE_POLLUTION.NOISY.TIER1);

            SoundEventVolumeCache.instance.AddVolume(
                animFile: "hugebatterysmart_kanim",
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
            batterySmart.capacity = 40000f;
            batterySmart.joulesLostPerSecond = 1.3333333f;
            batterySmart.powerSortOrder = 1000;
            base.DoPostConfigureComplete(go);
        }
    }
}
