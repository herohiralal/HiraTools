using System;
using HiraCreatures.Components.Blackboard.Internal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hiralal.Components.Blackboard.Tests
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

        private static SerializableKey[] Keys =>
            new[]
            {
                ScriptableObject.CreateInstance<SerializableKey>().Setup(BooleanKey, BlackboardKeyType.Bool, false),
                ScriptableObject.CreateInstance<SerializableKey>().Setup(FloatKey, BlackboardKeyType.Float, false),
                ScriptableObject.CreateInstance<SerializableKey>().Setup(IntKey, BlackboardKeyType.Int, false),
                ScriptableObject.CreateInstance<SerializableKey>().Setup(StringKey, BlackboardKeyType.String, false),
                ScriptableObject.CreateInstance<SerializableKey>().Setup(VectorKey, BlackboardKeyType.Vector, false),
                ScriptableObject.CreateInstance<SerializableKey>().Setup(FloatKeyInstanceSynced, BlackboardKeyType.Float, true),
                ScriptableObject.CreateInstance<SerializableKey>().Setup(VectorKeyInstanceSynced, BlackboardKeyType.Vector, true)
            };

        internal static KeysResource Resource => new KeysResource(Keys);
    }

    internal class KeysResource : IDisposable
    {
        public KeysResource(SerializableKey[] keys) => Keys = keys;

        public SerializableKey[] Keys { get; }
            
        public void Dispose()
        {
            foreach (var key in Keys) Object.DestroyImmediate(key);
        }
    }
}