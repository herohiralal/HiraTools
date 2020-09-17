using HiraCreatures.Components.Blackboard.Internal;

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

        internal static SerializableKey[] Keys =>
            new[]
            {
                new SerializableKey(BooleanKey, BlackboardKeyType.Bool, false),
                new SerializableKey(FloatKey, BlackboardKeyType.Float, false),
                new SerializableKey(IntKey, BlackboardKeyType.Int, false),
                new SerializableKey(StringKey, BlackboardKeyType.String, false),
                new SerializableKey(VectorKey, BlackboardKeyType.Vector, false),
                new SerializableKey(FloatKeyInstanceSynced, BlackboardKeyType.Float, true),
                new SerializableKey(VectorKeyInstanceSynced, BlackboardKeyType.Vector, true)
            };
    }
}