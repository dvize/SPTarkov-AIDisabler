using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using EFT.UI.Ragfair;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Nexus.SPTMod.Patches {
	public class RagfairWindowBuyAll : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(DialogWindow<GClass2455>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
				.Single(t => t.Name == "Update");
		}

		[PatchPostfix]
		private static void Postfix(Object __instance) {
			if (!(__instance is HandoverRagfairMoneyWindow)) {
				return;
			}

			if (Input.GetKeyDown(KeyCode.A)) {
				(typeof(HandoverRagfairMoneyWindow)
					.GetField("_allItemsButton", BindingFlags.NonPublic | BindingFlags.Instance)
					?.GetValue(__instance) as Button)?.onClick.Invoke();
			}
		}
	}
}