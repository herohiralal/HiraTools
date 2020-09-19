/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: Vector3SerializationSurrogate.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Defines serialization surrogate for Vector3.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEngine;

namespace HiraEngine.HiraSaves.Serialization.Surrogates
{
    [UsedImplicitly]
    internal class Vector3SerializationSurrogate : _SerializationSurrogateAbstract<Vector3>
    {
        private const string x_variable_serialization_string = "x";
        private const string y_variable_serialization_string = "y";
        private const string z_variable_serialization_string = "z";
        
        protected override void SaveObject(Vector3 casted, SerializationInfo info)
        {
            info.AddValue(x_variable_serialization_string, casted.x);
            info.AddValue(y_variable_serialization_string, casted.y);
            info.AddValue(z_variable_serialization_string, casted.z);
        }

        protected override Vector3 LoadObject(Vector3 casted, SerializationInfo info)
        {
            casted.x = (float) info.GetValue(x_variable_serialization_string, typeof(float));
            casted.y = (float) info.GetValue(y_variable_serialization_string, typeof(float));
            casted.z = (float) info.GetValue(z_variable_serialization_string, typeof(float));

            return casted;
        }
    }
}