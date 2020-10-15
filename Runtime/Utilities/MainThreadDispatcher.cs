using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


[HiraManager]
public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> execution_queue = new Queue<Action>();

    private static Thread _mainThread = null;

    private void Awake() => _mainThread = Thread.CurrentThread;

    private void Update()
    {
        lock (execution_queue)
        {
            while (execution_queue.Count > 0)
                execution_queue.Dequeue().Invoke();
        }
    }

    public static void ScheduleSync(Action action)
    {
        if (_mainThread != Thread.CurrentThread) ScheduleSync_NoThreadCheck(action);
        else action.Invoke();
    }

    public static void ScheduleSync_NoThreadCheck(Action action)
    {
        var hasRun = false;
        Schedule(() =>
        {
            action.Invoke();
            hasRun = true;
        });

        while (!hasRun) Thread.Sleep(10);
    }

    public static void Schedule(Action action)
    {
        lock (execution_queue) execution_queue.Enqueue(action);
    }
}