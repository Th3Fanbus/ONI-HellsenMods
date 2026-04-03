namespace TeleStorage
{
	public class TeleStorageLiquidConfig : TeleStorageBaseConfig
	{
		public const string ID = "TeleStorageLiquid";

		public override string Id => ID;
		public override string Anim => "telestorage_liquid_kanim";
		public override int Width => 2;
		public override int Height => 3;
		public override ConduitType ConduitType => ConduitType.Liquid;
		public override CellOffset UtilityInputOffset => new(1, 2);
		public override CellOffset UtilityOutputOffset => new(0, 0);
	}
}
