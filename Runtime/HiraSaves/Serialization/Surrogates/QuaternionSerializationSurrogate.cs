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
        private const string w_variable_serialization_string = "w";
        private const string x_variable_serialization_string = "x";
        private const string y_variable_serialization_string = "y";
        private const string z_variable_serialization_string = "z";

        protected override void SaveObject(Quaternion casted, SerializationInfo info)
        {
            info.AddValue(w_variable_serialization_string, casted.w);
            info.AddValue(x_variable_serialization_string, casted.x);
            info.AddValue(y_variable_serialization_string, casted.y);
            info.AddValue(z_variable_serialization_string, casted.z);
        }

        protected override Quaternion LoadObject(Quaternion casted, SerializationInfo info)
        {
            casted.w = (float) info.GetValue(w_variable_serialization_string, typeof(float));
            casted.x = (float) info.GetValue(x_variable_serialization_string, typeof(float));
            casted.y = (float) info.GetValue(y_variable_serialization_string, typeof(float));
            casted.z = (float) info.GetValue(z_variable_serialization_string, typeof(float));

            return casted;
        }
    }
}