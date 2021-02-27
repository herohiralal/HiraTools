using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace HiraEditor.Internal
{
    [CreateAssetMenu]
    public class UnmanagedPluginImporter : ScriptableObject
    {
#pragma warning disable CS0414
        [HiraButton("UpdateDefines")]
        [SerializeField] private Stub updateDefines = default;
#pragma warning restore CS0414

        [UsedImplicitly]
        private void UpdateDefines()
        {
            var pluginsFolderPath = Path.GetDirectoryName
            (
                Path.Combine
                (
                    Application.dataPath
                        .Replace("/Assets", "")
                        .Replace("\\Assets", ""),
                    AssetDatabase.GetAssetPath(this)
                )
            );
            
            if (string.IsNullOrWhiteSpace(pluginsFolderPath))
            {
                Debug.LogError("Could not find directory name.");
                return;
            }

            var pluginDefinesPath = Path.Combine(pluginsFolderPath, $".Source/{name}/Defines.h");
            if (!File.Exists(pluginDefinesPath))
            {
                Debug.LogError("File not found.");
                return;
            }

            var sb = new StringBuilder(500);

            sb
                .AppendLine("// ReSharper disable All")
                .AppendLine("#pragma once")
                .AppendLine("");

            var activeDefines = EditorUserBuildSettings.activeScriptCompilationDefines;
            foreach (var activeDefine in activeDefines)
                sb.AppendLine($"#define {activeDefine} 1");
            
            File.WriteAllText(pluginDefinesPath, sb.ToString());
        }
    }
}