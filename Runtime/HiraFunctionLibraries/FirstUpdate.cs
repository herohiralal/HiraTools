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

        public static void Catch(Action action) => execution_queue.Enqueue(action);
    }
}