using HarmonyLib;

namespace TeleStorage
{
	public class ElementLoaderPatch
	{
		[HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.FinaliseElementsTable))]
		public static class ElementLoader_FinaliseElementsTable_Patch
		{
			public static void Postfix()
			{
				foreach (Element el in ElementLoader.elements) {
					if (el == null) {
						continue;
					}

					Debug.Log($"HELL: el {el.name} is {el.GetStateString()} ({el.state})");
					if (el.IsLiquid || el.IsGas) {
						//el.state &= ~Element.State.Unbreakable;
					}
				}
			}
		}
	}
}
