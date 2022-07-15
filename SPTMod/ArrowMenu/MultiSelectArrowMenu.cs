using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT;
using Nexus.NexAPI;
using UnityEngine;

namespace Nexus.SPTMod.ArrowMenu {
	public class MultiSelectArrowMenu : IBaseArrowMenu {
		private Action<Optional<IEnumerable<Player>>> _callback;
		private List<Int32> _ids = new List<Int32>();
		private Int32 _index;
		private List<Player> _players;

		public MultiSelectArrowMenu(Action<Optional<IEnumerable<Player>>> callback) {
			this._callback = callback;
			this._index = 1;
		}

		public void Update() {
			Optional<IEnumerable<Player>>? optional = null;
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

				if (Input.GetKeyDown(KeyCode.O)) {
					this._ids.Clear();
				}

				if (Input.GetKeyDown(KeyCode.K)) {
					this._ids = this._players.Where(p => !p.IsYourPlayer).Select(p => p.Id).ToList();
				}

				if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
					Int32 id = this._players[this._index].Id;
					if (!this._ids.Remove(id)) {
						this._ids.Add(id);
					}
				}

				if (Input.GetKeyDown(KeyCode.Return)) {
					IEnumerable<Player> result = this._ids.Count > 0 ? this._players.Where(p => this._ids.Contains(p.Id)) : null;
					optional = new Optional<IEnumerable<Player>>(result);
				}

				if (Input.GetKeyDown(KeyCode.Backspace)) {
					optional = new Optional<IEnumerable<Player>>();
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
			GUILayout.Label("Press right/left arrow to toggle current selection");
			GUILayout.Label("Press K to select all");
			GUILayout.Label("Press O to clear all");
			for (Int32 i = this._index; i < this._players.Count + 10; i++) {
				if (i > this._players.Count - 1) {
					continue;
				}

				Player player = this._players[i];
				Int32 id = player.Id;
				GUI.color = i == this._index ? Color.green : Color.red;
				GUILayout.Label($"[{(this._ids.Contains(id) ? "-" : "+")}] {id}: {player.Profile.Nickname.Localized()}");
			}
		}
	}
}