using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class HiraConsoleAttribute : Attribute
    {
        public HiraConsoleAttribute(string name = "") => Name = name;

        public readonly string Name;
    }
}