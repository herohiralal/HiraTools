/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: QuaternionSerializationSurrogate.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Defines serialization surrogate for Quaternion.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEngine;

namespace HiraSaves.Serialization.Surrogates
{
    [UsedImplicitly]
    internal class QuaternionSerializationSurrogate : _SerializationSurrogateAbstract<Quaternion>
    {
        private const string W_VARIABLE_SERIALIZATION_STRING = "w";
        private const string X_VARIABLE_SERIALIZATION_STRING = "x";
        private const string Y_VARIABLE_SERIALIZATION_STRING = "y";
        private const string Z_VARIABLE_SERIALIZATION_STRING = "z";

        protected override void SaveObject(Quaternion casted, SerializationInfo info)
        {
            info.AddValue(W_VARIABLE_SERIALIZATION_STRING, casted.w);
            info.AddValue(X_VARIABLE_SERIALIZATION_STRING, casted.x);
            info.AddValue(Y_VARIABLE_SERIALIZATION_STRING, casted.y);
            info.AddValue(Z_VARIABLE_SERIALIZATION_STRING, casted.z);
        }

        protected override Quaternion LoadObject(Quaternion casted, SerializationInfo info)
        {
            casted.w = (float) info.GetValue(W_VARIABLE_SERIALIZATION_STRING, typeof(float));
            casted.x = (float) info.GetValue(X_VARIABLE_SERIALIZATION_STRING, typeof(float));
            casted.y = (float) info.GetValue(Y_VARIABLE_SERIALIZATION_STRING, typeof(float));
            casted.z = (float) info.GetValue(Z_VARIABLE_SERIALIZATION_STRING, typeof(float));

            return casted;
        }
    }
}