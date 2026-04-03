namespace TeleStorage
{
	public class TeleStorageGasConfig : TeleStorageBaseConfig
	{
		public const string ID = "TeleStorageGas";

		public override string Id => ID;
		public override string Anim => "telestorage_gas_kanim";
		public override int Width => 5;
		public override int Height => 3;
		public override ConduitType ConduitType => ConduitType.Gas;
		public override CellOffset UtilityInputOffset => new(1, 2);
		public override CellOffset UtilityOutputOffset => new(0, 0);
	}
}
