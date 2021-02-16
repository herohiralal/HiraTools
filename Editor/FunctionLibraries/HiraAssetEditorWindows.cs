using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace HiraEditor
{
	public interface IHiraAssetEditorWindow
	{
		string SelectedGuid { get; set; }
	}
	
	public class HiraAssetEditorWindowAttribute : Attribute
	{
		public readonly Type AssetType;

		public HiraAssetEditorWindowAttribute(Type assetType)
		{
			AssetType = assetType;
		}
	}
	
	namespace Internal
	{
		public static class HiraAssetEditorWindowUtility
		{
			[OnOpenAsset]
			public static bool OnOpenHiraAsset(int instanceID, int line)
			{
				var targetObject = EditorUtility.InstanceIDToObject(instanceID);
				var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(instanceID));
				var targetType = targetObject.GetType();

				var hiraAssetEditorType =
					typeof(HiraAssetEditorWindowAttribute)
						.GetTypesWithThisAttribute()
						.FirstOrDefault
						(t =>
							t.GetCustomAttribute<HiraAssetEditorWindowAttribute>().AssetType == targetType &&
							typeof(EditorWindow).IsAssignableFrom(t) &&
							typeof(IHiraAssetEditorWindow).IsAssignableFrom(t)
						);

				if (hiraAssetEditorType == null) return false;

				foreach (var w in Resources.FindObjectsOfTypeAll(hiraAssetEditorType))
				{
					if (((IHiraAssetEditorWindow) w).SelectedGuid != guid) continue;

					((EditorWindow) w).Focus();
					return true;
				}

				var window = (EditorWindow) ScriptableObject.CreateInstance(hiraAssetEditorType);
				window.titleContent = targetObject.name.GetGUIContent();
				window.Show();
				((IHiraAssetEditorWindow) window).SelectedGuid = guid;
				window.Focus();
				return true;
			}
		}
	}
}