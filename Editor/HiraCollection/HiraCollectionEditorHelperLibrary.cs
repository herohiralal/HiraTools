using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace HiraEditor.HiraCollection
{
	public static class HiraCollectionEditorHelperLibrary
	{
		private static readonly Dictionary<string, GUIContent> gui_content_cache = new Dictionary<string, GUIContent>();

		internal static GUIContent GetGUIContent(this string textAndTooltip)
		{
			if (string.IsNullOrEmpty(textAndTooltip))
				return GUIContent.none;

			if (gui_content_cache.TryGetValue(textAndTooltip, out var content)) return content;

			var s = textAndTooltip.Split('|');
			content = new GUIContent(s[0]);

			if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
				content.tooltip = s[1];

			gui_content_cache.Add(textAndTooltip, content);

			return content;
		}

		internal static void DrawSplitter()
		{
			var rect = GUILayoutUtility.GetRect(1f, 1f);

			rect.xMin = 0f;
			rect.width += 4f;

			if (Event.current.type != EventType.Repaint)
				return;

			EditorGUI.DrawRect(rect, HiraCollectionStyling.SplitterColor);
		}

		internal static bool DrawHeader(SerializedProperty group, SerializedObject targetObject,
			Action reset, Action remove, Action moveUp, Action moveDown, ref bool renameMode)
		{
			Assert.IsNotNull(targetObject);
			Assert.IsNotNull(group);

			var backgroundRect = GUILayoutUtility.GetRect(1f, 25f);

			var labelRect = backgroundRect;
			labelRect.x += 5;
			labelRect.xMin += 32f;
			labelRect.xMax -= 55f;

			var foldoutRect = backgroundRect;
			foldoutRect.y += 6f;
			foldoutRect.width = 13f;
			foldoutRect.height = 13f;

			var toggleRect = backgroundRect;
			toggleRect.x += 16f;
			toggleRect.y += 3f;
			toggleRect.width = 19f;
			toggleRect.height = 19f;

			var menuIcon = HiraCollectionStyling.MenuIcon;
			var menuRect = new Rect(labelRect.xMax + 44f, labelRect.y + 2f, menuIcon.width, menuIcon.height);

			var leftRect = menuRect;
			leftRect.x -= 40f;

			var rightRect = menuRect;
			rightRect.x -= 20f;

			// Background rect should be full-width
			backgroundRect.xMin = 0f;
			backgroundRect.width += 4f;

			// Background
			EditorGUI.DrawRect(backgroundRect, HiraCollectionStyling.HeaderColor);

			// renameMode = EditorGUI.Toggle(toggleRect, "I", renameMode, EditorStyles.miniButton);
			if (GUI.Button(toggleRect, "I", EditorStyles.miniButton))
			{
				renameMode = !renameMode;
			}

			if (renameMode)
			{
				labelRect.y += 4;
				targetObject.targetObject.name = EditorGUI.TextField(labelRect, GUIContent.none,
					targetObject.targetObject.name, EditorStyles.miniTextField);
			}
			else
			{
				EditorGUI.LabelField(labelRect, targetObject.targetObject.name, EditorStyles.boldLabel);
			}

			// foldout
			group.serializedObject.Update();
			group.isExpanded = GUI.Toggle(foldoutRect, group.isExpanded, GUIContent.none, EditorStyles.foldout);
			group.serializedObject.ApplyModifiedProperties();

			// Active checkbox
			targetObject.Update();
			targetObject.ApplyModifiedProperties();

			// Special buttons
			GUI.DrawTexture(menuRect, menuIcon);
			if (moveUp != null) GUI.DrawTexture(leftRect, HiraCollectionStyling.LeftIcon);
			if (moveDown != null) GUI.DrawTexture(rightRect, HiraCollectionStyling.RightIcon);

			// Handle events
			var e = Event.current;

			if (e.type == EventType.MouseDown)
			{
				if (menuRect.Contains(e.mousePosition))
				{
					ShowHeaderContextMenu(new Vector2(menuRect.x, menuRect.yMax), reset, remove);
					e.Use();
				}
				else if (leftRect.Contains(e.mousePosition) && e.button == 0 && moveUp != null)
				{
					moveUp();
					Debug.Log("move");
					e.Use();
				}
				else if (rightRect.Contains(e.mousePosition) && e.button == 0 && moveDown != null)
				{
					moveDown();
					Debug.Log("move");
					e.Use();
				}
				else if (labelRect.Contains(e.mousePosition))
				{
					if (e.button == 0)
						group.isExpanded = !group.isExpanded;
					else
					{
						ShowHeaderContextMenu(e.mousePosition, reset, remove);
					}

					e.Use();
				}
			}

			return group.isExpanded;
		}

		private static void ShowHeaderContextMenu(Vector2 position, Action reset, Action remove)
		{
			Assert.IsNotNull(reset);
			Assert.IsNotNull(remove);

			var menu = new GenericMenu();
			menu.AddItem("Reset".GetGUIContent(), false, reset.Invoke);
			menu.AddItem("Remove".GetGUIContent(), false, remove.Invoke);

			menu.DropDown(new Rect(position, Vector2.zero));
		}
	}
}