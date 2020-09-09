/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: FormatterUtility.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Provides a binary formatter with proper surrogate
 * *               selectors to SerializationUtility.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using HiraSaves.Serialization.Surrogates;
using HiraSaves.Utility;

namespace HiraSaves.Serialization
{
    internal static class FormatterUtility
    {
        static FormatterUtility()
        {
            Formatter = new BinaryFormatter {SurrogateSelector = Selector};
        }

        internal static BinaryFormatter Formatter { get; }

        private static SurrogateSelector Selector
        {
            get
            {
                var selector = new SurrogateSelector();

                ReflectionBoilerplate.PerformOnTypes(
                    t => t.IsSubclassOf(typeof(_SerializationSurrogateAbstract)) && !t.IsAbstract,
                    t => selector.AddTypeToSelector(t));

                return selector;
            }
        }

        private static void AddTypeToSelector(this SurrogateSelector selector, Type toAdd)
        {
            // create an instance of all the child classes
            var surrogate = (_SerializationSurrogateAbstract) Activator.CreateInstance(toAdd);
            
            // add them as surrogates to the surrogate selector
            selector.AddSurrogate(surrogate.IsSurrogateFor,
                new StreamingContext(StreamingContextStates.All), surrogate);
        }
    }
}