using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    [HiraManager, HiraConsole]
    public class ConsoleController : MonoBehaviour
    {
        private bool _consoleActive = false;
        [SerializeField] private ConsoleGUI gui = null;
        [SerializeField] private string[] commands = null;

        private void Awake()
        {
	        commands = ConsoleCommandRegistry.COMMANDS;
            gui = gameObject.AddComponent<ConsoleGUI>();
            gui.enabled = false;
        }

        private void OnDestroy()
        {
            gui.enabled = true;
            if (gui != null) Destroy(gui);
            ConsoleCommandRegistry.Cleanup();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) Toggle();
        }

        public void Toggle()
        {
            _consoleActive = !_consoleActive;
            Time.timeScale = _consoleActive ? 0f : 1f;
            gui.enabled = _consoleActive;
            enabled = !_consoleActive;
        }
    }
}