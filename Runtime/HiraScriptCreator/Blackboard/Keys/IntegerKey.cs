namespace UnityEngine
{
    public class IntegerKey : NumericalKey
    {
        [SerializeField] private int defaultValue = 0;
        protected override string KeyType => "int";
        protected override string DefaultValue => defaultValue.ToString();
    }
}