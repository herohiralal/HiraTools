namespace UnityEngine
{
	public interface IContainsComponent<out T>
	{
		T Component { get; }
	}

	public class HiraComponentContainer : MonoBehaviour
	{
	}
}