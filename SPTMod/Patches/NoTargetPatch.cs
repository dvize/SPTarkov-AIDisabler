using System;
using System.Reflection;
using Aki.Reflection.Patching;

namespace Nexus.SPTMod.Patches {
	public class NoTargetPatch : ModulePatch {
		public static Boolean NoTargetActive { get; set; }

		protected override MethodBase GetTargetMethod() {
			return typeof(GClass313).GetMethod(nameof(GClass313.FindDangerEnemy),
				BindingFlags.Public | BindingFlags.Instance);
		}

		[PatchPostfix]
		private static void Postfix(ref GClass442 __result) {
			if (!NoTargetActive || __result == null) {
				return;
			}

			if (__result.Person != null && __result.Person.GetPlayer != null &&
				__result.Person.GetPlayer.IsYourPlayer) {
				__result = null;
			}
		}
	}
}