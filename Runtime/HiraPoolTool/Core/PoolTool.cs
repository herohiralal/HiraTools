﻿using System;
using System.Collections.Generic;
using HiraPoolTool.Abstractions;

 namespace UnityEngine
{
    [AddComponentMenu("HiraTools/Pooling/PoolTool")]
    public class PoolTool : PoolToolAbstract
    {
        [SerializeField] private GameObject targetPrefab = null;
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
            if (IsResourceLoaded) return;

            if (getTarget == null) getTarget = go => go.transform;
            
            Pool = GetPool(getTarget(targetPrefab));

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