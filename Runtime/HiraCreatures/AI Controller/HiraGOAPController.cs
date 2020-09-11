﻿using System.Collections.Generic;
using System.Threading;
using Hiralal.GOAP.Planner;
using Hiralal.GOAP.Transitions;

namespace UnityEngine
{
    [RequireComponent(typeof(HiraBlackboard))]
    public class HiraGOAPController : HiraController
    {
        [Space] [Header("Required Components")]
        [SerializeField] private HiraBlackboard blackboard = null;
        
        [Space] [Header("Behaviour Customization")]
        [SerializeField] private HiraWorldStateTransition[] goals = null;
        private List<HiraCreatureAction> actions = null;

        [Space] [Header("Planner Settings")] 
        [SerializeField] private bool multiThreaded = false;
        [SerializeField] private float maxFScore = 100;
        
        private bool _planRequested = false;
        private HiraWorldStateTransition currentGoal = null;
        private Stack<HiraCreatureAction> _plan = null;
        private Planner<HiraCreatureAction> _planner = null;

        private void Awake()
        {
            _planner = new Planner<HiraCreatureAction>(blackboard, SetPlan);
        }

        private bool RequestPlan()
        {
            if (_planRequested) return false;

            _planRequested = true;

            _planner.Initialize(maxFScore, currentGoal, actions);
            
            if (multiThreaded) ThreadPool.QueueUserWorkItem(_planner.GeneratePlan);
            else _planner.GeneratePlan();

            return true;
        }

        private void SetPlan(Stack<HiraCreatureAction> newPlan)
        {
            _planRequested = false;
            _plan = newPlan;
        }
    }
}