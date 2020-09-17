using UnityEngine;

// ReSharper disable UnusedMember.Global
public static class BlackboardAccessHelper
{
    public static bool GetBooleanValue(this IBlackboardValueAccessor blackboard, string keyName) =>
        blackboard.GetBooleanValue(blackboard.GetHash(keyName));

    public static float GetFloatValue(this IBlackboardValueAccessor blackboard, string keyName) =>
        blackboard.GetFloatValue(blackboard.GetHash(keyName));

    public static int GetIntValue(this IBlackboardValueAccessor blackboard, string keyName) =>
        blackboard.GetIntValue(blackboard.GetHash(keyName));

    public static string GetStringValue(this IBlackboardValueAccessor blackboard, string keyName) =>
        blackboard.GetStringValue(blackboard.GetHash(keyName));

    public static Vector3 GetVectorValue(this IBlackboardValueAccessor blackboard, string keyName) =>
        blackboard.GetVectorValue(blackboard.GetHash(keyName));

    public static void SetBooleanValue(this IBlackboardValueAccessor blackboard, string keyName, bool value) =>
        blackboard.SetBooleanValue(blackboard.GetHash(keyName), value);

    public static void SetFloatValue(this IBlackboardValueAccessor blackboard, string keyName, float value) =>
        blackboard.SetFloatValue(blackboard.GetHash(keyName), value);

    public static void SetIntValue(this IBlackboardValueAccessor blackboard, string keyName, int value) =>
        blackboard.SetIntValue(blackboard.GetHash(keyName), value);

    public static void SetStringValue(this IBlackboardValueAccessor blackboard, string keyName, string value) =>
        blackboard.SetStringValue(blackboard.GetHash(keyName), value);

    public static void SetVectorValue(this IBlackboardValueAccessor blackboard, string keyName, Vector3 value) =>
        blackboard.SetVectorValue(blackboard.GetHash(keyName), value);
}