using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.UI;
using UnityEngine;

namespace Nexus.SPTMod.Patches {
	public class MenuTaskBarPatch : ModulePatch {
		private static Int32[] _tabsToRemove = {1, 3, 4, 5, 6, 7, 9};

		protected override MethodBase GetTargetMethod() {
			return typeof(MenuTaskBar).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		[PatchPostfix]
		private static void Postfix(MenuTaskBar __instance) {
			Transform tabs = __instance.transform.GetChild(0);
			foreach (Int32 tab in _tabsToRemove) {
				tabs.GetChild(tab).gameObject.SetActive(false);
			}
		}
	}
}