using System;
using DG.Tweening;
using UnityEngine;

namespace Nexus.SPTMod {
	public class HideoutTargetController {
		private Transform _firstTarget;
		private Transform _secondTarget;
		private Transform _thirdTarget;

		public Boolean IsInitialized {
			get {
				return this._firstTarget != null && this._secondTarget != null && this._thirdTarget != null;
			}
		}

		public Boolean IsUp(Int32 targetIndex) {
			switch (targetIndex) {
				case 0: {
					return this._firstTarget.transform.Find("metal_target_LOD0").position.y > -1f &&
						   this._firstTarget.transform.Find("metal_target_LOD1").position.y > -1f;
				}
				case 1: {
					return this._secondTarget.transform.Find("metal_target_LOD0").position.y > -1f &&
						   this._secondTarget.transform.Find("metal_target_LOD1").position.y > -1f;
				}
				case 2: {
					return this._thirdTarget.transform.Find("metal_target_LOD0").position.y > -1f &&
						   this._thirdTarget.transform.Find("metal_target_LOD1").position.y > -1f;
				}
				default:
					return false;
			}
		}

		public void Initialize() {
			this._firstTarget = GameObject.Find("metal_target")?.transform;
			this._secondTarget = GameObject.Find("metal_target (1)")?.transform;
			this._thirdTarget = GameObject.Find("metal_target (2)")?.transform;
			if (this._firstTarget == null || this._secondTarget == null || this._thirdTarget == null) {
				return;
			}

			this._firstTarget.Find("Effect Object Appearance")?.gameObject.SetActive(true);
			this._secondTarget.Find("Effect Object Appearance")?.gameObject.SetActive(true);
			this._thirdTarget.Find("Effect Object Appearance")?.gameObject.SetActive(true);
		}

		public Transform[] GetTargetGameObjects(Int32 targetIndex) {
			switch (targetIndex) {
				case 0: {
					return new[] {
						this._firstTarget.Find("metal_target_LOD0"), this._firstTarget.Find("metal_target_LOD1")
					};
				}
				case 1: {
					return new[] {
						this._secondTarget.Find("metal_target_LOD0"), this._secondTarget.Find("metal_target_LOD1")
					};
				}
				case 2: {
					return new[] {
						this._thirdTarget.Find("metal_target_LOD0"), this._thirdTarget.Find("metal_target_LOD1")
					};
				}
				default:
					return null;
			}
		}

		public ParticleSystem GetParticleSystem(Int32 targetIndex) {
			switch (targetIndex) {
				case 0: {
					return this._firstTarget.Find("Effect Object Appearance").GetComponent<ParticleSystem>();
				}
				case 1: {
					return this._secondTarget.Find("Effect Object Appearance").GetComponent<ParticleSystem>();
				}
				case 2: {
					return this._thirdTarget.Find("Effect Object Appearance").GetComponent<ParticleSystem>();
				}
				default:
					return null;
			}
		}

		public Transform GetBallisticTransform(Int32 targetIndex) {
			switch (targetIndex) {
				case 0: {
					return this._firstTarget.Find("ballistic");
				}
				case 1: {
					return this._secondTarget.Find("ballistic");
				}
				case 2: {
					return this._thirdTarget.Find("ballistic");
				}
				default:
					return null;
			}
		}

		public void Toggle(Int32 targetIndex) {
			if (!this.IsInitialized) {
				this.Initialize();
			}

			targetIndex = Mathf.Clamp(targetIndex, 0, 2);
			Transform[] transforms = this.GetTargetGameObjects(targetIndex);
			ParticleSystem particleSystem = this.GetParticleSystem(targetIndex);
			if (transforms == null || particleSystem == null || particleSystem.isPlaying) {
				return;
			}

			Transform ballistic = this.GetBallisticTransform(targetIndex);
			if (ballistic == null) {
				return;
			}

			particleSystem.Play();
			Boolean isUp = this.IsUp(targetIndex);
			ballistic.gameObject.SetActive(!isUp);
			Single targetY = isUp ? -1.85f : 0f;
			foreach (Transform transform in transforms) {
				transform.DOMoveY(targetY, 1.2f);
			}
		}
	}
}