using System;
using System.Reflection;
using Aki.Reflection.Patching;

namespace Nexus.SPTMod.Patches {
	public class NoTargetPatch : ModulePatch {
		public static Boolean NoTargetActive { get; set; }

		protected override MethodBase GetTargetMethod() {
			return typeof(GClass296).GetMethod(nameof(GClass296.FindDangerEnemy), BindingFlags.Public | BindingFlags.Instance);
		}

		[PatchPostfix]
		private static void Postfix(ref GClass421 __result) {
			if (NoTargetActive && __result != null) {
				if (__result.Person.GetPlayer.IsYourPlayer) {
					__result = null;
				}
			}
		}
	}
}