using System;
using System.Collections.Generic;
using EFT;
using EFT.InventoryLogic;
using EFT.UI;
using Nexus.NexAPI;
using Nexus.NexAPI.CustomInteractions;

namespace Nexus.SPTMod {
	public class CustomWorkbench : CustomInteractionsHandler {
		public override IEnumerable<Type> AffectedTypes { get; } = new[] {typeof(GInterface132)};

		public override Boolean AffectsRaid {
			get { return false; }
		}

		public override GClass2304 GetInteractions(GClass2304 __result, GamePlayerOwner owner, GInterface73 interactive) {
			if (!(interactive is GInterface132 GInterface132) || !GInterface132.Area.Data.IsInstalled) {
				return __result;
			}

			if (GInterface132.Area.Data.Template.Type == EAreaType.Workbench && (owner as HideoutPlayerOwner).AvailableForInteractions) {
				Slot pistolSlot = owner.Player.GetInventoryController().Inventory.Equipment.GetSlot(EquipmentSlot.Holster);
				if (pistolSlot != null) {
					Item pistol = pistolSlot.ContainedItem;
					if (pistol != null) {
						__result.Actions.Add(new GClass2303 {Name = $"MOD {pistol.LocalizedShortName()}", Action = () => { ItemUiContext.Instance.ModWeapon(pistol); }});
					}
				}

				Slot primarySlot = owner.Player.GetInventoryController().Inventory.Equipment.GetSlot(EquipmentSlot.FirstPrimaryWeapon);
				if (primarySlot != null) {
					Item primary = primarySlot.ContainedItem;
					if (primary != null) {
						__result.Actions.Add(new GClass2303 {Name = $"MOD {primary.LocalizedShortName()}", Action = () => { ItemUiContext.Instance.ModWeapon(primary); }});
					}
				}
				Slot secondarySlot = owner.Player.GetInventoryController().Inventory.Equipment.GetSlot(EquipmentSlot.SecondPrimaryWeapon);
				if (secondarySlot != null) {
					Item secondary = secondarySlot.ContainedItem;
					if (secondary != null) {
						__result.Actions.Add(new GClass2303 {Name = $"MOD {secondary.LocalizedShortName()}", Action = () => { ItemUiContext.Instance.ModWeapon(secondary); }});
					}
				}
			}

			return __result;
		}
	}
}