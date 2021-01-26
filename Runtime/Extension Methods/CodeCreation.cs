public static class CodeCreation
{
	public static string ToCode(this float f) => $"{f}f";
	public static string ToCode<T>(this T e) where T : System.Enum => $"{typeof(T).FullName}.{e}";
	public static string ToCode(this int i) => i.ToString();
	public static string ToCode(this bool b) => b.ToString().ToLowerInvariant();
	public static string ToCode(this UnityEngine.Vector3 v) => $"new Vector3({v.x}f, {v.y}f, {v.z}f)";
	public static string ToCode(this string s) => $"\"{s}\"";
}