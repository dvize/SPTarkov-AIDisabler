using System;
using System.Collections.Generic;
using System.Reflection;
using BSG.CameraEffects;
using EFT;
using EFT.UI;
using Nexus.NexAPI.CustomInteractions;
using Object = UnityEngine.Object;

namespace Nexus.SPTMod {
	public class CustomSecurityDoor : CustomInteractionsHandler {
		public override IEnumerable<Type> AffectedTypes { get; } = new[] {typeof(GInterface137)};

		public override Boolean AffectsRaid {
			get {
				return false;
			}
		}

		public override GClass2388
			GetInteractions(GClass2388 __result, GamePlayerOwner owner, GInterface79 interactive) {
			if (interactive == null) {
				return __result;
			}

			if (!(interactive is GInterface137 ginterface137) || ginterface137.Area == null ||
				ginterface137.Area.Data == null || ginterface137.Area.Data.Template.Type != EAreaType.Security) {
				return __result;
			}

			__result.Actions.Insert(0, new GClass2387 {Action = OnExitHideout, Name = "EXIT HIDEOUT"});
			__result.Actions.Insert(1, new GClass2387 {Action = OnVisitTraders, Name = "VISIT TRADERS"});

			return __result;
		}

		private static void OnExitHideout() {
			MainApplication mainApplication = Object.FindObjectOfType<MainApplication>();
			MainMenuController mainMenuController = (MainMenuController)typeof(MainApplication)
				.GetField("gclass1583_0", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(mainApplication);
			mainMenuController?.ShowScreen(EMenuType.Play, true);
			Object.FindObjectOfType<NightVision>().enabled = false;
		}

		private static void OnVisitTraders() {
			MainApplication mainApplication = Object.FindObjectOfType<MainApplication>();
			MainMenuController mainMenuController = (MainMenuController)typeof(MainApplication)
				.GetField("gclass1583_0", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(mainApplication);
			mainMenuController?.ShowScreen(EMenuType.Trade, true);
		}
	}
}