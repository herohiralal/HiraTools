using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HiraEditor
{
    internal static class UnmanagedPluginDefinesUpdater
    {
        [MenuItem("Assets/Update Native Plugin Defines")]
        private static void UpdateNativePluginDefines()
        {
            var activeObject = Selection.activeObject;

            var pluginDefinesPath = Path.Combine(
                Application.dataPath
                    .Replace("/Assets", "")
                    .Replace("\\Assets", ""),
                $"NativeDefines/{activeObject.name}.h");

            var sb = new StringBuilder(500);

            sb
                .AppendLine(@"// ReSharper disable All")
                .AppendLine(@"#pragma once")
                .AppendLine(@"")
                .AppendLine(@"#define NULL nullptr")
                .AppendLine(@"");

            var pluginAPIPath = Path.Combine(EditorApplication.applicationContentsPath, "PluginAPI");
            var pluginAPIFiles = new DirectoryInfo(pluginAPIPath).GetFiles("*.h");
            foreach (var pluginAPIFile in pluginAPIFiles)
            {
                if (!pluginAPIFile.Name.Contains("Graphics"))
                    sb.AppendLine($"#include \"{pluginAPIFile.FullName}\"");
            }

            var activeDefines = EditorUserBuildSettings.activeScriptCompilationDefines;
            foreach (var activeDefine in activeDefines)
                sb
                    .AppendLine(@"")
                    .AppendLine($"#if !defined({activeDefine})")
                    .AppendLine($"    #define {activeDefine} 1")
                    .AppendLine(@"#else")
                    .AppendLine($"    #error Unity-related native-define \"{activeDefine}\" has been defined elsewhere.")
                    .AppendLine(@"#endif");

            File.WriteAllText(pluginDefinesPath, sb.ToString());
        }

        [MenuItem("Assets/Update Native Plugin Defines", true)]
        private static bool ValidateUpdateNativePluginDefines()
        {
            var activeObject = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(activeObject);

            return activeObject is DefaultAsset && path.EndsWith(".dll");
        }
    }
}