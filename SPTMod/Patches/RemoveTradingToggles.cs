using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using UnityEngine;

namespace Nexus.SPTMod.Patches {
	public class RemoveTradingToggles : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(TradingScreen).GetMethod(nameof(TradingScreen.Show));
		}

		[PatchPostfix]
		private static void Postfix(TradingScreen __instance) {
			__instance.transform.Find("Toggles").gameObject
				.SetActive(Input.GetKey(SPTModPlugin.Instance.PreventInstantHideout.Value));
		}
	}
}