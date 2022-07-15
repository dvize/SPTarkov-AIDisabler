using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Comfort.Common;
using CommonAssets.Scripts.Game;
using EFT;
using EFT.InventoryLogic;
using Nexus.NexAPI;
using Nexus.NexAPI.CustomItems;
using Nexus.SPTMod.ArrowMenu;
using Nexus.SPTMod.Patches;
using UnityEngine;
using Object = System.Object;
using uObject = UnityEngine.Object;

namespace Nexus.SPTMod {
	public class DebugRimuru : CustomItemHandler {
		static DebugRimuru() {
			Type type = typeof(GClass1233);
			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
			_botZone_0 = type.GetField("botZone_0", flags);
			_int_3 = type.GetField("int_3", flags);
			_cancellationTokenSource_0 = type.GetField("cancellationTokenSource_0", flags);
			_method_12 = type.GetMethod("method_12", flags);
			_gclass1233_0 = typeof(BotControllerClass).GetField("gclass1233_0", flags);
		}

		public override IEnumerable<String> TemplateIds { get; } = new[] {"debug_rimuru"};

		private static void OnForceExtract() {
			(Singleton<AbstractGame>.Instance as EndByExitTrigerScenario.GInterface48).StopSession(Singleton<GameWorld>.Instance.RegisteredPlayers[0].ProfileId, ExitStatus.Survived, null);
		}

		private static void OnSpawnAI() {
			GClass1233 gclass = _gclass1233_0.GetValue((uObject.FindObjectOfType<AbstractGame>() as GInterface12).BotsController) as GClass1233;
			Player localPlayer = Singleton<GameWorld>.Instance.RegisteredPlayers[0];
			_int_3.SetValue(gclass, (Int32)_int_3.GetValue(gclass) + 1);
			BotZone botZone = (_botZone_0.GetValue(gclass) as BotZone[]).RandomElement();
			GClass555 data = new GClass555(SPTModPlugin.Instance.PlayerSide.Value, SPTModPlugin.Instance.WildSpawnType.Value, SPTModPlugin.Instance.BotDifficulty.Value, 0f);
			for (Int32 i = 0; i < SPTModPlugin.Instance.AmountToSpawn.Value; i++) {
				_method_12.Invoke(gclass, new Object[] {localPlayer.Transform.position, botZone, data, null, (_cancellationTokenSource_0.GetValue(gclass) as CancellationTokenSource).Token});
			}
		}

		private static void OnTeleportAIToMe() {
			if (SPTModPlugin.Instance.ArrowMenu == null) {
				Vector3 position = Singleton<GameWorld>.Instance.RegisteredPlayers[0].Position;
				SPTModPlugin.Instance.ArrowMenu = new MultiSelectArrowMenu(players => {
					SPTModPlugin.Instance.ArrowMenu = null;
					if (players.HasValue && players.Value != null) {
						foreach (Player player in players.Value) {
							if (player.HealthController != null && player.HealthController.IsAlive) {
								player.Teleport(position);
							}
						}
					}
				});
			}
		}

		private static void OnFullHeal() {
			GClass1804 localPlayer = Singleton<GameWorld>.Instance.RegisteredPlayers[0].ActiveHealthController;
			Array.ForEach(Enum.GetValues(typeof(EBodyPart)) as EBodyPart[] ?? throw new InvalidOperationException(), localPlayer.RemoveNegativeEffects);
			localPlayer.RestoreFullHealth();
		}

		private static void OnTeleportToAI() {
			if (SPTModPlugin.Instance.ArrowMenu == null) {
				SPTModPlugin.Instance.ArrowMenu = new SingleSelectArrowMenu(player => {
					SPTModPlugin.Instance.ArrowMenu = null;
					if (player.HasValue && player.Value != null && player.Value.HealthController != null && player.Value.HealthController.IsAlive) {
						Singleton<GameWorld>.Instance.RegisteredPlayers[0].Teleport(player.Value.Position);
					}
				});
			}
		}

		private static void OnToggleGodmode() {
			Singleton<GameWorld>.Instance.RegisteredPlayers[0].ActiveHealthController.SetDamageCoeff(Singleton<GameWorld>.Instance.RegisteredPlayers[0].ActiveHealthController.DamageCoeff < 0f ? 1f : -1f);
		}

