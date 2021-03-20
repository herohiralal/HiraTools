using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HiraEngine.Components.Blackboard.Internal
{
    public class EnumFlagsKey : HiraBlackboardKey
    {
        [Serializable]
        public struct Enumerator
        {
            public string name;
            public bool onAtStart;
        }

        [SerializeField] public Enumerator[] names = null;
        [SerializeField] private byte size = 1;
        [SerializeField] private sbyte default1ByteValue = 0;
        [SerializeField] private short default2ByteValue = 0;
        [SerializeField] private int default4ByteValue = 0;
        [SerializeField] private long default8ByteValue = 0;

        private void OnValidate()
        {
            if (names.Length == 0)
                Array.Resize(ref names, 1);

            var length = names.Length;

            size =
                length <= 8
                    ? (byte) 1
                    : length <= 16
                        ? (byte) 2
                        : length <= 32
                            ? (byte) 4
                            : length <= 64
                                ? (byte) 8
                                : throw new ArgumentOutOfRangeException(nameof(names), "Only upto 64 flags are supported per key.");

            for (var i = 0; i < length; i++)
            {
                if (names[i].onAtStart)
                {
                    switch (size)
                    {
                        case 1:
                            default1ByteValue |= (sbyte) (1 << i);
                            break;
                        case 2:
                            default2ByteValue |= (short) (1 << i);
                            break;
                        case 4:
                            default4ByteValue |= (1 << i);
                            break;
                        case 8:
                            default8ByteValue |= (1L << i);
                            break;
                    }
                }
                else
                {
                    switch (size)
                    {
                        case 1:
                            default1ByteValue &= (sbyte) ~((sbyte) (1 << i));
                            break;
                        case 2:
                            default2ByteValue &= (short) ~((short) (1 << i));
                            break;
                        case 4:
                            default4ByteValue &= ~(1 << i);
                            break;
                        case 8:
                            default8ByteValue &= ~(1L << i);
                            break;
                    }
                }
            }
        }

        public override byte SizeInBytes => size;

        public override unsafe void SetDefault(void* value)
        {
            switch (size)
            {
                case 1:
                    *(sbyte*) value = default1ByteValue;
                    break;
                case 2:
                    *(short*) value = default2ByteValue;
                    break;
                case 4:
                    *(int*) value = default4ByteValue;
                    break;
                case 8:
                    *(long*) value = default8ByteValue;
                    break;
            }
        }

#if UNITY_EDITOR
        public override unsafe void DrawEditor(void* data, IBlackboardComponent blackboard)
        {
            // long value;
            switch (size)
            {
                case 1: DrawEditor1Byte(data, blackboard);
                    break;
                case 2: DrawEditor2Byte(data, blackboard);
                    break;
                case 4: DrawEditor4Byte(data, blackboard);
                    break;
                case 8: DrawEditor8Byte(data, blackboard);
                    break;
                default:
                    return;
            }
        }

        public unsafe void DrawEditor1Byte(void* data, IBlackboardComponent blackboard)
        {
            var value = *(sbyte*) data;
            var length = names.Length;
            
            for (var i = 0; i < length; i++)
            {
                if (EditorGUILayout.Toggle(names[i].name, ((value >> i) & 1) == 1))
                    value |= (sbyte) (1 << i);
                else
                    value &= (sbyte) ~((sbyte) (1 << i));
            }

            if (value != *(sbyte*) data)
                blackboard.SetValue(Index, value);
        }

        public unsafe void DrawEditor2Byte(void* data, IBlackboardComponent blackboard)
        {
            var value = *(short*) data;
            var length = names.Length;
            
            for (var i = 0; i < length; i++)
            {
                if (EditorGUILayout.Toggle(names[i].name, ((value >> i) & 1) == 1))
                    value |= (short) (1 << i);
                else
                    value &= (short) ~((short) (1 << i));
            }

            if (value != *(short*) data)
                blackboard.SetValue(Index, value);
        }

        public unsafe void DrawEditor4Byte(void* data, IBlackboardComponent blackboard)
        {
            var value = *(int*) data;
            var length = names.Length;
            
            for (var i = 0; i < length; i++)
            {
                if (EditorGUILayout.Toggle(names[i].name, ((value >> i) & 1) == 1))
                    value |= (1 << i);
                else
                    value &= ~(1 << i);
            }

            if (value != *(int*) data)
                blackboard.SetValue(Index, value);
        }

        public unsafe void DrawEditor8Byte(void* data, IBlackboardComponent blackboard)
        {
            var value = *(long*) data;
            var length = names.Length;
            
            for (var i = 0; i < length; i++)
            {
                if (EditorGUILayout.Toggle(names[i].name, ((value >> i) & 1) == 1))
                    value |= (1L << i);
                else
                    value &= ~(1L << i);
            }

            if (value != *(long*) data)
                blackboard.SetValue(Index, value);
        }
#endif
    }
}