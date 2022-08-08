using System;
using System.Collections.Generic;
using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using Nexus.NexAPI;
using Nexus.NexAPI.CustomInteractions;

namespace Nexus.SPTMod {
	public class CustomWorkbench : CustomInteractionsHandler {
		public override IEnumerable<Type> AffectedTypes { get; } = new[] {typeof(GInterface137)};

		public override Boolean AffectsRaid {
			get {
				return false;
			}
		}

		public override GClass2388
			GetInteractions(GClass2388 __result, GamePlayerOwner owner, GInterface79 interactive) {
			if (!(interactive is GInterface137 ginterface137) || ginterface137.Area == null ||
				ginterface137.Area.Data == null || !ginterface137.Area.Data.IsInstalled ||
				ginterface137.Area.Data.Template.Type != EAreaType.Workbench) {
				return __result;
			}

			Slot pistolSlot = owner.Player.GetInventoryController().Inventory.Equipment.GetSlot(EquipmentSlot.Holster);
			Item pistol = pistolSlot?.ContainedItem;
			if (pistol != null) {
				__result.Actions.Add(new GClass2387 {
					Name = $"MOD {pistol.LocalizedShortName()}",
					Action = () => {
						ItemUiContext.Instance.ModWeapon(pistol);
					}
				});
			}

			Slot primarySlot = owner.Player.GetInventoryController().Inventory.Equipment
				.GetSlot(EquipmentSlot.FirstPrimaryWeapon);
			Item primary = primarySlot?.ContainedItem;
			if (primary != null) {
				__result.Actions.Add(new GClass2387 {
					Name = $"MOD {primary.LocalizedShortName()}",
					Action = () => {
						ItemUiContext.Instance.ModWeapon(primary);
					}
				});
			}

			Slot secondarySlot = owner.Player.GetInventoryController().Inventory.Equipment
				.GetSlot(EquipmentSlot.SecondPrimaryWeapon);
			Item secondary = secondarySlot?.ContainedItem;
			if (secondary != null) {
				__result.Actions.Add(new GClass2387 {
					Name = $"MOD {secondary.LocalizedShortName()}",
					Action = () => {
						ItemUiContext.Instance.ModWeapon(secondary);
					}
				});
			}

			__result.Actions.Add(new GClass2387 {
				Name = "CREATE PRESET",
				Action = () => {
					ItemUiContext.Instance.EditBuild(null);
				}
			});

			return __result;
		}
	}
}