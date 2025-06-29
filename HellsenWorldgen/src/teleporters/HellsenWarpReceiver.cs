public static partial class HellsenComponents
{
	public static Components.Cmps<HellsenWarpReceiver> HellsenWarpReceivers = new();
}

public class HellsenWarpReceiver : WarpReceiver
{
	public override void OnSpawn()
	{
		base.OnSpawn();
		HellsenComponents.HellsenWarpReceivers.Add(this);
	}

	public override void OnCleanUp()
	{
		base.OnCleanUp();
		HellsenComponents.HellsenWarpReceivers.Remove(this);
	}
}