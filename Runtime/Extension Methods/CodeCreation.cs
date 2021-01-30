using System.Linq;

public static class CodeCreation
{
	public static string ToCode(this float f) => $"{f}f";
	public static string ToCode<T>(this T e) where T : System.Enum
	{
		var typeString = typeof(T).FullName;
		var enumString = e.ToString();
		if (!enumString.Contains(", ")) return $"{typeString}.{enumString}";
		
		// flags
		var values = enumString.Split(new[]{", "}, System.StringSplitOptions.RemoveEmptyEntries);
		var sb = new System.Text.StringBuilder(250);
		var valuesLength = values.Length;
		for (var index = 0; index < valuesLength; index++)
		{
			sb.Append($"{typeString}.{values[index]}");
			if (index != valuesLength - 1)
				sb.Append(" | ");
		}

		return sb.ToString();

	}

	public static string ToCode(this int i) => i.ToString();
	public static string ToCode(this bool b) => b.ToString().ToLowerInvariant();
	public static string ToCode(this UnityEngine.Vector3 v) => $"new Vector3({v.x}f, {v.y}f, {v.z}f)";
	public static string ToCode(this string s) => $"\"{s}\"";

	public static string PascalToCamel(this string name, bool underscore = true)
	{
		var charArray = name.ToCharArray();
		charArray[0] = char.ToLowerInvariant(charArray[0]);
		var s = new string(charArray);
		return underscore ? $"_{s}" : s;
	}

	public static string PascalToAllUpper(this string input) =>
		string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{char.ToUpper(x)}" : $"{char.ToUpper(x)}"));
}