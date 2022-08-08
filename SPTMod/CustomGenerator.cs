using System;
using System.Collections.Generic;
using System.Linq;
using EFT;
using EFT.Communications;
using EFT.Hideout;
using EFT.InventoryLogic;
using Nexus.NexAPI;
using Nexus.NexAPI.CustomInteractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Nexus.SPTMod {
	public class CustomGenerator : CustomInteractionsHandler {
		public override IEnumerable<Type> AffectedTypes { get; } = new[] {typeof(GInterface137)};

		public override GClass2388
			GetInteractions(GClass2388 __result, GamePlayerOwner owner, GInterface79 interactive) {
			if (!(interactive is GInterface137 gInterface137) || !GeneratorBehaviour.IsGeneratorInstalled ||
				gInterface137.Area == null || gInterface137.Area.Data == null || !gInterface137.Area.Data.IsInstalled ||
				gInterface137.Area.AreaTemplate.Type != EAreaType.Generator) {
				return __result;
			}

			if (!(gInterface137.Area.AreaTemplate.AreaBehaviour is GeneratorBehaviour generatorBehaviour)) {
				return __result;
			}

			Boolean hasEmptySlot = generatorBehaviour.UsingItems.Any(t => t is null);
			Boolean needsToRemove = generatorBehaviour.UsingItems.Any(t =>
				t != null && t.ResourceHolderComponent.Value.ApproxEquals(0f));
			switch (GeneratorBehaviour.FuelDetails.Type) {
				case EDetailsType.SwitchedOff:
					if (needsToRemove) {
						if (!(Object.FindObjectOfType<Player>().GetInventoryController().Inventory.Stash.Containers
								.FirstOrDefault() is GClass1947 stash)) {
							break;
						}

						__result.Actions.Insert(0, new GClass2387 {
							Action = () => {
								GClass2141[] items = generatorBehaviour.UsingItems;
								for (Int32 i = 0; i < items.Length; i++) {
									if (!items[i].ResourceHolderComponent.Value.ApproxEquals(0f)) {
										continue;
									}

									if (stash.Add(items[i]).Succeeded) {
										items[i] = null;
									}
								}
							},
							Name = "Remove empty canisters"
						});
					}

					if (hasEmptySlot) {
						if (!(Object.FindObjectOfType<Player>().GetInventoryController().Inventory.Stash.Containers
								.FirstOrDefault() is GClass1947 stash)) {
							break;
						}

						IEnumerable<Item> fuels = stash.Items.Where(t => t.TemplateId == "5d650c3e815116009f6201d2");
						foreach (Item fuel in fuels) {
							ResourceComponent component = fuel.GetItemComponent<ResourceComponent>();
							if (component == null) {
								continue;
							}

							if (component.Value.ApproxEquals(0f)) {
								continue;
							}

							Boolean isFull = component.Value.ApproxEquals(component.MaxResource);
							String text = (fuel.SpawnedInSession ? "FIR" : String.Empty) +
										  (isFull ? "full" : $"{component.Value}/{component.MaxResource}");
							__result.Actions.Insert(0, new GClass2387 {
								Action = () => {
									GStruct285<Boolean> attempt = stash.Remove(fuel);
									if (attempt.Succeeded) {
										generatorBehaviour.InstallConsumableItems(new[] {fuel as GClass2141});
									}
									else {
										GClass1759.DisplayMessageNotification(
											attempt.Error == null ? "Unknown error" : attempt.Error.ToString(),
											ENotificationDurationType.Default, ENotificationIconType.Alert, Color.red);
									}
								},
								Name = $"Insert {text} fuel"
							});
						}
					}

					break;
			}

			__result.Actions.Insert(0, new GClass2387 {
				Action = () => {
					generatorBehaviour.ResourceConsumer.IsOn = !generatorBehaviour.ResourceConsumer.IsOn;
				},
				Name = $"Turn {(generatorBehaviour.SwitchedOn ? "off" : "on")}"
			});

			return __result;
		}
	}
}