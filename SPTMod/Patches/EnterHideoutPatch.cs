using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.Hideout;
using UnityEngine;
using Object = System.Object;

namespace Nexus.SPTMod.Patches {
	public class EnterHideoutPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(HideoutScreenOverlay).GetMethod(nameof(HideoutScreenOverlay.Show));
		}

		[PatchPostfix]
		private static void Postfix(HideoutScreenOverlay __instance) {
			if (Input.GetKey(SPTModPlugin.Instance.PreventInstantHideout.Value) ||
				(typeof(HideoutScreenOverlay)
					.GetField("hideoutPlayer_0", BindingFlags.NonPublic | BindingFlags.Instance)
					?.GetValue(__instance) as Player)?.InteractableObject != null) {
				return;
			}

			typeof(HideoutScreenOverlay).GetMethod("method_6", BindingFlags.NonPublic | BindingFlags.Instance)
				?.Invoke(__instance, Array.Empty<Object>());
		}
	}
}