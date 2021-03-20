using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace HiraEngine.Components.Blackboard.Internal
{
    public class EnumKey : HiraBlackboardKey
    {
        [SerializeField] public string[] names = null;
        [SerializeField] public int defaultValue = 0;
#if UNITY_EDITOR
#pragma warning disable cs4014
        [SerializeField] public string defaultValueName = "";
#pragma warning restore cs4014
#endif
        [SerializeField, HideInInspector] private byte size = 1;

        private void OnValidate()
        {
            if (names.Length == 0)
                Array.Resize(ref names, 1);

            var length = names.Length;
            defaultValue = Mathf.Clamp(defaultValue, 0, length - 1);
#if UNITY_EDITOR
            defaultValueName = names[defaultValue];
#endif

            size = length <= 256
                ? (byte) 1
                : length <= 65536
                    ? (byte) 2
                    : (byte) 4;
        }

        public sealed override byte SizeInBytes => size;

        public override unsafe void SetDefault(void* value)
        {
            switch (size)
            {
                case 1:
                    *(byte*) value = (byte) defaultValue;
                    break;
                case 2:
                    *(ushort*) value = (ushort) defaultValue;
                    break;
                case 4:
                    *(int*) value = defaultValue;
                    break;
            }
        }

#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
        {
            int value;
            switch (size)
            {
                case 1:
                    value = *(byte*) data;
                    break;
                case 2:
                    value = *(ushort*) data;
                    break;
                case 4:
                    value = *(int*) data;
                    break;
                default:
                    return;
            }

            value = EditorGUILayout.Popup(name, value, names);

            switch (size)
            {
                case 1:
                    if (value != *(byte*) data) blackboard.SetValue(Index, (byte) value);
                    break;
                case 2:
                    if (value != *(ushort*) data) blackboard.SetValue(Index, (ushort) value);
                    break;
                case 4:
                    if (value != *(int*) data) blackboard.SetValue(Index, value);
                    break;
            }
        }
#endif
    }
}