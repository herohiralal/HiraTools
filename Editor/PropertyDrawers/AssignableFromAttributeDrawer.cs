using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HiraEditor
{
    [CustomPropertyDrawer(typeof(AssignableFromAttribute))]
    public class AssignableFromAttributeDrawer : PropertyDrawer
    {
        private const string class_data_variable_name = "classData";
        private const string no_instances = "No subclasses found.";

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.type.Contains(nameof(SerializedInstance)))
            {
                EditorGUI.LabelField(position, "Only use AssignableFrom attribute with SerializedInstance.");
                return;
            }

            var types = Attribute.Type.GetSubclasses().ToArray();
            var names = types.Select(t=>t.Name).ToArray();
            //var (types, names) = GetTypesAndNames();
            if (names.Length == 0) names = new[] {no_instances};

            var data = property.FindPropertyRelative(class_data_variable_name);
            var mainRect = EditorGUI.PrefixLabel(position, label);

            var index = 0;

            if (names[0] != no_instances)
            {
                var type = Type.GetType(data.stringValue);
                if (type != null) index = Array.IndexOf(types, type);
            }

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            index = EditorGUI.Popup(mainRect, "", index, names);

            data.stringValue = names[0] == no_instances
                ? no_instances
                : types[index].FullName + ", " + types[index].Assembly.FullName;

            EditorGUI.indentLevel = indent;
        }
        
        private AssignableFromAttribute Attribute => (AssignableFromAttribute) attribute;
    }
}