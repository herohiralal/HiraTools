using System.Collections.Generic;
using System.Linq;
using Hiralal.Blackboard;
using UnityEngine;

namespace Hiralal.GOAP
{
    [CreateAssetMenu(fileName = "New HiraGOAP Transition", menuName = "Hiralal/HiraEngine/HiraGOAP/Transition")]
    public class HiraWorldStateTransition : ScriptableObject
    {
        [SerializeField] private HiraBlackboardKeySet keySet = null;
        [SerializeField] [Range(0, 100)] private float baseCost = 0f;

        [SerializeField] private HiraSerializableBlackboardValue[] preconditions = null;
        [SerializeField] private HiraSerializableBlackboardValue[] effects = null;

        private HiraBlackboardValue[] generatedPreconditions = null;
        private HiraBlackboardValue[] generatedEffects = null;

        public bool ArePreConditionsSatisfied(HiraBlackboardValueSet valueSet) =>
            generatedPreconditions.All(valueSet.ContainsValue);

        public void Activate()
        {
            if (keySet == null) return;
            
            var generatedPreconditionsEnumerable =
                preconditions.Select(serializableValue => serializableValue.GetBlackboardValue(keySet));
            generatedPreconditions = generatedPreconditionsEnumerable as HiraBlackboardValue[] ??
                                     generatedPreconditionsEnumerable.ToArray();

            var generatedEffectsEnumerable =
                effects.Select(serializableValue => serializableValue.GetBlackboardValue(keySet));
            generatedEffects = generatedEffectsEnumerable as HiraBlackboardValue[] ??
                               generatedEffectsEnumerable.ToArray();
        }

        public void Deactivate()
        {
            generatedPreconditions = null;
            generatedEffects = null;
        }

        public float BaseCost => baseCost;
        public IReadOnlyList<HiraBlackboardValue> Effects => generatedEffects;
    }
}