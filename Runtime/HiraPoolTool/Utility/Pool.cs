using System.Collections.Generic;
 using UnityEngine;
using Object = UnityEngine.Object;

namespace HiraEngine.PoolTool
{
    internal readonly struct Pool : IPool
    {
        public Pool(Component bucket, Component target)
        {
            (this._bucket, this._target) = (bucket.transform, target);
            _pooledObjects = new List<Component>();
            _counter = new Counter();
        }

        // destroy all pooled objects and clear the references
        public void Dispose()
        {
            foreach (var pooledObject in _pooledObjects) 
                Object.Destroy(pooledObject.gameObject);

            _counter.Reset();
            _pooledObjects.Clear();
        }

        private readonly Counter _counter;
        private readonly Component _target;
        private readonly List<Component> _pooledObjects;
        private readonly Transform _bucket;

        public int PoolCount => _counter.Value;

        #region Pool Access
        
        // get an object from the pool
        public Component GetObject()
        {
            _counter.Decrement();
            var pooledObject = _pooledObjects[PoolCount];
            _pooledObjects.RemoveAt(PoolCount);
            return pooledObject;
        }

        // return an object to the pool
        public void ReturnObject(Component component)
        {
            _counter.Increment();
            component.transform.SetParent(_bucket);
            component.gameObject.SetActive(false);
            _pooledObjects.Add(component);
        }
        
        #endregion
        
        #region Pool modification

        // add a select number of instances to the pool
        public void AddToPool(byte count) { for (; count > 0; count--) AddToPool(); }

        // remove a select number of instances from the pool
        public void RemoveFromPool(byte count) { for (; count > 0; count--) RemoveFromPool(); }

        // add one instance to the pool
        public void AddToPool() => ReturnObject(Object.Instantiate(_target));
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