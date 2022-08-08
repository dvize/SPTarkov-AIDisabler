using System;
using System.Diagnostics;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.InputSystem;
using EFT.UI;

namespace Nexus.SPTMod.Patches {
	public class NeuterHideoutControls : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(HideoutPlayerOwner).GetMethod("TranslateCommand",
				BindingFlags.NonPublic | BindingFlags.Instance);
		}

		[PatchPrefix]
		private static Boolean Prefix(HideoutPlayerOwner __instance, ECommand command) {
			if (__instance.InShootingRange) {
				return true;
			}

			if (command != ECommand.Escape) {
				return __instance.InShootingRange;
			}

			ItemUiContext.Instance.ShowMessageWindow(
				$"Press Y to close the game{Environment.NewLine}Press N to keep playing{Environment.NewLine}Hold {SPTModPlugin.Instance.PreventInstantHideout.Value} to go to the main menu",
				() => {
					Process.GetCurrentProcess().Close();
				}, null, String.Empty);

			return false;
		}
	}
}