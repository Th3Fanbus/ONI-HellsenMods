namespace TeleStorage
{
	public class TeleStorageGasConfig : TeleStorageBaseConfig
	{
		public const string Id = "TeleStorageGas";

		public override TeleStorageProperties GetProperties() => new(
			id: Id,
			anim: "gasstorage_kanim",
			width: 5,
			height: 3,
			conduitType: ConduitType.Gas,
			utilityInputOffset: new CellOffset(1, 2),
			utilityOutputOffset: new CellOffset(0, 0)
		);
	}
}
