namespace UnityEngine.Internal
{
    internal struct HiraNativeHookInitParams
    {
        public int NativeObjectRegistryInitReserveSize;
    }
    
    [CreateAssetMenu]
    internal class HiraNativeHookProperties : ScriptableObject
    {
        [SerializeField] private int nativeObjectRegistryInitReserveSize;
        
        
        public static implicit operator HiraNativeHookInitParams(HiraNativeHookProperties properties)
        {
            return new HiraNativeHookInitParams
            {
                NativeObjectRegistryInitReserveSize = properties.nativeObjectRegistryInitReserveSize
            };
        }
    }
}