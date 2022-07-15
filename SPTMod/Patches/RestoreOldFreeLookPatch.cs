using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;

namespace Nexus.SPTMod.Patches {
	public class RestoreOldFreeLookPatch : ModulePatch {
		private static FieldInfo _float0;
		private static FieldInfo _float1;
		private static FieldInfo _bool1;
		private static FieldInfo _bool2;

		static RestoreOldFreeLookPatch() {
			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
			Type type = typeof(Player);
			_float0 = type.GetField("float_0", flags);
			_float1 = type.GetField("float_1", flags);
			_bool1 = type.GetField("bool_1", flags);
			_bool2 = type.GetField("bool_2", flags);
		}
		
		protected override MethodBase GetTargetMethod() {
			return typeof(Player).GetMethod(nameof(Player.Look), BindingFlags.Public | BindingFlags.Instance);
		}

		[PatchPrefix]
		private static Boolean Prefix(Player __instance, Single deltaLookY, Single deltaLookX, Boolean withReturn) {
			if (!SPTModPlugin.Instance.RestoreOldFreelook.Value || !__instance.IsYourPlayer || __instance.HandsController == null || !__instance.HandsController.IsAiming) {
				return true;
			}

			Vector2 first = new Vector2(-40f, 40f);
			Vector2 second = new Vector2(-50f, 20f);
			Single float0 = Mathf.Clamp((Single)_float0.GetValue(__instance) - deltaLookY, first.x, first.y);
			Single float1 = Mathf.Clamp((Single)_float1.GetValue(__instance) + deltaLookX, second.x, second.y);
			_float0.SetValue(__instance, float0);
			_float1.SetValue(__instance, float1);
			Single x = float1 > 0f ? float1 * (1f - float0 / 40f * (float0 / 40f)) : float1;
			if ((Boolean)_bool1.GetValue(__instance)) {
				_bool2.SetValue(__instance, false);
				_bool1.SetValue(__instance, false);
				deltaLookY = 0f;
				deltaLookX = 0f;
			}

			if (Math.Abs(deltaLookY) >= 1E-45f && Math.Abs(deltaLookX) >= 1E-45f) {
				_bool2.SetValue(__instance, true);
			}

			if (!(Boolean)_bool2.GetValue(__instance) && withReturn) {
				float0 = Mathf.Abs(float0) > 0.01f ? Mathf.Lerp(float0, 0f, Time.deltaTime * 15f) : 0f;
				_float0.SetValue(__instance, float0);
				_float1.SetValue(__instance, Mathf.Abs(float1) > 0.01f ? Mathf.Lerp(float1, 0f, Time.deltaTime * 15f) : 0f);
			}

			__instance.HeadRotation = new Vector3(x, float0, 0f);
			__instance.ProceduralWeaponAnimation.SetHeadRotation(__instance.HeadRotation);

			return false;
		}
	}
}