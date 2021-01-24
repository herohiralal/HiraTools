using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace HiraEditor
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
    public class DoCreateHiraScript : EndNameEditAction
    {
        [MenuItem("Assets/Convert to Script")]
        private static void Create()
        {
            var activeObject = Selection.activeObject;
            var scriptCreator = (IHiraScriptCreator) activeObject;
            var fileData = scriptCreator.FileData;
            
            var creator = CreateInstance<DoCreateHiraScript>();
            
            var sourceDir = Path
                .GetDirectoryName(AssetDatabase.GetAssetPath(activeObject))
                ?.Replace('\\', '/');
            var classPath = $"{sourceDir}/{scriptCreator.FileName}.cs";

            if (classPath != scriptCreator.CachedFilePath)
            {
                creator.toDelete = scriptCreator.CachedFilePath;
                scriptCreator.CachedFilePath = classPath;
            }
            
            if (AssetDatabase.LoadMainAssetAtPath(classPath) == null)
                ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                    creator,
                    classPath,
                    (Texture2D) EditorGUIUtility.IconContent("cs Script Icon").image,
                    fileData);
            else
            {
                creator.Action(0, classPath, fileData);
                creator.CleanUp();
            }
        }

        [MenuItem("Assets/Convert to Script", true)]
        private static bool ValidateCreation()
        {
            var activeObject = Selection.activeObject;
            return activeObject != null && activeObject is IHiraScriptCreator;
        }

        [SerializeField] private string toDelete = "";

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            File.WriteAllText(Path.GetFullPath(pathName), FixLineEndings(resourceFile), new UTF8Encoding(true));
            
            AssetDatabase.ImportAsset(pathName);
            
            var o = AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
            ProjectWindowUtil.ShowCreatedAsset(o);

            if (!string.IsNullOrWhiteSpace(toDelete) && AssetDatabase.LoadMainAssetAtPath(toDelete) != null)
                AssetDatabase.DeleteAsset(toDelete);
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