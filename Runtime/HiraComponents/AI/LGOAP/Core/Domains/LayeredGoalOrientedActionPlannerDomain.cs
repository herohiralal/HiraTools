using UnityEngine;

namespace HiraEngine.Components.AI.LGOAP
{
    [CreateAssetMenu(menuName = "Hiralal/AI/LGOAP Domain", fileName = "New LGOAPDomain")]
    [HiraCollectionCustomizer(1, MaxObjectCount = byte.MaxValue, Title = "Goals")]
    [HiraCollectionCustomizer(2, MaxObjectCount = byte.MaxValue, Title = "Intermediate Goals")]
    [HiraCollectionCustomizer(3, MaxObjectCount = byte.MaxValue, Title = "Actions")]
    public class LayeredGoalOrientedActionPlannerDomain : HiraCollection<Goal, IntermediateGoal, Action>, IInitializable
    {
        public void Initialize()
        {
            
        }

        public void Shutdown()
        {
            
        }
    }
}