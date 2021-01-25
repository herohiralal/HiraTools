﻿namespace UnityEngine
{
    public class IntegerKey : BlackboardKey
    {
        [SerializeField] private int defaultValue = 0;
        protected override string KeyType => "int";
        protected override string DefaultValue => defaultValue.ToString();
    }
}