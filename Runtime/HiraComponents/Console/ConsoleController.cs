using System;
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
        [SerializeField] private ConsoleExecutor executor = null;

        private void OnValidate()
        {
	        executor = GetComponent<ConsoleExecutor>();
	        gui = GetComponent<ConsoleGUI>();
        }

        private void Awake()
        {
	        if (executor == null) executor = gameObject.AddComponent<ConsoleExecutor>();
            if (gui == null) gui = gameObject.AddComponent<ConsoleGUI>();
            gui.OnClose += OnGUIClose;
            gui.enabled = false;
        }

        private void OnDestroy()
        {
            gui.enabled = true;
            gui.OnClose -= OnGUIClose;
            if (gui != null) Destroy(gui);
            if (executor != null) Destroy(executor);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) Toggle();
        }

        private void OnGUIClose() => SetConsoleGUIVisibility(false);

        private void Toggle() => SetConsoleGUIVisibility(!_consoleActive);

        private void SetConsoleGUIVisibility(bool value)
        {
	        _consoleActive = value;
	        gui.enabled = value;
	        enabled = !value;
        }
    }
}