using System;
using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace Nexus.NexAPI {
	public static class Utils {
		private static Texture2D _lineText = new Texture2D(1, 1);
		public static Vector2 CenterScreen { get; } = new Vector2(Screen.width, Screen.height) / 2f;

		public static void DrawLine(Vector2 pointA, Vector2 pointB, Boolean correctY, Color? color = null, Single thickness = 1f) {
			if (correctY) {
				pointA.y = Screen.height - pointA.y;
				pointB.y = Screen.height - pointB.y;
			}

			Matrix4x4 matrix = GUI.matrix;
			Color color2 = GUI.color;
			GUI.color = color ?? Color.green;
			Single num = Vector3.Angle(pointB - pointA, Vector2.right);
			if (pointA.y > pointB.y) {
				num = -num;
			}

			GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, thickness), new Vector2(pointA.x, pointA.y + 0.5f));
			GUIUtility.RotateAroundPivot(num, pointA);
			GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), _lineText);
			GUI.matrix = matrix;
			GUI.color = color2;
		}

		public static void DrawLine(Single x1, Single y1, Single x2, Single y2, Boolean correctY, Color? color = null, Single thickness = 1f) {
			DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), correctY, color, thickness);
		}

		public static Boolean CreateShot([NotNull] Player player, [NotNull] Weapon weapon, Vector3 position, Vector3 direction, BulletClass bullet) {
			BallisticsCalculator sharedBallisticsCalculator;
			if (player == null || weapon == null || !Singleton<GameWorld>.Instance || (sharedBallisticsCalculator = Singleton<GameWorld>.Instance._sharedBallisticsCalculator) == null) {
				return false;
			}

			Singleton<GameWorld>.Instance._sharedBallisticsCalculator.Shoot(sharedBallisticsCalculator.CreateShot(bullet, position, direction, 0, player, weapon));

			return true;
		}

		public static GClass2110 GetInventoryController(this Player player) {
			return typeof(Player).GetField("_inventoryController", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player) as GClass2110;
		}

		public static Boolean ContainsIgnoreCase(this String source, String toCheck, StringComparison comp = StringComparison.InvariantCultureIgnoreCase) {
			return source?.IndexOf(toCheck, comp) >= 0;
		}

		public static Boolean IsItemTemplateOfType(ItemTemplate template, String parentId) {
			GClass1028 instance = Singleton<GClass1242>.Instance.ItemTemplates;
			while (!String.IsNullOrEmpty(template._parent)) {
				if (template._parent == parentId) {
					return true;
				}

				template = instance[template._parent];
			}

			return false;
		}

		public static Boolean IsItemTemplateOfType(String templateId, String parentId) {
			return Singleton<GClass1242>.Instance.ItemTemplates.TryGetValue(templateId, out ItemTemplate itemTemplate) && IsItemTemplateOfType(itemTemplate, parentId);
		}

		public static T CreateItem<T>(String templateId, String itemId = null) where T : Item {
			if (Singleton<GClass1242>.Instantiated) {
				GClass1242 gclass1242 = Singleton<GClass1242>.Instance;
				if (gclass1242.ItemTemplates.ContainsKey(templateId)) {
					if (itemId == null) {
						itemId = GenerateId();
					}

					return gclass1242.CreateItem(itemId, templateId, null) as T;
				}
			}

			return null;
		}

		public static String GenerateId() {
			return Guid.NewGuid().ToString("N").Substring(0, 24);
		}
	}
}