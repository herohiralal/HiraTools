using System;

namespace UnityEngine
{
    [Serializable]
    public class SerializedInstance
    {
        [SerializeField] private string classData = null;

        public T Get<T>(bool cachedInstance = true) => 
            string.IsNullOrEmpty(classData) 
                ? default 
                : HiraEngine.SerializedInstance.SerializedInstanceFactory.Get<T>(classData, cachedInstance);
    }
}