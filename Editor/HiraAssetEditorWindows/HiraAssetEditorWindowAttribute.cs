using System;

namespace HiraEditor.HiraAssetEditorWindows
{
	public class HiraAssetEditorWindowAttribute : Attribute
	{
		public readonly Type AssetType;

		public HiraAssetEditorWindowAttribute(Type assetType)
		{
			AssetType = assetType;
		}
	}
}