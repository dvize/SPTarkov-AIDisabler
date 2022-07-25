using System;
using System.Collections.Generic;
using EFT;
using JetBrains.Annotations;

namespace Nexus.NexAPI.CustomInteractions {
	public abstract class CustomInteractionsHandler {
		/// <summary>
		///     Search through this method and see what type(s) you want to override
		/// </summary>
		public abstract IEnumerable<Type> AffectedTypes { get; }

		public virtual Boolean AffectsHideout {
			get {
				return true;
			}
		}

		public virtual Boolean AffectsRaid {
			get {
				return true;
			}
		}

		/// <summary>
		///     Will need to cast owner to HideoutPlayerOwner manually
		///     <param name="interactive">Can be null, if so then that means we are no longer looking at it</param>
		/// </summary>
		public abstract GClass2388 GetInteractions(GClass2388 __result, GamePlayerOwner owner,
			[CanBeNull] GInterface79 interactive);
	}
}