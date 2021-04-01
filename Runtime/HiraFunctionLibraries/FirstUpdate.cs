using System;
using System.Collections.Generic;

namespace UnityEngine
{
    [HiraManager]
    [AddComponentMenu("")]
    [DefaultExecutionOrder(-10000)]
    public class FirstUpdate : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        private static void ResetExecutionQueue() => execution_queue.Clear();

        private static readonly Queue<Action> execution_queue = new Queue<Action>();

        private void Update()
        {
            while(execution_queue.Count>0)
                execution_queue.Dequeue().Invoke();
        }

        public static void Catch(Action action) => execution_queue.Enqueue(action);
    }
}