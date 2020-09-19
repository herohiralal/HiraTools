/*
 * Name: PoolToolAbstract.cs
 * Created By: Rohan Jadav
 * Description: Abstraction for a PoolTool.
 */

using System;
using HiraEngine.PoolTool;

namespace UnityEngine
{
    public abstract class PoolToolAbstract : MonoBehaviour
    {
        [SerializeField] protected byte initialPopulate = 0;
        [SerializeField] private PoolKey key = null;

        protected IPool Pool = null;
        public bool IsResourceLoaded => Pool != null;

        private void Awake()
        {
            if (key != null) key.SetOwner(this);
        }

        private void OnDestroy()
        {
            if (IsResourceLoaded) UnloadResource();
            if (key != null) key.ClearOwner();
        }

        #region Pool API

        public virtual T GetPooledInstance<T>() where T : Component
        {
            if (!IsResourceLoaded)
                throw new NullReferenceException("Please load the resources on this PoolTool" +
                                                 " before attempting to get an instance.");

            if (Pool.PoolCount == 0) Pool.AddToPool();
            var pooledInstance = Pool.GetObject();
            pooledInstance.transform.SetParent(null);
            pooledInstance.gameObject.SetActive(true);

            try { return (T) pooledInstance; }
            
            catch (InvalidCastException)
            {
                Debug.LogErrorFormat(this, $"Invalid type {typeof(T).Name} requested.");
                return null;
            }
        }
        
        public T GetPooledInstanceWithAutoReturn<T>(float timer, bool ignoreTimescale = false) where T : Component
        {
            var pooledInstance = GetPooledInstance<T>();
            HiraTimerEvents.RequestPing(() => ReturnInstanceToPool(pooledInstance),
                timer,
                true,
                ignoreTimescale);
            return pooledInstance;
        }

        public virtual void ReturnInstanceToPool(Component component)
        {
            if(!IsResourceLoaded) 
                Debug.LogWarning($"The GameObject {component.gameObject.name} could not be returned" +
                                 $" to the PoolTool on GameObject {gameObject.name}.", this);
            Pool.ReturnObject(component);
        }

        #endregion

        public abstract void LoadResource(Func<GameObject, Component> getTarget = null, byte? initialPopulateOverride = null);

        public abstract void UnloadResource();
        
        protected IPool GetPool(Component target) => new Pool(this, target);
    }
}