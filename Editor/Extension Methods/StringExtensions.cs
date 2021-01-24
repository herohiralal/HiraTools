using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
	public static class StringExtensions
	{
		private static readonly Dictionary<string, GUIContent> gui_content_cache = new Dictionary<string, GUIContent>();
		internal static GUIContent GetGUIContent(this string textAndTooltip)
		{
			if (string.IsNullOrEmpty(textAndTooltip))
				return GUIContent.none;

			if (gui_content_cache.TryGetValue(textAndTooltip, out var content)) return content;

			var s = textAndTooltip.Split('|');
			content = new GUIContent(s[0]);

			if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
				content.tooltip = s[1];

			gui_content_cache.Add(textAndTooltip, content);

			return content;
		}
	}
}