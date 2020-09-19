/* * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * *  
 * *  File: _SerializationSurrogateAbstract.cs
 * *  Type: Visual C# Source File
 * *  
 * *  Author: Rohan Jadav
 * *  
 * *  Description: Abstract class for serialization surrogates.
 * *               The system then finds all the non-abstract child classes,
 * *               creates an instance of them and then adds them to the
 * *               surrogate selector.
 * *  
 * * * * * * * * * * * * * * * * * * * * * * * * *
 * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Runtime.Serialization;

namespace HiraEngine.HiraSaves.Serialization.Surrogates
{
    // ReSharper disable once InconsistentNaming
    public abstract class _SerializationSurrogateAbstract : ISerializationSurrogate
    {
        public abstract void GetObjectData(object obj, SerializationInfo info, StreamingContext context);

        public abstract object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
            ISurrogateSelector selector);

        internal abstract Type IsSurrogateFor { get; } 
    }
    
    // ReSharper disable once InconsistentNaming
    public abstract class _SerializationSurrogateAbstract<T> : _SerializationSurrogateAbstract
    {
        public sealed override void GetObjectData(object obj, SerializationInfo info, StreamingContext context) => 
            SaveObject((T)obj, info);

        public sealed override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) => 
            LoadObject((T)obj, info);

        internal sealed override Type IsSurrogateFor => typeof(T);
        protected abstract void SaveObject(T casted, SerializationInfo info);
        protected abstract T LoadObject(T casted, SerializationInfo info);
    }
}