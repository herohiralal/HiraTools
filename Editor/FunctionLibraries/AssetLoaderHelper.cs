using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEditor
{
	public static class AssetLoaderHelper
	{
		public static void LoadAllAssetsOfType(this Type t)
		{
			var relevantAssets = AssetDatabase.FindAssets($"t:{t.FullName}");
			foreach (var relevantAsset in relevantAssets)
				AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(relevantAsset));
		}

		public static Object[] TrueFindObjectsOfTypeAll(this Type t)
		{
			t.LoadAllAssetsOfType();
			return Resources.FindObjectsOfTypeAll(t);
		}
	}
}