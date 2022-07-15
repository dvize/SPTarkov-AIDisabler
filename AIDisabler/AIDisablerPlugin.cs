using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace Nexus.AIDisabler {
	[BepInPlugin("com.pandahhcorp.aidisabler", "AIDisabler", "1.0.0")]
	public class AIDisablerPlugin : BaseUnityPlugin {
		private ConfigEntry<Single> _configRange;
		private Transform _mainCameraTransform;

		private void Awake() {
			this.Logger.LogInfo("Loading: AIDisabler");
			this._configRange = this.Config.Bind("General", "Range", 100f, "All AI outside of this range will be disabled");
			this.Logger.LogInfo("Loaded: AIDisabler");
		}

		private void FixedUpdate() {
			if (Singleton<GameWorld>.Instantiated) {
				if (this._mainCameraTransform != null) {
					if (Singleton<GameWorld>.Instance.RegisteredPlayers.Count > 1) {
						Vector3 cameraPosition = this._mainCameraTransform.position;
						foreach (Player player in Singleton<GameWorld>.Instance.RegisteredPlayers.Where(player => !player.IsYourPlayer)) {
							player.enabled = Vector3.Distance(cameraPosition, player.Position) <= this._configRange.Value;
						}
					}
				}
				else {
					if (GClass1687.Instance.Camera != null) {
						this._mainCameraTransform = GClass1687.Instance.Camera.transform;
					}
				}
			}
			else {
				this._mainCameraTransform = null;
			}
		}
	}
}