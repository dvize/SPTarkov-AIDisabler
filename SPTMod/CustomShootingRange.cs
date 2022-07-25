using System;
using System.Collections.Generic;
using System.Reflection;
using Comfort.Common;
using DeferredDecals;
using EFT;
using Nexus.NexAPI.CustomInteractions;
using Systems.Effects;

namespace Nexus.SPTMod {
	public class CustomShootingRange : CustomInteractionsHandler {
		private static FieldInfo _maxDecals;
		private static MethodInfo _setDirtyMethod;

		static CustomShootingRange() {
			_maxDecals =
				typeof(DeferredDecalRenderer).GetField("_maxDecals", BindingFlags.NonPublic | BindingFlags.Instance);
			_setDirtyMethod =
				typeof(DeferredDecalRenderer).GetMethod("method_13", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public HideoutTargetController TargetController { get; private set; }

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

			if (this.TargetController == null) {
				this.TargetController = new HideoutTargetController();
			}

			if (!this.TargetController.IsInitialized) {
				this.TargetController.Initialize();
				if (!this.TargetController.IsInitialized) {
					return __result;
				}
			}

			if (!(interactive is GInterface137 ginterface137) ||
				ginterface137.Area.Data.Template.Type != EAreaType.ShootingRange ||
				!((HideoutPlayerOwner)owner).AvailableForInteractions) {
				return __result;
			}

			GClass2387 switchCommand = null;
			if (__result.Actions.Count > 1) {
				switchCommand = __result.Actions[1];
				__result.Actions.RemoveAt(1);
			}

			__result.Actions.Add(new GClass2387 {Action = OnClearDecals, Name = "CLEAR DECALS"});
			for (Int32 i = 0; i < 3; i++) {
				Int32 targetIndex = i;
				__result.Actions.Add(new GClass2387 {
					Action = () => {
						this.TargetController.Toggle(targetIndex);
					},
					Name = $"Toggle Target {i}"
				});
			}

			if (switchCommand != null) {
				__result.Actions.Add(switchCommand);
			}

			return __result;
		}

		private static void OnClearDecals() {
			DeferredDecalRenderer decalSystem = Singleton<Effects>.Instance.DeferredDecals;
			decalSystem.SetMaxDynamicDecals((Int32)_maxDecals.GetValue(decalSystem));
			_setDirtyMethod.Invoke(decalSystem, null);
		}
	}
}