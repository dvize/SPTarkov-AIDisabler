using System;
using System.Collections.Generic;

namespace Nexus.NexAPI.CustomItems {
	public abstract class CustomItemHandler {
		public GClass2161 Context { get; set; }
		public virtual IEnumerable<String> TemplateIds { get; } = Array.Empty<String>();
		public virtual IEnumerable<Type> Types { get; } = Array.Empty<Type>();

		public virtual IEnumerable<GClass2312> GenerateCallbacks() {
			return Array.Empty<GClass2312>();
		}
	}
}