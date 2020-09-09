/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: HiraFileType.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Provides data regarding different types of files.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HiraSaves.FileManagement
{
    internal class HiraFileType
    {
        #region Constructor
        
        // fields and constructor
        private HiraFileType(string folderPath, string extension) { FolderPath = folderPath; Extension = extension; }
        
        #endregion

        internal readonly string FolderPath, Extension;

        #region PseudoEnumeration
        
        // general settings for any file
        private static readonly string root_folder = Application.persistentDataPath + root_folder_name;
        private const string root_folder_name = "/data/";

        // settings for a config file
        internal static readonly HiraFileType CONFIG =
            new HiraFileType(root_folder + config_folder_name, config_extension);

        private const string config_folder_name = "config/";
        private const string config_extension = ".hiracfg";

        // settings for a save file
        internal static readonly HiraFileType SAVE =
            new HiraFileType(root_folder + save_folder_name, save_extension);

        private const string save_folder_name = "save/";
        private const string save_extension = ".hirasave";
        
        #endregion

        // whether the folder exists; tries to create one if it doesn't and then checks again
        internal bool FolderCouldntBeInitialized
        {
            get
            {
                if (Directory.Exists(FolderPath)) return false;

                Directory.CreateDirectory(FolderPath);

                return !Directory.Exists(FolderPath);
            }
        }

        // names of files present in the path
        internal IEnumerable<string> FileList =>
            FolderCouldntBeInitialized
            
                ? null
                
                : from file in Directory.GetFiles(FolderPath)
                where file.EndsWith(Extension)
                select file.Replace(Extension, "")
                    .Replace(FolderPath, "");
    }
}