using System;
using System.Diagnostics;
using BepInEx;
using BepInEx.Configuration;
using EFT;
using EFT.UI;
using Nexus.NexAPI;
using Nexus.SPTMod.ArrowMenu;
using Nexus.SPTMod.Patches;

namespace Nexus.SPTMod {
	[BepInPlugin("com.pandahhcorp.sptmod", "SPTMod", "1.0.0")]
	[BepInDependency("com.pandahhcorp.nexapi")]
	public class SPTModPlugin : BaseUnityPlugin {
		public static SPTModPlugin Instance { get; private set; }
		public ConfigEntry<Boolean> InfiniteAmmo { get; private set; }
		public ConfigEntry<Boolean> DeathMarker { get; private set; }
		public ConfigEntry<Boolean> InspectWhileSearching { get; private set; }
		public IBaseArrowMenu ArrowMenu { get; set; }
		public CustomShootingRange CustomShootingRange { get; private set; }

		private void Awake() {
			this.Logger.LogInfo("Loading: SPTMod");
			Instance = this;
			ConsoleScreen.Processor.RegisterCommand("clear", () => PreloaderUI.Instance.Console.Clear());
			ConsoleScreen.Processor.RegisterCommand("quit", () => Process.GetCurrentProcess().Kill());
			this.InfiniteAmmo = this.Config.Bind("Player", "Infinite Ammo", false);
			this.DeathMarker = this.Config.Bind("Player", "Death Marker", true);
			this.InspectWhileSearching = this.Config.Bind("Player", "Inspect While Searching", true);
			new InfiniteAmmoPatch().Enable();
			new NoTargetPatch().Enable();
			new RemoveConsoleBackgroundPatch().Enable();
			new ContextMenuWhileSearchingPatch().Enable();
			new DeathMarkerPatch().Enable();
			NexAPIPlugin.Instance.CustomItemHandlerManager.RegisterHandler(new DebugRimuru());
			NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(this.CustomShootingRange =
				new CustomShootingRange());
			NexAPIPlugin.Instance.CustomInteractionsHandlerManager.RegisterHandler(new CustomWorkbench());
			this.Logger.LogInfo("Loaded: SPTMod");
		}

		private void Update() {
			this.ArrowMenu?.Update();
		}

		private void OnGUI() {
			this.ArrowMenu?.OnGUI();
		}
	}
}