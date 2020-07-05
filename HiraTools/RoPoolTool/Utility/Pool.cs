﻿using System.Collections.Generic;
 using HiraPoolTool.Abstractions;
 using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraPoolTool.Core
{
    internal readonly struct Pool : IPool
    {
        public Pool(Component bucket, Component target)
        {
            (this.bucket, this.target) = (bucket.transform, target);
            pooledObjects = new List<Component>();
            counter = new Counter();
        }

        // destroy all pooled objects and clear the references
        public void Dispose()
        {
            foreach (var pooledObject in pooledObjects) 
                Object.Destroy(pooledObject.gameObject);

            counter.Reset();
            pooledObjects.Clear();
        }

        private readonly Counter counter;
        private readonly Component target;
        private readonly List<Component> pooledObjects;
        private readonly Transform bucket;

        public int PoolCount => counter.Value;

        #region Pool Access
        
        // get an object from the pool
        public Component GetObject()
        {
            counter.Decrement();
            var pooledObject = pooledObjects[PoolCount];
            pooledObjects.RemoveAt(PoolCount);
            return pooledObject;
        }

        // return an object to the pool
        public void ReturnObject(Component component)
        {
            counter.Increment();
            component.transform.SetParent(bucket);
            component.gameObject.SetActive(false);
            pooledObjects.Add(component);
        }
        
        #endregion
        
        #region Pool modification

        // add a select number of instances to the pool
        public void AddToPool(byte count) { for (; count > 0; count--) AddToPool(); }

        // remove a select number of instances from the pool
        public void RemoveFromPool(byte count) { for (; count > 0; count--) RemoveFromPool(); }

        // add one instance to the pool
        public void AddToPool() => ReturnObject(Object.Instantiate(target));
        // remove one instance from the pool
        public void RemoveFromPool() => Object.Destroy(GetObject().gameObject);
        
        #endregion

        private class Counter
        {
            public int Value { get; private set; } = 0;
            public void Increment() => Value++;
            public void Decrement() => Value--;
            public void Reset() => Value = 0;
        }
    }
}