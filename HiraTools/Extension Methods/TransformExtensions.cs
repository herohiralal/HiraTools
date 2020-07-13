using System.Collections.Generic;

namespace UnityEngine
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> GetAllChildren(this Transform transform)
        {
            var stack = new Stack<Transform>();
            stack.Push(transform);
            while(stack.Count > 0)
            {
                var t = stack.Pop();
                yield return t;

                for (var i = 0; i < t.childCount; i++)
                {
                    stack.Push(t.GetChild(i));
                }
            }
        }
    }
}