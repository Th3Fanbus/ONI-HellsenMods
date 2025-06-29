namespace TeleStorage
{
	public class TeleStorageLiquidConfig : TeleStorageBaseConfig
	{
		public const string Id = "TeleStorageLiquid";

		public override TeleStorageProperties GetProperties() => new(
			id: Id,
			anim: "liquidreservoir_kanim",
			width: 2,
			height: 3,
			conduitType: ConduitType.Liquid,
			utilityInputOffset: new CellOffset(1, 2),
			utilityOutputOffset: new CellOffset(0, 0)
		);
	}
}
