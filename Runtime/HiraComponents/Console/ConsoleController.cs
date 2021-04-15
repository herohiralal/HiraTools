using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [HiraManager]
#endif
    public class ConsoleController : MonoBehaviour
    {
        private bool _consoleActive = false;
        [SerializeField] private ConsoleGUI gui = null;

        private void Awake()
        {
            gui = gameObject.AddComponent<ConsoleGUI>();
            gui.enabled = false;
        }

        private void OnDestroy()
        {
            gui.enabled = true;
            if (gui != null) Destroy(gui);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) Toggle();
        }

        public void Toggle()
        {
            _consoleActive = !_consoleActive;
            gui.enabled = _consoleActive;
            enabled = !_consoleActive;
        }
    }
}