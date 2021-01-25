namespace UnityEngine
{
    public interface IHiraScriptCreator
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        string CachedFilePath { get; set; }
        string FileName { get; }
        string FileData { get; }
        ScriptableObject[] Dependencies { get; }
#endif
    }
}