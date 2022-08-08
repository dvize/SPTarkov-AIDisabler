using System;
using System.Linq;
using System.Reflection;
using Aki.Common.Http;
using Aki.Reflection.Patching;
using EFT;
using EFT.Interactive;
using Newtonsoft.Json;

namespace Nexus.SPTMod.Patches {
	public class SaveDoorStatePatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(ISession).Assembly.GetType("Class213").GetNestedType("Class214", BindingFlags.NonPublic)
				.GetMethod(nameof(ISession.OfflineRaidEnded));
		}

		[PatchPostfix]
		private static void Postfix(ExitStatus exitStatus, String exitName, Double raidSeconds) {
			RequestHandler.PostJson("/client/game/doorstates/set",
				JsonConvert.SerializeObject(new {
					exitStatus,
					exitName,
					raidSeconds,
					doors = LocationScene.GetAll<WorldInteractiveObject>().Where(o => o is Door).Cast<Door>()
						.Select(d => new DoorState(d))
				}));
		}
	}
}