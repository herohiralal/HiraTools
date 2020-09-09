/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: ReflectionBoilerplate.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Provides boilerplate code for reflection.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Linq;

namespace HiraSaves.Utility
{
    internal static class ReflectionBoilerplate
    {
        internal static void PerformOnTypes(Func<Type, bool> filter, Action<Type> action)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes().Where(filter))
                action(type);
        }
    }
}