using HiraEngine.Components.Planner;
using HiraEngine.Components.Planner.Internal;

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
        
        public static IPlanner<T> GetPlanner<T>(IBlackboardValueAccessor valueAccessor, byte length) where T : IAction =>
            new Planner<T>(valueAccessor, length);
        
        public static IPlanStack<T> GetPlanStack<T>(byte length) where T : IAction =>
            new PlanStack<T>(length);
    }
}