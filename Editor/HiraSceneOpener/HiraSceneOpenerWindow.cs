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
        private static SceneOpener[] _sceneAssets;
        private IMGUIContainer _toolbar = null;
        private ListView _list = null;
        static HiraSceneOpenerWindow() => RebuildSceneData();

        [MenuItem("Window/General/Scene Opener &O", false, 10)]
        public static void GetSceneOpener()
        {
            GetWindow<HiraSceneOpenerWindow>().Focus();
        }

        private static void RebuildSceneData() =>
            _sceneAssets = AssetDatabase.FindAssets("t: scene")
                .Select(s => new SceneOpener(s))
                .ToArray();

        private void Awake() => titleContent = "Scene Loader".GetGUIContent();

        public void OnEnable()
        {
            _toolbar = new IMGUIContainer(OnToolbarGUI);
            rootVisualElement.Add(_toolbar);

            CreateList();
        }

        private void OnDisable()
        {
            DeleteList();

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
            DeleteList();
            RebuildSceneData();
            CreateList();
            rootVisualElement.MarkDirtyRepaint();
        }

        private void CreateList()
        {
            _list = new ListView(_sceneAssets, 20, MakeItemForListView, BindItemForListView) {unbindItem = UnbindItemFromListView};
            _list.style.flexGrow = 1;
            rootVisualElement.Add(_list);
        }

        private void DeleteList()
        {
            
            rootVisualElement.Remove(_list);
            _list = null;
        }

        private void BindItemForListView(VisualElement createdVisualElement, int index)
        {
            var sceneOpener = _sceneAssets[index];

            var label = createdVisualElement.Q<Label>(className: "SceneNameLabel");
            if (label != null)
            {
                label.text = sceneOpener.Name;
                label.tooltip = sceneOpener.Path;
            }

            var openButton = createdVisualElement.Q<Button>("SceneOpenerButton");
            if (openButton != null)
            {
                openButton.text = "O";
                openButton.tooltip = $"Open {sceneOpener.Name}.";
                openButton.clickable.clicked += sceneOpener.Open;
            }

            var openAdditiveButton = createdVisualElement.Q<Button>("AdditiveSceneOpenerButton");
            if (openAdditiveButton != null)
            {
                openAdditiveButton.text = "+";
                openAdditiveButton.tooltip = $"Open {sceneOpener.Name} additively.";
                openAdditiveButton.clickable.clicked += sceneOpener.OpenAdditive;
            }

            var openAdditiveNoLoadButton = createdVisualElement.Q<Button>("AdditiveNoLoadSceneOpenerButton");
            if (openAdditiveNoLoadButton != null)
            {
                openAdditiveNoLoadButton.text = "!";
                openAdditiveNoLoadButton.tooltip = $"Open {sceneOpener.Name} additively without loading.";
                openAdditiveNoLoadButton.clickable.clicked += sceneOpener.OpenAdditiveWithoutLoading;
            }

            var closeButton = createdVisualElement.Q<Button>("SceneCloserButton");
            if (closeButton != null)
            {
                closeButton.text = "-";
                closeButton.tooltip = $"Close {sceneOpener.Name}.";
                closeButton.clickable.clicked += sceneOpener.Close;
            }
        }

        private void UnbindItemFromListView(VisualElement createdVisualElement, int index)
        {
            var sceneOpener = _sceneAssets[index];

            var label = createdVisualElement.Q<Label>(className: "SceneNameLabel");
            if (label != null)
            {
                label.text = "";
                label.tooltip = "";
            }

            var openButton = createdVisualElement.Q<Button>("SceneOpenerButton");
            if (openButton != null)
            {
                openButton.text = "";
                openButton.tooltip = "";
                openButton.clickable.clicked -= sceneOpener.Open;
            }

            var openAdditiveButton = createdVisualElement.Q<Button>("AdditiveSceneOpenerButton");
            if (openAdditiveButton != null)
            {
                openAdditiveButton.text = "";
                openAdditiveButton.tooltip = "";
                openAdditiveButton.clickable.clicked -= sceneOpener.OpenAdditive;
            }

            var openAdditiveNoLoadButton = createdVisualElement.Q<Button>("AdditiveNoLoadSceneOpenerButton");
            if (openAdditiveNoLoadButton != null)
            {
                openAdditiveNoLoadButton.text = "";
                openAdditiveNoLoadButton.tooltip = "";
                openAdditiveNoLoadButton.clickable.clicked -= sceneOpener.OpenAdditiveWithoutLoading;
            }

            var closeButton = createdVisualElement.Q<Button>("SceneCloserButton");
            if (closeButton != null)
            {
                closeButton.text = "";
                closeButton.tooltip = "";
                closeButton.clickable.clicked -= sceneOpener.Close;
            }
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