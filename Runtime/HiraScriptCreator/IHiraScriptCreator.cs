namespace UnityEngine
{
    public interface IHiraScriptCreator
    {
        string CachedFilePath { get; set; }
        string FileName { get; }
        string FileData { get; }
    }
}