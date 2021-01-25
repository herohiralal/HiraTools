using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace HiraEditor
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
    public class DoCreateHiraScript : ScriptableObject//EndNameEditAction
    {
        [MenuItem("Assets/Convert to Script", true)]
        private static bool ValidateCreation() => ValidateObjectForScriptConversion(Selection.activeObject);
        
        [MenuItem("Assets/Convert to Script")]
        private static void Create() => CreateScriptFrom(Selection.activeObject);

        private static bool ValidateObjectForScriptConversion(Object activeObject) =>
            activeObject != null && activeObject is ScriptableObject && activeObject is IHiraScriptCreator;

        private static void CreateScriptFrom(Object activeObject)
        {
            var scriptCreator = (IHiraScriptCreator) activeObject;
            var fileData = scriptCreator.FileData;

            var creator = CreateInstance<DoCreateHiraScript>();
            creator.dependencies = new List<ScriptableObject>();
            foreach (var dependency in scriptCreator.Dependencies)
            {
                if (ValidateObjectForScriptConversion(dependency))
                    creator.dependencies.Add(dependency);
            }

            var sourceDir = Path
                .GetDirectoryName(AssetDatabase.GetAssetPath(activeObject))
                ?.Replace('\\', '/');
            var classPath = $"{sourceDir}/{scriptCreator.FileName}.cs";

            if (classPath != scriptCreator.CachedFilePath)
            {
                creator.toDelete = scriptCreator.CachedFilePath;
                scriptCreator.CachedFilePath = classPath;
            }
            
            creator.Action(classPath, fileData);
        }

        [SerializeField] private string toDelete = "";
        [SerializeField] private List<ScriptableObject> dependencies = null;

        private void Action(string pathName, string resourceFile)
        {
            File.WriteAllText(Path.GetFullPath(pathName), FixLineEndings(resourceFile), new UTF8Encoding(true));
            
            AssetDatabase.ImportAsset(pathName);
            
            var o = AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
            ProjectWindowUtil.ShowCreatedAsset(o);

            if (!string.IsNullOrWhiteSpace(toDelete) && AssetDatabase.LoadMainAssetAtPath(toDelete) != null)
                AssetDatabase.DeleteAsset(toDelete);

            foreach (var dependency in dependencies) CreateScriptFrom(dependency);
            DestroyImmediate(this);
        }

        private static string FixLineEndings(string content)
        {
            const string windowsLineEndings = "\r\n";
            const string unixLineEndings = "\n";

            var preferredLineEndings = EditorSettings.lineEndingsForNewScripts switch
            {
                LineEndingsMode.OSNative => Application.platform == RuntimePlatform.WindowsEditor
                    ? windowsLineEndings
                    : unixLineEndings,
                LineEndingsMode.Unix => unixLineEndings,
                LineEndingsMode.Windows => windowsLineEndings,
                _ => unixLineEndings
            };

            content = Regex.Replace(content, @"\r\n?|\n", preferredLineEndings);

            return content;
        }
    }
#endif
}