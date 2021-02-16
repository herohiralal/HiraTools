using UnityEngine;
using UnityEditor;

namespace HiraEditor.Internal
{
	public static class HiraCollectionStyling
	{
		public static Texture2D MenuIcon => (Texture2D) (EditorGUIUtility.isProSkin
			? EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png")
			: EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png"));
		
		public static Texture2D RightIcon => (Texture2D) (EditorGUIUtility.isProSkin
			? EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/ArrowNavigationRight.png")
			: EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/ArrowNavigationRight.png"));
		
		public static Texture2D LeftIcon => (Texture2D) (EditorGUIUtility.isProSkin
			? EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/ArrowNavigationLeft.png")
			: EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/ArrowNavigationLeft.png"));

		public static Color SplitterColor => EditorGUIUtility.isProSkin
			? new Color(0.12f, 0.12f, 0.12f, 1.333f)
			: new Color(0.6f, 0.6f, 0.6f, 1.333f);

		public static Color HeaderColor => EditorGUIUtility.isProSkin
			? new Color(0.1f, 0.1f, 0.1f, 0.2f)
			: new Color(1f, 1f, 1f, 0.2f);
	}
}