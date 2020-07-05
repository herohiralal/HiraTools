/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: HiraSaveUtility.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Access to the saving API.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;
using System.Linq;
using HiraSaves.FileManagement;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace UnityEngine
{
    public static class HiraSaveUtility
    {
        public static HiraFile<T> AccessSave<T>(string fileName) where T : class =>
            HiraFile<T>.Get(fileName, HiraFileType.SAVE);
        public static HiraFile<T> AccessConfig<T>(string fileName) where T : class =>
            HiraFile<T>.Get(fileName, HiraFileType.CONFIG);
        public static IEnumerable<string> SaveFilesList => HiraFileType.SAVE.FileList;
        public static IEnumerable<string> ConfigFilesList => HiraFileType.CONFIG.FileList;

        public static bool SaveFileExists(string fileName) => SaveFilesList.Contains(fileName);
        public static bool ConfigFileExists(string fileName) => ConfigFilesList.Contains(fileName);
    }
}