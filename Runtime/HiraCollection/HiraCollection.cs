using System.Linq;
using UnityEngine.Assertions;

namespace UnityEngine
{
    public interface ICollectionAwareTarget<T>
    {
        HiraCollection<T> ParentCollection { set; }
        int Index { set; }
    }

    public abstract class HiraCollection<T> : ScriptableObject
#if UNITY_EDITOR
        , IDirtiable
#endif
    {
#if UNITY_EDITOR
        public bool IsDirty { get; set; }
#endif
        [SerializeField] protected ScriptableObject[] collection1 = { };

        protected virtual void OnValidate()
        {
            foreach (var scriptableObject in collection1) 
                Assert.IsTrue(scriptableObject is T);
        }

        public void Setup(T[] inCollection)
        {
            Assert.IsNotNull(inCollection);
            Assert.IsTrue(inCollection.All(t=>t is ScriptableObject));
            collection1 = inCollection as ScriptableObject[];
            Assert.IsNotNull(collection1);
        }
    }

    public abstract class HiraCollection<T1, T2> : HiraCollection<T1>
    {
        [SerializeField] protected ScriptableObject[] collection2 = { };

        protected override void OnValidate()
        {
            base.OnValidate();
            foreach (var scriptableObject in collection2)
                Assert.IsTrue(scriptableObject is T2);
        }

        public void Setup(T1[] inCollection1, T2[] inCollection2)
        {
            Setup(inCollection1);
            
            Assert.IsNotNull(inCollection2);
            Assert.IsTrue(inCollection2.All(t=>t is ScriptableObject));
            collection2 = inCollection2 as ScriptableObject[];
            Assert.IsNotNull(collection2);
        }
    }
}