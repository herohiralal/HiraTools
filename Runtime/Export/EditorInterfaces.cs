namespace UnityEngine
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
	public interface IDirtiable
	{
		bool IsDirty { get; set; }
	}
	
	public interface IHiraScriptCreator
	{
		string CachedFilePath { get; set; }
		string FileName { get; }
		string FileData { get; }
		ScriptableObject[] Dependencies { get; }
	}
#endif
}