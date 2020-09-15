using System.Collections.Generic;
using Hiralal.Blackboard;
using UnityEngine;
// ReSharper disable SuspiciousTypeConversion.Global

namespace HiraCreatures.Components.Movement
{
    public class HiraCreatureGoToAction : HiraCreatureAction
    {
        public HiraCreatureGoToAction(Vector3 targetPosition, uint typeSpecificIndex, int positionID)
        {
            _targetPosition = targetPosition;
            _effects = new HiraBlackboardValue[] { new HiraBlackboardValue<int>(typeSpecificIndex, positionID)  };
        }
 
        public override bool IsApplicableTo(HiraCreature creature) =>
            creature is IComponentOwner<IHiraCreatureMover>;

        private IHiraCreatureMover _creatureMover = null;
        private readonly Vector3 _targetPosition;
        private float _cost = float.MaxValue;
        private readonly HiraBlackboardValue[] _effects = null;

        public override void BuildPrePlanCache()
        {
            _cost = Vector3.Distance(_creatureMover.Position, _targetPosition);
        }
        
        public override HiraCreature TargetCreature 
        {
            set
            {
                if (value is IComponentOwner<IHiraCreatureMover> movableCreature)
                    _creatureMover = movableCreature.Component;
            }
        }

        public override float Cost => _cost;

        public override IReadOnlyList<HiraBlackboardValue> Effects => _effects;
    }
}