		private static void OnCreateAllItems() {
			Player localPlayer = Singleton<GameWorld>.Instance.RegisteredPlayers[0];
			GClass2041 sortingTable = localPlayer.GetInventoryController().Inventory.SortingTable;
			GClass1864 mainGrid = sortingTable.Grid;
			foreach (Item item in Singleton<GClass1242>.Instance.CreateAllItemsEver()) {
				item.SpawnedInSession = true;
				item.StackObjectsCount = item.StackMaxSize;
				mainGrid.Add(item);
			}

			GamePlayerOwner gamePlayerOwner = localPlayer.gameObject.GetComponent<GamePlayerOwner>();
			gamePlayerOwner.CloseInventoryIfOpen();
			gamePlayerOwner.ShowInventoryScreenLoot(sortingTable, () => { sortingTable.Grid.RemoveAll(); });
		}

		private static void OnOpenInventory() {
			if (SPTModPlugin.Instance.ArrowMenu == null) {
				SPTModPlugin.Instance.ArrowMenu = new SingleSelectArrowMenu(player => {
					SPTModPlugin.Instance.ArrowMenu = null;
					if (player.HasValue && player.Value != null && player.Value.HealthController != null && player.Value.HealthController.IsAlive) {
						GamePlayerOwner gamePlayerOwner = Singleton<GameWorld>.Instance.RegisteredPlayers[0].gameObject.GetComponent<GamePlayerOwner>();
						gamePlayerOwner.CloseInventoryIfOpen();
						gamePlayerOwner.ShowInventoryScreenLoot(player.Value.GetInventoryController().Inventory.Equipment, () => { });
					}
				});
			}
		}

		private static void MakeCrouch() {
			if (SPTModPlugin.Instance.ArrowMenu == null) {
				SPTModPlugin.Instance.ArrowMenu = new MultiSelectArrowMenu(players => {
					SPTModPlugin.Instance.ArrowMenu = null;
					if (players.HasValue && players.Value != null) {
						foreach (Player player in players.Value) {
							if (player.HealthController != null && player.HealthController.IsAlive) {
								player.MovementContext.PlayerAnimatorSetPoseLevel(0f);
							}
						}
					}
				});
			}
		}

		private static void MakeProne() {
			if (SPTModPlugin.Instance.ArrowMenu == null) {
				SPTModPlugin.Instance.ArrowMenu = new MultiSelectArrowMenu(players => {
					SPTModPlugin.Instance.ArrowMenu = null;
					if (players.HasValue && players.Value != null) {
						foreach (Player player in players.Value) {
							if (player.HealthController != null && player.HealthController.IsAlive) {
								player.ToggleProne();
							}
						}
					}
				});
			}
		}

		private static void OnNoTarget() {
			NoTargetPatch.NoTargetActive = !NoTargetPatch.NoTargetActive;
		}

		public override IEnumerable<GClass2312> GenerateCallbacks() {
			GameWorld gameWorld;
			Player localPlayer;
			if (!Singleton<GameWorld>.Instantiated || (gameWorld = Singleton<GameWorld>.Instance).RegisteredPlayers.Count == 0 || (localPlayer = gameWorld.RegisteredPlayers[0]) is HideoutPlayer) {
				yield break;
			}

			yield return new GClass2312("Force Extract", OnForceExtract);
			yield return new GClass2312("Spawn AI", OnSpawnAI);

			if (gameWorld.RegisteredPlayers.Count > 1 && SPTModPlugin.Instance.ArrowMenu == null) {
				yield return new GClass2312("Teleport To AI", OnTeleportToAI);
				yield return new GClass2312("Teleport AI To Me", OnTeleportAIToMe);
				yield return new GClass2312("Open AI Inventory", OnOpenInventory);
				yield return new GClass2312("Make Crouch", MakeCrouch);
				yield return new GClass2312("Make Prone", MakeProne);
			}

			if (localPlayer.HealthController != null) {
				if (!localPlayer.ActiveHealthController.GetBodyPartHealth(EBodyPart.Common).AtMaximum) {
					yield return new GClass2312("Full Heal", OnFullHeal);
				}

				yield return new GClass2312($"Godmode: {(localPlayer.HealthController.DamageCoeff < 0f ? "deactivate" : "activate")}", OnToggleGodmode);
			}

			yield return new GClass2312("Create All Items", OnCreateAllItems);
			yield return new GClass2312($"NoTarget: {(NoTargetPatch.NoTargetActive ? "deactivate" : "activate")}", OnNoTarget);
		}

#region Reflection
		private static FieldInfo _botZone_0;
		private static FieldInfo _cancellationTokenSource_0;
		private static MethodInfo _method_12;
		private static FieldInfo _int_3;
		private static FieldInfo _gclass1233_0;
#endregion
	}
}