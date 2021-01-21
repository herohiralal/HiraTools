using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace HiraEditor
{
    public class DoCreateHiraScript : EndNameEditAction
    {
        [MenuItem("Assets/Create/CreateScript")]
        private static void Create()
        {
            var scriptCreator = (HiraScriptCreator) Selection.activeObject;
            var icon = (Texture2D) EditorGUIUtility.IconContent("cs Script Icon").image;
            var sourcePath = AssetDatabase.GetAssetPath(scriptCreator);
            var sourceDir = Path.GetDirectoryName(sourcePath)?.Replace('\\', '/');
            var classPath = $"{sourceDir}/{scriptCreator.FileName}.cs";
            var creator = CreateInstance<DoCreateHiraScript>();

            if (File.Exists(classPath))
                ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, creator,
                    classPath, icon, scriptCreator.FileData);
            else
            {
                creator.Action(0, classPath, scriptCreator.FileData);
                creator.CleanUp();
            }
        }

        [MenuItem("Assets/Create/CreateScript", true)]
        private static bool ValidateCreation()
        {
            var activeObject = Selection.activeObject;
            return activeObject != null && activeObject is HiraScriptCreator;
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            File.WriteAllText(Path.GetFullPath(pathName), FixLineEndings(resourceFile), new UTF8Encoding(true));
            
            AssetDatabase.ImportAsset(pathName);
            
            var o = AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
            ProjectWindowUtil.ShowCreatedAsset(o);
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
}