using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using JetBrains.Annotations;
using Nexus.NexAPI.CustomInteractions;

namespace Nexus.NexAPI.Patches {
	public class CustomInteractionsPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(GClass1614).GetMethod(nameof(GClass1614.GetAvailableActions),
				BindingFlags.Public | BindingFlags.Static);
		}

		[PatchPostfix]
		private static void Postfix(ref GClass2388 __result, GamePlayerOwner owner,
			[CanBeNull] GInterface79 interactive) {
			if (interactive == null) {
				return;
			}

			if (NexAPIPlugin.Instance.CustomInteractionsHandlerManager.TryGetHandlers(interactive.GetType(), false,
					out List<CustomInteractionsHandler> handlers)) {
				__result = handlers.Aggregate(__result,
					(current, handler) => handler.GetInteractions(current, owner, interactive));
			}
		}
	}
}