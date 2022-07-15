using System.Reflection;
using Aki.Reflection.Patching;
using DG.Tweening;
using EFT;
using UnityEngine;

namespace Nexus.SPTMod.Patches {
	public class DeathMarkerPatch : ModulePatch {
		protected override MethodBase GetTargetMethod() {
			return typeof(Player).GetMethod("OnDead", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		[PatchPostfix]
		private static void Postfix(Player __instance) {
			if (!SPTModPlugin.Instance.DeathMarker.Value || __instance.IsYourPlayer) {
				return;
			}

			GameObject gameObject = new GameObject("Image");
			Transform transform = gameObject.transform;
			transform.SetParent(__instance.PlayerBody.transform, false);
			transform.localPosition = new Vector3(0f, 1.1f, 0f);
			transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			SpriteRenderer image = gameObject.AddComponent<SpriteRenderer>();
			image.sprite = GClass716.Load<Sprite>("spirit");
			gameObject.AddComponent<PrimitiveBillboard>();
			image.color = new Color(1f, 1f, 1f, 0.3f);
			image.DOFade(0f, 3f);
			transform.DOMoveY(transform.position.y + 5f, 3f);
		}
	}
}