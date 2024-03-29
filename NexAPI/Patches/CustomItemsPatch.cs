﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.InventoryLogic;
using EFT.UI;
using Nexus.NexAPI.CustomItems;

namespace Nexus.NexAPI.Patches {
	public class CustomItemHandlerPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(ItemUiContext).GetMethod(nameof(ItemUiContext.GetItemContextInteractions));
		}

		[PatchPostfix]
		private static void Postfix(GClass2397<EItemInfoButton> __result, ItemUiContext __instance,
			GClass2244 itemContext, Action closeAction) {
			if (!(__result is GClass2398) || itemContext?.Item == null) {
				return;
			}

			if (!NexAPIPlugin.Instance.CustomItemHandlerManager.TryGetHandler(itemContext.Item.TemplateId,
					itemContext.Item.GetType(), out CustomItemHandler handler)) {
				return;
			}

			IEnumerable<GClass2396> generatedCallbacks = handler.GenerateCallbacks();
			handler.Context = itemContext;
			typeof(GClass2397<EItemInfoButton>).GetField("dictionary_1", BindingFlags.NonPublic | BindingFlags.Instance)
				?.SetValue(__result,
					(__result.DynamicInteractions != null ?
						__result.DynamicInteractions.Concat(generatedCallbacks) :
						generatedCallbacks).ToDictionary(t => t.Key));
		}
	}
}