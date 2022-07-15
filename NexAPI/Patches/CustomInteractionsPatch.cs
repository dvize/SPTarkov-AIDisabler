using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using JetBrains.Annotations;
using Nexus.NexAPI.CustomInteractions;

namespace Nexus.NexAPI.Patches {
	public class CustomInteractionsPatch : ModulePatch {
		private static List<CustomInteractionsHandler> _handlers = new List<CustomInteractionsHandler>();

		protected override MethodBase GetTargetMethod() {
			return typeof(GClass1543).GetMethod(nameof(GClass1543.GetAvailableActions), BindingFlags.Public | BindingFlags.Static);
		}

		[PatchPostfix]
		private static void Postfix(ref GClass2304 __result, GamePlayerOwner owner, [CanBeNull] GInterface73 interactive) {
			if (interactive == null) {
				return;
			}

			if (NexAPIPlugin.Instance.CustomInteractionsHandlerManager.TryGetHandlers(interactive.GetType(), false, _handlers)) {
				__result = _handlers.Aggregate(__result, (current, handler) => handler.GetInteractions(current, owner, interactive));
			}
		}
	}
}