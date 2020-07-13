using System.Collections.Generic;

namespace UnityEngine
{
    public static class GameObjectExtensions
    {
        public static IEnumerable<GameObject> GetAllChildren(this GameObject gameObject)
        {
            var childrenTransforms = gameObject.transform.GetAllChildren();
            foreach(var t in childrenTransforms)
            {
                yield return t.gameObject;
            }
        }
    }
}