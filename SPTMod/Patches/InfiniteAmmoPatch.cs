using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.Ballistics;
using EFT.InventoryLogic;
using Nexus.NexAPI;

namespace Nexus.SPTMod.Patches {
	public class InfiniteAmmoPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(BallisticsCalculator).GetMethod(nameof(BallisticsCalculator.Shoot));
		}

		[PatchPostfix]
		private static void Postfix(GClass2286 shot) {
			if (SPTModPlugin.Instance.InfiniteAmmo.Value && shot.Player.IsYourPlayer && shot.Weapon is Weapon weapon) {
				if (weapon.IsMultiBarrel) {
					Slot emptySlot = weapon.Chambers.FirstOrDefault(slot => slot.ContainedItem == null);
					if (emptySlot != null) {
						emptySlot.Add(Utils.CreateItem<Item>(shot.Ammo.TemplateId) ?? shot.Ammo);
					}
				}
				else if (weapon is GClass2050 gClass2050) {
					
				}
				else {
					GClass2023 magazine = weapon.GetCurrentMagazine();
					if (magazine != null) {
						if (magazine.Cartridges == null || magazine.Cartridges.Count == 0) {
							(shot.Player.HandsController as Player.FirearmController).ReloadRevolverDrum(
							new GClass1860(new List<BulletClass> {(Utils.CreateItem<Item>(shot.Ammo.TemplateId) ?? shot.Ammo) as BulletClass}), null);
						}
						else {
							magazine.Cartridges?.Add(Utils.CreateItem<Item>(shot.Ammo.TemplateId) ?? shot.Ammo, false);
						}
					}
				}
			}
		}
	}
}