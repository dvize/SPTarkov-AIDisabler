using System;
using System.Collections.Generic;
using EFT;
using EFT.Hideout;
using Nexus.NexAPI.CustomInteractions;

namespace Nexus.SPTMod {
	public class HideoutBuild : CustomInteractionsHandler {
		public override IEnumerable<Type> AffectedTypes { get; } = new[] {typeof(GInterface137)};

		public override GClass2388
			GetInteractions(GClass2388 __result, GamePlayerOwner owner, GInterface79 interactive) {
			if (!(interactive is GInterface137 gInterface137) || gInterface137.Area == null ||
				gInterface137.Area.Data == null) {
				return __result;
			}

			if (gInterface137.Area.Data.Status == EAreaStatus.ReadyToInstallConstruct ||
				gInterface137.Area.Data.Status == EAreaStatus.ReadyToConstruct) {
				__result.Actions.Insert(0, new GClass2387 {
					Action = () => {
						gInterface137.Area.Data.UpgradeAction();
					},
					Name = "Construct"
				});
			}

			if (gInterface137.Area.Data.Status == EAreaStatus.ReadyToInstallUpgrade ||
				gInterface137.Area.Data.Status == EAreaStatus.ReadyToUpgrade) {
				__result.Actions.Insert(0, new GClass2387 {
					Action = () => {
						gInterface137.Area.Data.UpgradeAction();
					},
					Name = "Upgrade"
				});
			}

			return __result;
		}
	}
}