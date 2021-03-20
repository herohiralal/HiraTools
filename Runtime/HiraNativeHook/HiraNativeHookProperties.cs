namespace UnityEngine.Internal
{
    internal struct HiraNativeHookInitParams
    {
        public int NativeObjectRegistryInitReserveSize;
    }

    internal class HiraNativeHookProperties : ScriptableObject
    {
        [SerializeField] private int nativeObjectRegistryInitReserveSize = 10;


        public static implicit operator HiraNativeHookInitParams(HiraNativeHookProperties properties)
        {
            return new HiraNativeHookInitParams
            {
                NativeObjectRegistryInitReserveSize = properties.nativeObjectRegistryInitReserveSize
            };
        }
    }
}