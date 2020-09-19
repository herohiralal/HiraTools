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

using HiraEngine.HiraSaves.Serialization;
// ReSharper disable UnusedMember.Global

namespace HiraEngine.HiraSaves.FileManagement
{
    public class HiraFile<T> where T : class
    {
        #region Factory

        private HiraFile(string fileName, HiraFileType type)
        {
            _type = type;
            _fileName = fileName;
        }

        internal static HiraFile<T> Get(string fileName, HiraFileType type) => new HiraFile<T>(fileName, type);

        #endregion

        private readonly string _fileName;
        private readonly HiraFileType _type;

        public T Data
        {
            get
            {
                if (_type.FolderCouldntBeInitialized) return null;

                using (var file = _fileName.GetFileStreamAs(_type))
                    return FormatterUtility.Formatter.Deserialize(file) as T;
            }

            set
            {
                if (_type.FolderCouldntBeInitialized) return;

                if (value == null) _type.DeleteFile(_fileName);
                else
                    using (var file = _fileName.GetFileStreamAs(_type)) 
                        FormatterUtility.Formatter.Serialize(file, value);
            }
        }

        public override string ToString() =>
            "Saves to, and loads from data in " + _fileName + " as " + nameof(T) + ".";
    }
}