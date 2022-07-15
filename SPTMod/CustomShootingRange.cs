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
			_maxDecals = typeof(DeferredDecalRenderer).GetField("_maxDecals", BindingFlags.NonPublic | BindingFlags.Instance);
			_setDirtyMethod = typeof(DeferredDecalRenderer).GetMethod("method_13", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		public HideoutTargetController TargetController { get; private set; }

		public override IEnumerable<Type> AffectedTypes { get; } = new[] {typeof(GInterface132)};

		public override Boolean AffectsRaid {
			get { return false; }
		}

		public override GClass2304 GetInteractions(GClass2304 __result, GamePlayerOwner owner, GInterface73 interactive) {
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

			GClass2303 switchCommand = null;
			if (__result.Actions.Count > 1) {
				switchCommand = __result.Actions[1];
				__result.Actions.RemoveAt(1);
			}

			if ((interactive as GInterface132).Area.Data.Template.Type == EAreaType.ShootingRange && (owner as HideoutPlayerOwner).AvailableForInteractions) {
				__result.Actions.Add(new GClass2303 {Action = this.OnClearDecals, Name = "CLEAR DECALS"});
				for (Int32 i = 0; i < 3; i++) {
					Int32 targetIndex = i;
					__result.Actions.Add(new GClass2303 {Action = () => { this.TargetController.Toggle(targetIndex); }, Name = $"Toggle Target {i}"});
				}
			}

			if (switchCommand != null) {
				__result.Actions.Add(switchCommand);
			}

			return __result;
		}

		private void OnClearDecals() {
			DeferredDecalRenderer decalSystem = Singleton<Effects>.Instance.DeferredDecals;
			decalSystem.SetMaxDynamicDecals((Int32)_maxDecals.GetValue(decalSystem));
			_setDirtyMethod.Invoke(decalSystem, null);
		}
	}
}