/*
 * Name: HiraBlackboardKeyType.cs
 * Created By: Rohan Jadav
 * Description: The exhaustive list of all possible types of values for a blackboard key.
 */

namespace Hiralal.Blackboard
{
    public enum HiraBlackboardKeyType : byte
    {
        Undefined,
        Bool,
        Float,
        Int,
        String,
        Vector,
        UnityObject
    }
}