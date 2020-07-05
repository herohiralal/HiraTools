/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: HiraFile.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Access the data within a serialized file.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using HiraSaves.Serialization;
// ReSharper disable UnusedMember.Global

namespace HiraSaves.FileManagement
{
    public class HiraFile<T> where T : class
    {
        #region Factory

        private HiraFile(string fileName, HiraFileType type)
        {
            this.type = type;
            this.fileName = fileName;
        }

        internal static HiraFile<T> Get(string fileName, HiraFileType type) => new HiraFile<T>(fileName, type);

        #endregion

        private readonly string fileName;
        private readonly HiraFileType type;

        public T Data
        {
            get
            {
                if (type.FolderCouldntBeInitialized) return null;

                using (var file = fileName.GetFileStreamAs(type))
                    return FormatterUtility.Formatter.Deserialize(file) as T;
            }

            set
            {
                if (type.FolderCouldntBeInitialized) return;

                if (value == null) type.DeleteFile(fileName);
                else
                    using (var file = fileName.GetFileStreamAs(type)) 
                        FormatterUtility.Formatter.Serialize(file, value);
            }
        }

        public override string ToString() =>
            "Saves to, and loads from data in " + fileName + " as " + nameof(T) + ".";
    }
}