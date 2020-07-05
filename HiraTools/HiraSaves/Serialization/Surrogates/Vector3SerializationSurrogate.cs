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

namespace HiraSaves.Serialization.Surrogates
{
    [UsedImplicitly]
    internal class Vector3SerializationSurrogate : _SerializationSurrogateAbstract<Vector3>
    {
        private const string X_VARIABLE_SERIALIZATION_STRING = "x";
        private const string Y_VARIABLE_SERIALIZATION_STRING = "y";
        private const string Z_VARIABLE_SERIALIZATION_STRING = "z";
        
        protected override void SaveObject(Vector3 casted, SerializationInfo info)
        {
            info.AddValue(X_VARIABLE_SERIALIZATION_STRING, casted.x);
            info.AddValue(Y_VARIABLE_SERIALIZATION_STRING, casted.y);
            info.AddValue(Z_VARIABLE_SERIALIZATION_STRING, casted.z);
        }

        protected override Vector3 LoadObject(Vector3 casted, SerializationInfo info)
        {
            casted.x = (float) info.GetValue(X_VARIABLE_SERIALIZATION_STRING, typeof(float));
            casted.y = (float) info.GetValue(Y_VARIABLE_SERIALIZATION_STRING, typeof(float));
            casted.z = (float) info.GetValue(Z_VARIABLE_SERIALIZATION_STRING, typeof(float));

            return casted;
        }
    }
}