using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using UnityEngine;

namespace Nexus.SPTMod.Patches {
	public class StraightToHideoutPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(MenuScreen).GetMethod(nameof(MenuScreen.Show));
		}

		[PatchPrefix]
		private static Boolean Prefix(MenuScreen __instance) {
			Boolean showMenu = Input.GetKey(SPTModPlugin.Instance.PreventInstantHideout.Value);
			if (!showMenu) {
				(typeof(MenuScreen).GetField("ScreenController", BindingFlags.NonPublic | BindingFlags.Instance)
					?.GetValue(__instance) as MenuScreen.GClass2480)?.SelectMenuItem(EMenuType.Hideout);
			}

			return showMenu;
		}
	}
}