using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Assertions;

namespace UnityEngine
{
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
    public interface IHiraCollectionEditorInterface
    {
        public Object[][] CollectionInternal { get; }
    }
#endif

    public abstract class HiraCollection : ScriptableObject
    {
    }

    public abstract class HiraCollection<T> : HiraCollection, ISerializationCallbackReceiver
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        , IDirtiable
        , IHiraCollectionEditorInterface
#endif
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        public bool IsDirty { get; set; }
        public virtual Object[][] CollectionInternal => new[]{collection1};
#endif
        [SerializeField] protected Object[] collection1 = { };
        private T[] _collection1Actual = { };

        protected void OnValidate() => UpdateMainCollection();

        public void OnBeforeSerialize()
        {
            // not needed
        }

        public void OnAfterDeserialize() => UpdateMainCollection();

        protected virtual void UpdateMainCollection()
        {
            _collection1Actual = Consume<T>(collection1);
        }

        public T[] Collection1
        {
            get => _collection1Actual;
            set
            {
                _collection1Actual = value;
                collection1 = Consume(value);
            }
        }

        [Conditional("UNITY_ASSERTIONS")]
        private static void AssertAllElementsAreObjects<TInput>(IEnumerable<TInput> input) => 
            Assert.IsTrue(input.All(o=>o is Object));

        [Conditional("UNITY_ASSERTIONS")]
        private static void AssertAllElementsAre<TInput>(IEnumerable<Object> input) =>
            Assert.IsTrue(input.All(o => o is TInput));

        protected static Object[] Consume<TInput>(TInput[] input)
        {
            AssertAllElementsAreObjects(input);
            
            var length = input.Length;
            var output = new Object[length];
            for (var i = 0; i < length; i++)
            {
                output[i] = input[i] as Object;
            }

            return output;
        }

        protected static TOutput[] Consume<TOutput>(Object[] input)
        {
            AssertAllElementsAre<TOutput>(input);

            var length = input.Length;
            var output = new TOutput[length];
            for (var i = 0; i < length; i++)
            {
                if (input[i] is TOutput outputType)
                    output[i] = outputType;
            }

            return output;
        }
    }

    public abstract class HiraCollection<T1, T2> : HiraCollection<T1>
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        public override Object[][] CollectionInternal => new[] {collection1, collection2};
#endif
        [SerializeField] protected Object[] collection2 = { };
        private T2[] _collection2Actual = { };

        protected override void UpdateMainCollection()
        {
            base.UpdateMainCollection();
            _collection2Actual = Consume<T2>(collection2);
        }

        public T2[] Collection2
        {
            get => _collection2Actual;
            set
            {
                _collection2Actual = value;
                collection2 = Consume(value);
            }
        }
    }

    public abstract class HiraCollection<T1, T2, T3> : HiraCollection<T1, T2>
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        public override Object[][] CollectionInternal => new[] {collection1, collection2, collection3};
#endif
        [SerializeField] protected Object[] collection3 = { };
        private T3[] _collection3Actual = { };

        protected override void UpdateMainCollection()
        {
            base.UpdateMainCollection();
            _collection3Actual = Consume<T3>(collection3);
        }
        
        public T3[] Collection3
        {
            get => _collection3Actual;
            set
            {
                _collection3Actual = value;
                collection3 = Consume(value);
            }
        }
    }

    public abstract class HiraCollection<T1, T2, T3, T4> : HiraCollection<T1, T2, T3>
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        public override Object[][] CollectionInternal => new[] {collection1, collection2, collection3, collection4};
#endif
        [SerializeField] protected Object[] collection4 = { };
        private T4[] _collection4Actual = { };

        protected override void UpdateMainCollection()
        {
            base.UpdateMainCollection();
            _collection4Actual = Consume<T4>(collection4);
        }
        
        public T4[] Collection4
        {
            get => _collection4Actual;
            set
            {
                _collection4Actual = value;
                collection4 = Consume(value);
            }
        }
    }

    public abstract class HiraCollection<T1, T2, T3, T4, T5> : HiraCollection<T1, T2, T3, T4>
    {
#if UNITY_EDITOR && !STRIP_EDITOR_CODE
        public override Object[][] CollectionInternal => new[] {collection1, collection2, collection3, collection4, collection5};
#endif
        [SerializeField] protected Object[] collection5 = { };
        private T5[] _collection5Actual = { };

        protected override void UpdateMainCollection()
        {
            base.UpdateMainCollection();
            _collection5Actual = Consume<T5>(collection5);
        }
        
        public T5[] Collection5
        {
            get => _collection5Actual;
            set
            {
                _collection5Actual = value;
                collection5 = Consume(value);
            }
        }
    }
}