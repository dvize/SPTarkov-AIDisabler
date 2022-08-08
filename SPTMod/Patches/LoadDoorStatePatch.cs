using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Aki.Common.Http;
using Aki.Reflection.Patching;
using EFT.Interactive;
using EFT.UI;
using Newtonsoft.Json;

namespace Nexus.SPTMod.Patches {
	public class LoadDoorStatePatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(ISession).Assembly.GetType("Class213").GetNestedType("Class214", BindingFlags.NonPublic)
				.GetMethod(nameof(ISession.OfflineRaidStarted));
		}

		[PatchPostfix]
		private static async Task Postfix(Task __result) {
			await __result;
			DoorState[] obj =
				(JsonConvert.DeserializeObject(RequestHandler.GetJson("/client/game/doorstates/get"),
					 typeof(DoorState[])) ??
				 throw new Exception("Failed to deserialize server response")) as DoorState[];
			if (obj == null) {
				ConsoleScreen.LogError("It's null lmao");
			}

			ConsoleScreen.Log($"Deserialized object: {obj.Length} first is: {JsonConvert.SerializeObject(obj[0])}");
			Dictionary<String, DoorState> doorStates = obj.ToDictionary(d => d.Id);
			ConsoleScreen.Log("Dictionarized");
			MethodInfo methodInfo =
				typeof(WorldInteractiveObject).GetMethod("method_2", BindingFlags.NonPublic | BindingFlags.Instance);
			if (methodInfo == null) {
				ConsoleScreen.LogError("Failed to find WorldInteractiveObject.method_2");

				return;
			}

			foreach (WorldInteractiveObject worldInteractiveObject in LocationScene.GetAll<WorldInteractiveObject>()
						 .Where(o => o is Door)) {
				if (!(worldInteractiveObject is Door door)) {
					continue;
				}

				if (!doorStates.TryGetValue(door.Id, out DoorState state)) {
					continue;
				}

				if (door is KeycardDoor) {
					if (state.Key is IEnumerable<String> keys) {
						typeof(KeycardDoor).GetField("_additionalKeys", BindingFlags.NonPublic | BindingFlags.Instance)
							?.SetValue(door, keys.ToArray());
					}
					else {
						door.KeyId = state.Key.ToString();
					}
				}
				else {
					if (door.KeyId != null) {
						door.KeyId = state.Key.ToString();
					}
				}

				methodInfo.Invoke(door, new Object[] {state.State, true});
			}
		}
	}
}