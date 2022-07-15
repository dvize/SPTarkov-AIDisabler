using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT;
using Nexus.NexAPI;
using UnityEngine;

namespace Nexus.SPTMod.ArrowMenu {
	public class SingleSelectArrowMenu : IBaseArrowMenu {
		private Action<Optional<Player>> _callback;
		private Int32 _index;
		private List<Player> _players;

		public SingleSelectArrowMenu(Action<Optional<Player>> callback) {
			this._callback = callback;
			this._index = 1;
		}

		public void Update() {
			Optional<Player>? optional = null;
			if (Singleton<GameWorld>.Instantiated && (this._players = Singleton<GameWorld>.Instance.RegisteredPlayers) != null && this._players.Count != 0) {
				if (Input.GetKeyDown(KeyCode.DownArrow)) {
					this._index++;
				}

				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					this._index--;
				}

				if (this._index < 1) {
					this._index = this._players.Count - 1;
				}

				if (this._index > this._players.Count - 1) {
					this._index = 1;
				}

				if (Input.GetKeyDown(KeyCode.Backspace)) {
					optional = new Optional<Player>();
					this._callback?.Invoke(null);
				}

				if (Input.GetKeyDown(KeyCode.Return)) {
					Player player = this._players[this._index];
					optional = player.IsYourPlayer ? null : player;
				}
			}

			if (optional != null) {
				this._callback?.Invoke(optional.Value);
			}
		}

		public void OnGUI() {
			if (this._players == null) {
				return;
			}

			GUI.color = Color.yellow;
			GUILayout.Label("Press backspace to cancel");
			GUILayout.Label("Press enter to confirm");
			for (Int32 i = this._index; i < this._players.Count + 10; i++) {
				if (i > this._players.Count - 1) {
					continue;
				}

				Player player = this._players[i];
				Int32 id = player.Id;
				GUI.color = i == this._index ? Color.green : Color.red;
				GUILayout.Label($"{id}: {player.Profile.Nickname}");
			}
		}
	}
}