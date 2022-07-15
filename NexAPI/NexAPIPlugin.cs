using BepInEx;
using Nexus.NexAPI.CustomInteractions;
using Nexus.NexAPI.CustomItems;
using Nexus.NexAPI.Patches;

namespace Nexus.NexAPI {
	[BepInPlugin("com.pandahhcorp.nexapi", "NexAPI", "1.0.0")]
	public class NexAPIPlugin : BaseUnityPlugin {
		public static NexAPIPlugin Instance { get; private set; }
		public CustomItemHandlerManager CustomItemHandlerManager { get; private set; }
		public CustomInteractionsHandlerManager CustomInteractionsHandlerManager { get; private set; }
		public CustomItemHandlerPatch CustomItemHandlerPatch { get; private set; }
		public CustomInteractionsPatch CustomInteractionsPatch { get; private set; }
		public CustomHideoutInteractionsPatch CustomHideoutInteractionsPatch { get; private set; }

		private void Awake() {
			Instance = this;
			this.Logger.LogInfo("Loading: NexAPI");
			this.CustomItemHandlerManager = new CustomItemHandlerManager();
			this.CustomInteractionsHandlerManager = new CustomInteractionsHandlerManager();
			(this.CustomItemHandlerPatch = new CustomItemHandlerPatch()).Enable();
			(this.CustomInteractionsPatch = new CustomInteractionsPatch()).Enable();
			(this.CustomHideoutInteractionsPatch = new CustomHideoutInteractionsPatch()).Enable();
			this.Logger.LogInfo("Loaded: NexAPI");
		}
	}
}