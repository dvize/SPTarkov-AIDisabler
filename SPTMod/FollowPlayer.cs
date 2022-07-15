using System;
using EFT;
using UnityEngine;

namespace Nexus.SPTMod {
	public class FollowPlayer : MonoBehaviour {
		private Single _movementSpeed = 3f;
		private Player _player;
		private Rigidbody _rigidbody;

		private void Update() {
			if (this._player == null) {
				return;
			}

			this._rigidbody.MovePosition((this._player.Position - this.transform.position) * (this._movementSpeed * Time.deltaTime));
		}

		public void Init(Player player) {
			this._player = player;
			this._rigidbody = this.gameObject.GetOrAddComponent<Rigidbody>();
		}
	}
}