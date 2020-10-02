using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraTests.HiraEngine.Components.Blackboard
{
    public static class BlackboardTestsCommon
    {
        internal const string BOOLEAN_KEY = "Boolean";
        internal const string FLOAT_KEY = "Float";
        internal const string INT_KEY = "Integer";
        internal const string STRING_KEY = "String";
        internal const string VECTOR_KEY = "Vector";
        internal const string FLOAT_KEY_INSTANCE_SYNCED = "InstanceSyncedFloat";
        internal const string VECTOR_KEY_INSTANCE_SYNCED = "InstanceSyncedVector";

        private static SerializableBlackboardKey[] Keys =>
            new[]
            {
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(BOOLEAN_KEY, BlackboardKeyType.Bool, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(FLOAT_KEY, BlackboardKeyType.Float, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(INT_KEY, BlackboardKeyType.Int, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(STRING_KEY, BlackboardKeyType.String, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(VECTOR_KEY, BlackboardKeyType.Vector, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(FLOAT_KEY_INSTANCE_SYNCED, BlackboardKeyType.Float, true),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(VECTOR_KEY_INSTANCE_SYNCED, BlackboardKeyType.Vector, true)
            };

        internal static KeysResource Resource => new KeysResource(Keys);
    }

    internal class KeysResource : IDisposable
    {
        public KeysResource(SerializableBlackboardKey[] keys) => Keys = keys;

        public SerializableBlackboardKey[] Keys { get; }
            
        public void Dispose()
        {
            foreach (var key in Keys) Object.DestroyImmediate(key);
        }
    }
}