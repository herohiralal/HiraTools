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
        private static readonly string ROOT_FOLDER = Application.persistentDataPath + ROOT_FOLDER_NAME;
        private const string ROOT_FOLDER_NAME = "/data/";

        // settings for a config file
        internal static readonly HiraFileType CONFIG =
            new HiraFileType(ROOT_FOLDER + CONFIG_FOLDER_NAME, CONFIG_EXTENSION);

        private const string CONFIG_FOLDER_NAME = "config/";
        private const string CONFIG_EXTENSION = ".hiracfg";

        // settings for a save file
        internal static readonly HiraFileType SAVE =
            new HiraFileType(ROOT_FOLDER + SAVE_FOLDER_NAME, SAVE_EXTENSION);

        private const string SAVE_FOLDER_NAME = "save/";
        private const string SAVE_EXTENSION = ".hirasave";
        
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