using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using UnityEngine;

namespace Nexus.SPTMod.Patches {
	public class ContextMenuWhileSearchingPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(ItemUiContext).GetMethod(nameof(ItemUiContext.ShowContextMenu));
		}

		[PatchPrefix]
		private static Boolean Prefix(ItemUiContext __instance, GClass2244 itemContext, Vector2 position) {
			Boolean isEnabled = SPTModPlugin.Instance.InspectWhileSearching.Value;
			if (isEnabled) {
				__instance.ContextMenu.Show(position, __instance.GetItemContextInteractions(itemContext, null), null,
					itemContext.Item);
			}

			return !isEnabled;
		}
	}
}