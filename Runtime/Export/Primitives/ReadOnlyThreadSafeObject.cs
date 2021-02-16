public class ReadOnlyThreadSafeObject<T>
{
    public ReadOnlyThreadSafeObject() => _value = default;

    public ReadOnlyThreadSafeObject(T value) => _value = value;

    private readonly T _value;
    private readonly object _lock = new object();

    public T Value
    {
        get
        {
            lock (_lock) return _value;
        }
    }

    public static implicit operator T(ReadOnlyThreadSafeObject<T> target) => target.Value;
    public override string ToString() => Value.ToString();
    public override int GetHashCode() => Value.GetHashCode();
    public override bool Equals(object obj) => obj is ThreadSafeObject<T> threadSafeObject && Value.Equals(threadSafeObject.Value);
}