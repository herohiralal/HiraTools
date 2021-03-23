[System.Serializable]
public struct FlipFlopPool<T>
{
	public FlipFlopPool(bool state, T a, T b)
	{
		this.state = state;
		this.a = a;
		this.b = b;
	}
	
	[UnityEngine.SerializeField] private bool state;
	[UnityEngine.SerializeField] private T a;
	[UnityEngine.SerializeField] private T b;

	public T First
	{
		get => state ? a : b;
		set
		{
			if (state) a = value;
			else b = value;
		}
	}

	public T Second
	{
		get => state ? b : a;
		set
		{
			if (state) b = value;
			else a = value;
		}
	}

	public void Flip() => state = !state;
}