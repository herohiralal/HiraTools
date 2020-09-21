using HiraEngine.Components.Planner;

namespace UnityEngine
{
    public static class PlannerTypes
    {
        public static IBlackboardQuery GetQuery(string typeString,
            IBlackboardValueConstructorParams constructorParams) =>
            BlackboardQueryFactory.GetValue(typeString, constructorParams);

        public static IBlackboardModification GetModification(string typeString,
            IBlackboardValueConstructorParams constructorParams) =>
            BlackboardModificationFactory.GetValue(typeString, constructorParams);

        public static IBlackboardHybridValue GetHybridValue(string typeString,
            IBlackboardValueConstructorParams constructorParams) =>
            BlackboardHybridValueFactory.GetValue(typeString, constructorParams);
    }
}