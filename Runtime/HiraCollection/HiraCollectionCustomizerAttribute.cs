using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HiraCollectionCustomizerAttribute : Attribute
    {
        public HiraCollectionCustomizerAttribute(byte collectionID) => CollectionID = collectionID;
        public readonly byte CollectionID;
        public string Title { get; set; } = "Contents";
        public Type[] RequiredAttributes { get; set; } = { };
        public int MaxObjectCount { get; set; } = int.MaxValue;
        public int MinObjectCount { get; set; } = 0;
        public static readonly HiraCollectionCustomizerAttribute DEFAULT = new HiraCollectionCustomizerAttribute(255);
    }
}