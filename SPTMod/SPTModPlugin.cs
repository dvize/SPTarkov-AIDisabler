using System;
using System.Diagnostics;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using EFT.UI;
using Nexus.NexAPI;
using Nexus.SPTMod.ArrowMenu;
using Nexus.SPTMod.Patches;
using UnityEngine;

namespace Nexus.SPTMod {
	[BepInPlugin("com.pandahhcorp.sptmod", "SPTMod", "1.0.0")]
	[BepInDependency("com.pandahhcorp.nexapi")]
	public class SPTModPlugin : BaseUnityPlugin {
		private static String _exceptionPath =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "sptmod.txt");

		public static SPTModPlugin Instance { get; private set; }
		public ConfigEntry<Boolean> InfiniteAmmo { get; private set; }
		public ConfigEntry<Boolean> DeathMarker { get; private set; }
		public ConfigEntry<Boolean> ContextWhileSearching { get; private set; }
		public ConfigEntry<KeyCode> PreventInstantHideout { get; private set; }
		public IBaseArrowMenu ArrowMenu { get; set; }
		public CustomShootingRange CustomShootingRange { get; private set; }

		private void Awake() {
			try {
				if (File.Exists(_exceptionPath)) {
					File.Delete(_exceptionPath);
				}

				this.Logger.LogInfo("Loading: SPTMod");
				Instance = this;
				ConsoleScreen.Processor.RegisterCommand("clear", () => PreloaderUI.Instance.Console.Clear());
				ConsoleScreen.Processor.RegisterCommand("quit", () => Process.GetCurrentProcess().Kill());
				this.InfiniteAmmo = this.Config.Bind("Player", "Infinite Ammo", false);
				this.DeathMarker = this.Config.Bind("Player", "Death Marker", true);
				this.ContextWhileSearching = this.Config.Bind("Player", "Context While Searching", true);
				new InfiniteAmmoPatch().Enable();
				new NoTargetPatch().Enable();
				new RemoveConsoleBackgroundPatch().Enable();
				new ContextMenuWhileSearchingPatch().Enable();
				new LoadDoorStatePatch().Enable();
				new SaveDoorStatePatch().Enable();
				new DeathMarkerPatch().Enable();
				new RagfairWindowBuyAll().Enable();
				NexAPIPlugin.Instance.CustomItemHandlerManager.RegisterHandler(new DebugRimuru());
				NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(this.CustomShootingRange =
					new CustomShootingRange());






				/* All of this has to do with my mod to remove the main menu and put you boots on ground in hideout
				this.PreventInstantHideout =
					this.Config.Bind("Hideout Gameplay", "Prevent Instant Hideout", KeyCode.F1);
				new StraightToHideoutPatch().Enable();
				new NeuterHideoutControls().Enable();
				new EnterHideoutPatch().Enable();
				new RemoveTradingToggles().Enable();
				new MenuTaskBarPatch().Enable();
				NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(new CustomWorkbench());
				NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(new CustomSecurityDoor());
				NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(new HideoutBuild());
				NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(new CustomIntelligenceCenter());
				NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(new CustomGenerator());
				*/
				this.Logger.LogInfo("Loaded: SPTMod");
			}
			catch (Exception ex) {
				File.WriteAllText(_exceptionPath, ex.ToString());
			}
		}

		private void Update() {
			this.ArrowMenu?.Update();
		}

		private void OnGUI() {
			this.ArrowMenu?.OnGUI();
		}
	}
}