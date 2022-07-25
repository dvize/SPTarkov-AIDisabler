using System;
using System.IO;
using System.Reflection;
using BepInEx;

namespace Nexus.SeparateConfig {
	[BepInPlugin("com.pandahhcorp.separateconfig", "SeparateConfig", "1.0.0")]
	public class SeparateConfigPlugin : BaseUnityPlugin {
		private void Awake() {
			this.Logger.LogInfo("Loading: SeparateConfig");
			String targetDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"Battlestate Games", "Escape from Tarkov", "SPTSettings");
			if (!Directory.Exists(targetDirectory)) {
				Directory.CreateDirectory(targetDirectory);
			}

			typeof(GClass1630).GetField("string_1", BindingFlags.NonPublic | BindingFlags.Static)
				?.SetValue(null, targetDirectory);
			this.Logger.LogInfo("Loaded: SeparateConfig");
		}
	}
}