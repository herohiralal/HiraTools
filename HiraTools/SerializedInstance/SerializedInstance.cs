using System;
using HiraBoilerplate.SerializedInstance;

namespace UnityEngine
{
    [Serializable]
    public class SerializedInstance
    {
        [SerializeField] private string classData = null;

        public T Get<T>(bool cachedInstance = true) => 
            string.IsNullOrEmpty(classData) 
                ? default 
                : SerializedInstanceFactory.Get<T>(classData, cachedInstance);
    }
}