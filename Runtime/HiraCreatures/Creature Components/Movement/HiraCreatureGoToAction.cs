using System.Collections.Generic;
using Hiralal.Blackboard;
using UnityEngine;

// ReSharper disable SuspiciousTypeConversion.Global

namespace HiraCreatures.Components.Movement
{
    public class HiraCreatureGoToAction : HiraCreatureAction
    {
        public HiraCreatureGoToAction(Vector3 targetPosition,
            IEnumerable<HiraBlackboardValue> preconditions,
            IReadOnlyList<HiraBlackboardValue> effects,
            float proximityThreshold)
            : base(preconditions, effects)
        {
            _targetPosition = targetPosition;
            _proximityThreshold = proximityThreshold;
        }

        public override bool IsApplicableTo(HiraCreature creature) =>
            creature is IComponentOwner<IHiraCreatureMover>;

        private IHiraCreatureMover _creatureMover = null;
        private readonly Vector3 _targetPosition;
        private float _cost = float.MaxValue;

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

        private readonly float _proximityThreshold;

        public override void OnActionStart()
        {
            _creatureMover.MovementMode = HiraCreatureMovementMode.Positional;
            _creatureMover.MoveTo(_targetPosition);
        }

        public override void OnActionExecute()
        {
            if (Vector3.Distance(_creatureMover.Position, _targetPosition) < _proximityThreshold)
                MarkCompleted();
        }
    }
}