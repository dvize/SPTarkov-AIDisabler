using System;
using System.Collections.Generic;
using Comfort.Common;
using CommonAssets.Scripts.Game;
using EFT;
using EFT.InventoryLogic;
using Nexus.NexAPI;
using Nexus.NexAPI.CustomItems;
using Nexus.SPTMod.ArrowMenu;
using Nexus.SPTMod.Patches;
using UnityEngine;
using uObject = UnityEngine.Object;

namespace Nexus.SPTMod {
	public class DebugRimuru : CustomItemHandler {
		public override IEnumerable<String> TemplateIds { get; } = new[] {"debug_rimuru"};

		private static void OnForceExtract() {
			(Singleton<AbstractGame>.Instance as EndByExitTrigerScenario.GInterface53)?.StopSession(
				Singleton<GameWorld>.Instance.RegisteredPlayers[0].ProfileId, ExitStatus.Survived, null);
		}

		private static void OnTeleportAIToMe() {
			if (SPTModPlugin.Instance.ArrowMenu != null) {
				return;
			}

			Vector3 position = Singleton<GameWorld>.Instance.RegisteredPlayers[0].Position;
			SPTModPlugin.Instance.ArrowMenu = new MultiSelectArrowMenu(players => {
				SPTModPlugin.Instance.ArrowMenu = null;
				if (!players.HasValue || players.Value == null) {
					return;
				}

				foreach (Player player in players.Value) {
					if (player.HealthController != null && player.HealthController.IsAlive) {
						player.Teleport(position);
					}
				}
			});
		}

		private static void OnFullHeal() {
			GClass1887 localPlayer = Singleton<GameWorld>.Instance.RegisteredPlayers[0].ActiveHealthController;
			Array.ForEach(Enum.GetValues(typeof(EBodyPart)) as EBodyPart[] ?? throw new InvalidOperationException(),
				localPlayer.RemoveNegativeEffects);
			localPlayer.RestoreFullHealth();
		}

		private static void OnTeleportToAI() {
			if (SPTModPlugin.Instance.ArrowMenu != null) {
				return;
			}

			SPTModPlugin.Instance.ArrowMenu = new SingleSelectArrowMenu(player => {
				SPTModPlugin.Instance.ArrowMenu = null;
				if (player.HasValue && player.Value != null && player.Value.HealthController != null &&
					player.Value.HealthController.IsAlive) {
					Singleton<GameWorld>.Instance.RegisteredPlayers[0].Teleport(player.Value.Position);
				}
			});
		}

		private static void OnToggleGodmode() {
			Singleton<GameWorld>.Instance.RegisteredPlayers[0].ActiveHealthController.SetDamageCoeff(
				Singleton<GameWorld>.Instance.RegisteredPlayers[0].ActiveHealthController.DamageCoeff < 0f ? 1f : -1f);
		}

		private static void OnCreateAllItems() {
			Player localPlayer = Singleton<GameWorld>.Instance.RegisteredPlayers[0];
			GClass2126 sortingTable = localPlayer.GetInventoryController().Inventory.SortingTable;
			GClass1947 mainGrid = sortingTable.Grid;
			foreach (Item item in Singleton<GClass1307>.Instance.CreateAllItemsEver()) {
				item.SpawnedInSession = true;
				item.StackObjectsCount = item.StackMaxSize;
				mainGrid.Add(item);
			}

			GamePlayerOwner gamePlayerOwner = localPlayer.gameObject.GetComponent<GamePlayerOwner>();
			gamePlayerOwner.CloseInventoryIfOpen();
			gamePlayerOwner.ShowInventoryScreenLoot(sortingTable, () => {
				sortingTable.Grid.RemoveAll();
			});
		}

		private static void OnOpenInventory() {
			if (SPTModPlugin.Instance.ArrowMenu != null) {
				return;
			}

			SPTModPlugin.Instance.ArrowMenu = new SingleSelectArrowMenu(player => {
				SPTModPlugin.Instance.ArrowMenu = null;
				if (!player.HasValue || player.Value == null || player.Value.HealthController == null ||
					!player.Value.HealthController.IsAlive) {
					return;
				}

				GamePlayerOwner gamePlayerOwner = Singleton<GameWorld>.Instance.RegisteredPlayers[0].gameObject
					.GetComponent<GamePlayerOwner>();
				gamePlayerOwner.CloseInventoryIfOpen();
				gamePlayerOwner.ShowInventoryScreenLoot(player.Value.GetInventoryController().Inventory.Equipment,
					() => {
					});
			});
		}

		private static void OnNoTarget() {
			NoTargetPatch.NoTargetActive = !NoTargetPatch.NoTargetActive;
		}

		public override IEnumerable<GClass2396> GenerateCallbacks() {
			GameWorld gameWorld;
			Player localPlayer;
			if (!Singleton<GameWorld>.Instantiated ||
				(gameWorld = Singleton<GameWorld>.Instance).RegisteredPlayers.Count == 0 ||
				(localPlayer = gameWorld.RegisteredPlayers[0]) is HideoutPlayer) {
				yield break;
			}

			yield return new GClass2396("Force Extract", OnForceExtract);
			if (gameWorld.RegisteredPlayers.Count > 1 && SPTModPlugin.Instance.ArrowMenu == null) {
				yield return new GClass2396("Teleport To AI", OnTeleportToAI);
				yield return new GClass2396("Teleport AI To Me", OnTeleportAIToMe);
				yield return new GClass2396("Open AI Inventory", OnOpenInventory);
			}

			if (localPlayer.HealthController != null) {
				if (!localPlayer.ActiveHealthController.GetBodyPartHealth(EBodyPart.Common).AtMaximum) {
					yield return new GClass2396("Full Heal", OnFullHeal);
				}

				yield return new GClass2396(
					$"Godmode: {(localPlayer.HealthController.DamageCoeff < 0f ? "deactivate" : "activate")}",
					OnToggleGodmode);
			}

			yield return new GClass2396("Create All Items", OnCreateAllItems);
			yield return new GClass2396($"NoTarget: {(NoTargetPatch.NoTargetActive ? "deactivate" : "activate")}",
				OnNoTarget);
		}
	}
}