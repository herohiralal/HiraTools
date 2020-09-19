using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraTests.HiraEngine.Components.Blackboard
{
    public static class BlackboardTestsCommon
    {
        internal const string BooleanKey = "Boolean";
        internal const string FloatKey = "Float";
        internal const string IntKey = "Integer";
        internal const string StringKey = "String";
        internal const string VectorKey = "Vector";
        internal const string FloatKeyInstanceSynced = "InstanceSyncedFloat";
        internal const string VectorKeyInstanceSynced = "InstanceSyncedVector";

        private static SerializableBlackboardKey[] Keys =>
            new[]
            {
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(BooleanKey, BlackboardKeyType.Bool, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(FloatKey, BlackboardKeyType.Float, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(IntKey, BlackboardKeyType.Int, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(StringKey, BlackboardKeyType.String, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(VectorKey, BlackboardKeyType.Vector, false),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(FloatKeyInstanceSynced, BlackboardKeyType.Float, true),
                ScriptableObject.CreateInstance<SerializableBlackboardKey>().Setup(VectorKeyInstanceSynced, BlackboardKeyType.Vector, true)
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