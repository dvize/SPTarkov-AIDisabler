using System;
using System.IO;
using BepInEx;

namespace Nexus.SeparateTemp {
	[BepInPlugin("com.pandahhcorp.separatetemp", "SeparateTemp", "1.0.0")]
	public class SeparateTemp : BaseUnityPlugin {
		private void Awake() {
			this.Logger.LogInfo("Loading: SeparateTemp");
			typeof(GClass793).GetField("Path").SetValue(null, Path.Combine(Environment.CurrentDirectory, "Cache"));
			this.Logger.LogInfo("Loaded: SeparateTemp");
		}
	}
}