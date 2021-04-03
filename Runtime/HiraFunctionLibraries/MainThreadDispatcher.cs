using System;
using System.Collections.Generic;
using System.Threading;

namespace UnityEngine
{
    [HiraManager]
    [AddComponentMenu("")]
    internal class MainThreadDispatcher : MonoBehaviour
    {
        private class ScheduleSyncHelper
        {
            public Action Action;
            public bool IsDone;

            public void Invoke()
            {
                Action.Invoke();
                IsDone = true;
            }
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void ResetExecutionQueue()
        {
            lock (execution_queue)
                execution_queue.Clear();
        }

        private static readonly Queue<Action> execution_queue = new Queue<Action>();

        private static Thread _mainThread = null;

        private void Awake() => _mainThread = Thread.CurrentThread;

        private void Update()
        {
            lock (execution_queue)
            {
                for (var i = execution_queue.Count; i > 0; i--)
                {
                    var current = execution_queue.Dequeue();
                    try
                    {
                        current.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e, this);
                    }
                }
            }
        }

        public static void ScheduleSync(Action action)
        {
            if (_mainThread != Thread.CurrentThread) ScheduleSync_NoThreadCheck(action);
            else action.Invoke();
        }

        public static void ScheduleSync_NoThreadCheck(Action action)
        {
            var helper = GenericPool<ScheduleSyncHelper>.Retrieve();
            helper.Action = action;
            helper.IsDone = false;
            Schedule(helper.Invoke);

            while (!helper.IsDone) Thread.Sleep(10);
            GenericPool<ScheduleSyncHelper>.Return(helper);
        }

        public static void Schedule(Action action)
        {
            lock (execution_queue) execution_queue.Enqueue(action);
        }
    }
}