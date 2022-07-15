using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using JetBrains.Annotations;
using Nexus.NexAPI.CustomInteractions;

namespace Nexus.NexAPI.Patches {
	public class CustomHideoutInteractionsPatch : ModulePatch {
		private static List<CustomInteractionsHandler> _handlers = new List<CustomInteractionsHandler>();

		protected override MethodBase GetTargetMethod() {
			return typeof(GClass1543).GetMethod(nameof(GClass1543.GetAvailableHideoutActions), BindingFlags.Public | BindingFlags.Static);
		}

		[PatchPostfix]
		private static void Postfix(ref GClass2304 __result, HideoutPlayerOwner owner, [CanBeNull] GInterface73 interactive) {
			if (NexAPIPlugin.Instance.CustomInteractionsHandlerManager.TryGetHandlers(interactive?.GetType(), true, _handlers)) {
				__result = _handlers.Aggregate(__result, (current, handler) => handler.GetInteractions(current, owner, interactive));
			}
		}
	}
}