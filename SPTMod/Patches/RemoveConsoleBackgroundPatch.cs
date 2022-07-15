using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;

namespace Nexus.SPTMod.Patches {
	public class RemoveConsoleBackgroundPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(ConsoleScreen).GetMethod(nameof(ConsoleScreen.Show));
		}

		[PatchPostfix]
		private static void Postfix(ConsoleScreen __instance) {
			__instance.transform.Find("LogsPanel")?.Find("Background")?.gameObject.SetActive(false);
		}
	}
}