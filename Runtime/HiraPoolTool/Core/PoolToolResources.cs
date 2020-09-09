using System;
using System.Collections;
using System.Collections.Generic;
using HiraPoolTool.Abstractions;

namespace UnityEngine
{
    [AddComponentMenu("HiraTools/Pooling/PoolTool (Resources)")]
    public class PoolToolResources : PoolToolAbstract
    {
        [SerializeField] private StringReference address = null;
        
        private readonly List<Component> loanedObjects = new List<Component>();

        public override T GetPooledInstance<T>()
        {
            var pooledInstance = base.GetPooledInstance<T>();
            loanedObjects.Add(pooledInstance);
            return pooledInstance;
        }

        public override void ReturnInstanceToPool(Component component)
        {
            loanedObjects.Remove(component);
            base.ReturnInstanceToPool(component);
        }

        public override void LoadResource(Func<GameObject, Component> getTarget = null, byte? initialPopulateOverride = null)
        {
            getTarget ??= go => go.transform;
            StartCoroutine(LoadResourceEnumerator(getTarget, initialPopulateOverride));
        }

        private IEnumerator LoadResourceEnumerator(Func<GameObject, Component> getTarget, byte? initialPopulateOverride)
        {
            if (IsResourceLoaded) yield break;

            var resourceRequest = Resources.LoadAsync<GameObject>(address);
            yield return resourceRequest;
            Pool = GetPool(getTarget((GameObject) resourceRequest.asset));
            
            if (initialPopulateOverride.HasValue) Pool.AddToPool(initialPopulateOverride.Value);
            else if (initialPopulate > 0) Pool.AddToPool(initialPopulate);
        }

        public override void UnloadResource()
        {
            if (!IsResourceLoaded) return;

            foreach (var loanedObject in loanedObjects) Destroy(loanedObject.gameObject);
            loanedObjects.Clear();

            Pool.Dispose();
            Pool = null;
        }
    }
}