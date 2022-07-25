using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus.NexAPI.CustomInteractions {
	public class CustomInteractionsHandlerManager {
		private readonly List<CustomInteractionsHandler> _registeredHandlers = new List<CustomInteractionsHandler>();

		public void RegisterHandler(CustomInteractionsHandler handler) {
			if (!this._registeredHandlers.Contains(handler)) {
				this._registeredHandlers.Add(handler);
			}
		}

		public Boolean TryGetHandlers(Type type, Boolean isHideout, out List<CustomInteractionsHandler> handlers) {
			handlers = new List<CustomInteractionsHandler>();
			foreach (CustomInteractionsHandler handler in this._registeredHandlers) {
				if (handler.AffectsHideout && isHideout && handler.AffectedTypes.Any(t => t.IsAssignableFrom(type))) {
					handlers.Add(handler);
				}

				if (handler.AffectsRaid && handler.AffectedTypes.Any(t => t.IsAssignableFrom(type))) {
					handlers.Add(handler);
				}
			}

			return handlers.Count > 0;
		}
	}
}