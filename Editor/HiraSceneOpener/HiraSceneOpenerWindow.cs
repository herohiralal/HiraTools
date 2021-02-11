using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace HiraEditor
{
	public class HiraSceneOpenerWindow : EditorWindow
	{
		private List<SceneOpener> _sceneAssets;
		private IMGUIContainer _toolbar = null;
		private ListView _list = null;

		[MenuItem("Window/General/Scene Opener &O", false, 10)]
		public static void GetSceneOpener() => GetWindow<HiraSceneOpenerWindow>().Focus();

		private void Awake()
		{
			_sceneAssets = new List<SceneOpener>();
			titleContent = "Scene Loader".GetGUIContent();
		}

		private void OnDestroy()
		{
			_sceneAssets = null;
		}

		public void OnEnable()
		{
			_toolbar = new IMGUIContainer(OnToolbarGUI);
			rootVisualElement.Add(_toolbar);

			RefreshSceneAssetList();
			_list = new ListView(_sceneAssets, 45, MakeItemForListView, BindItemForListView) {unbindItem = UnbindItemFromListView};
			_list.style.flexGrow = 1;
			rootVisualElement.Add(_list);
		}

		private void OnDisable()
		{
			rootVisualElement.Remove(_list);
			_list = null;

			rootVisualElement.Remove(_toolbar);
			_toolbar = null;
		}

		private void OnToolbarGUI()
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
					Refresh();

				GUILayout.FlexibleSpace();

				GUILayout.Label("Made by Rohan Jadav");
			}
		}

		private void Refresh()
		{
			RefreshSceneAssetList();
			_list.Refresh();
		}

		private void RefreshSceneAssetList()
		{
			var sceneAssets = AssetDatabase.FindAssets("t: scene")
				.Select(s => new SceneOpener(s));

			_sceneAssets.Clear();
			foreach (var asset in sceneAssets) _sceneAssets.Add(asset);
		}

		private void BindItemForListView(VisualElement createdVisualElement, int index)
		{
			var sceneOpener = _sceneAssets[index];
			createdVisualElement.tooltip = sceneOpener.Path;

			var label = createdVisualElement.Q<Label>("SceneNameLabel");
			if (label != null) label.text = sceneOpener.Name;

			var pathLabel = createdVisualElement.Q<Label>("ScenePathLabel");
			if (pathLabel != null) pathLabel.text = sceneOpener.Path;

			var openButton = createdVisualElement.Q<Button>("SceneOpenerButton");
			if (openButton != null) openButton.clickable.clicked += sceneOpener.Open;

			var openAdditiveButton = createdVisualElement.Q<Button>("AdditiveSceneOpenerButton");
			if (openAdditiveButton != null) openAdditiveButton.clickable.clicked += sceneOpener.OpenAdditive;

			var openAdditiveNoLoadButton = createdVisualElement.Q<Button>("AdditiveNoLoadSceneOpenerButton");
			if (openAdditiveNoLoadButton != null) openAdditiveNoLoadButton.clickable.clicked += sceneOpener.OpenAdditiveWithoutLoading;

			var closeButton = createdVisualElement.Q<Button>("SceneCloserButton");
			if (closeButton != null) closeButton.clickable.clicked += sceneOpener.Close;
		}

		private void UnbindItemFromListView(VisualElement createdVisualElement, int index)
		{
			var sceneOpener = _sceneAssets[index];
			createdVisualElement.tooltip = "";

			var label = createdVisualElement.Q<Label>("SceneNameLabel");
			if (label != null) label.text = "";

			var pathLabel = createdVisualElement.Q<Label>("ScenePathLabel");
			if (pathLabel != null) pathLabel.text = "";

			var openButton = createdVisualElement.Q<Button>("SceneOpenerButton");
			if (openButton != null) openButton.clickable.clicked -= sceneOpener.Open;

			var openAdditiveButton = createdVisualElement.Q<Button>("AdditiveSceneOpenerButton");
			if (openAdditiveButton != null) openAdditiveButton.clickable.clicked -= sceneOpener.OpenAdditive;

			var openAdditiveNoLoadButton = createdVisualElement.Q<Button>("AdditiveNoLoadSceneOpenerButton");
			if (openAdditiveNoLoadButton != null) openAdditiveNoLoadButton.clickable.clicked -= sceneOpener.OpenAdditiveWithoutLoading;

			var closeButton = createdVisualElement.Q<Button>("SceneCloserButton");
			if (closeButton != null) closeButton.clickable.clicked -= sceneOpener.Close;
		}

		private VisualElement MakeItemForListView()
		{
			var template = Resources.Load<VisualTreeAsset>("HiraSceneOpenerWindowListElement");
			var style = Resources.Load<StyleSheet>("HiraSceneOpenerWindowListElementStyling");
			var element = template.CloneTree();
			element.styleSheets.Add(style);
			return element;
		}
	}

	internal class SceneOpener
	{
		public SceneOpener(string sceneAssetGuid)
		{
			Path = AssetDatabase.GUIDToAssetPath(sceneAssetGuid);
			Name = ((SceneAsset) AssetDatabase.LoadMainAssetAtPath(Path)).name;
		}

		public readonly string Name;
		public readonly string Path;

		public void Open()
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene(Path, OpenSceneMode.Single);
		}

		public void OpenAdditive()
		{
			EditorSceneManager.OpenScene(Path, OpenSceneMode.Additive);
		}

		public void OpenAdditiveWithoutLoading()
		{
			EditorSceneManager.OpenScene(Path, OpenSceneMode.AdditiveWithoutLoading);
		}

		public void Close()
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(Path), true);
		}
	}
}