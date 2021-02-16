namespace UnityEngine
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
	public interface IDirtiable
	{
		bool IsDirty { get; set; }
	}
#endif
}