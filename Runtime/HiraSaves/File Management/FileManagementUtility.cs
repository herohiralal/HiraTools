/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: FileManagementUtility.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Manages the file itself.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.IO;

namespace HiraEngine.HiraSaves.FileManagement
{
    internal static class FileManagementUtility
    {
        internal static FileStream GetFileStreamAs(this string fileName, HiraFileType type) => 
            File.Open(fileName.GetFullPathAs(type), FileMode.OpenOrCreate);

        internal static void DeleteFile(this HiraFileType type, string fileName)
        {
            var path = fileName.GetFullPathAs(type);
            if(File.Exists(path)) File.Delete(path);
        }

        private static string GetFullPathAs(this string saveName, HiraFileType fileType)
        {var name = string.IsNullOrWhiteSpace(saveName)
                ? DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")
                : saveName;

            return fileType.FolderPath + name + fileType.Extension;
        }
    }
}