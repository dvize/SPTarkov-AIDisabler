using System;
using EFT.Interactive;

namespace Nexus.SPTMod {
	public readonly struct DoorState {
		public readonly String Id;
		public readonly EDoorState State;
		public readonly Boolean IsKeycard;
		public readonly Object Key;

		public DoorState(Door door) {
			this.Id = door.Id;
			this.State = door.DoorState;
			this.IsKeycard = door is KeycardDoor;
			this.Key = door.KeyId;
		}
	}
}