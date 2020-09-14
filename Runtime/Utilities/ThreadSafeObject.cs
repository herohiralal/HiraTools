namespace Hiralal.Utilities
{
    public class ThreadSafeObject<T>
    {
        public ThreadSafeObject() => _value = default;

        public ThreadSafeObject(T value) => _value = value;

        private T _value;
        private readonly object _lock = new object();

        public T Value
        {
            get
            {
                lock (_lock) return _value;
            }
            set
            {
                lock (_lock) _value = value;
            }
        }

        public static implicit operator T(ThreadSafeObject<T> target) => target.Value;
        public override string ToString() => Value.ToString();
    }
}