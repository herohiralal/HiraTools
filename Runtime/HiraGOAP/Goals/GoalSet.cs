using System.Collections;
using System.Collections.Generic;
using Hiralal.GOAP.Transitions;
using UnityEngine;

namespace HiraGOAP.Goals
{
    [CreateAssetMenu(fileName = "New Goal Set", menuName = "Hiralal/HiraEngine/HiraGOAP/Goal Set")]
    public class GoalSet : ScriptableObject, IEnumerable<HiraWorldStateTransition>
    {
        [SerializeField] private HiraWorldStateTransition[] goals = null;

        public IEnumerator<HiraWorldStateTransition> GetEnumerator() =>
            ((IEnumerable<HiraWorldStateTransition>) goals).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}