﻿using UnityEngine;

namespace HiraEngine.Components.Planner.Internal
{
    public class PlanStack<T> : IPlanStack<T> where T : IAction
    {
        public PlanStack(int length) => _actions = new T[length];
        
        private readonly T[] _actions = null;
        private int _planSize = 0;
        private int _currentIndex = 0;

        public void Consume(T[] actions, int planSize)
        {
            _planSize = planSize;

            var startIndex = planSize - 1;
            _currentIndex = startIndex;
            
            for (var i = 0; i < planSize; i++) _actions[startIndex - i] = actions[i];
        }

        public T Pop() => _actions[_currentIndex--];

        public bool HasActions => _currentIndex > -1;

        public override string ToString()
        {
            var data = "";
            for (var i = 0; i < _planSize; i++)
            {
                data += _actions[i].Name;
                data += _currentIndex == i ? " <--\n" : "    \n";
            }
            return data;
        }
    }
}