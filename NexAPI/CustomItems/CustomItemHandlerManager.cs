using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT.UI;
using JetBrains.Annotations;

namespace Nexus.NexAPI.CustomItems {
	public class CustomItemHandlerManager {
		private List<CustomItemHandler> _registeredHandlers = new List<CustomItemHandler>();

		public void RegisterHandler(CustomItemHandler handler) {
			// Maybe TODO: Re-implement checks to see if item(s)/type(s) are already being handled. Merge them or refuse?
			this._registeredHandlers.Add(handler);
		}

		public Boolean TryGetHandler([NotNull] String templateId, [NotNull] Type type, out CustomItemHandler handler) {
			handler = default;
			if (String.IsNullOrEmpty(templateId) || !Singleton<GClass1242>.Instantiated || !Singleton<GClass1242>.Instance.ItemTemplates.ContainsKey(templateId)) {
				ConsoleScreen.LogError($"Failed to find template");
				
				return false;
			}

			handler = this._registeredHandlers.FirstOrDefault(h => h.TemplateIds.Contains(templateId));
			if (handler != null) {
				return true;
			}

			handler = this._registeredHandlers.FirstOrDefault(h => h.Types.Any(t => t.IsAssignableFrom(type)));

			return handler != null;
		}
	}
}