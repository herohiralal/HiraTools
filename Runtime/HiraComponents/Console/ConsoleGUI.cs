using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    public class ConsoleGUI : MonoBehaviour
    {
        [SerializeField] private string input = "";

        [SerializeField] private ConsoleController controller = null;
        private readonly List<string> _similarCommands = new List<string>();
        
        private bool _needToFocus = false;
        private Vector2 _scroll = Vector2.zero;

        private void Awake()
        {
            controller = GetComponent<ConsoleController>();
        }

        private void OnDestroy()
        {
            controller = null;
        }

        private void OnEnable()
        {
	        _similarCommands.Clear();
            input = "";
            _needToFocus = true;
        }

        private void OnGUI()
        {
	        // ReSharper disable once RedundantAssignment
            var y = 5f; // initial padding

            const string controlName = "Console";

            GUI.SetNextControlName(controlName);
            var receivedInput = GUI.TextArea(new Rect(10f, y, Screen.width - 20f, 20f), input);
            if (_needToFocus)
            {
	            GUI.FocusControl(controlName);
	            _needToFocus = false;
            }
            y += 20f + 5f; // commandline height + padding

            if (receivedInput.Length >= 3)
            {
	            if (receivedInput != input)
	            {
		            _similarCommands.Clear();
		            ConsoleCommandRegistry.GetSimilarCommands(receivedInput, _similarCommands);
	            }
            }
            else
            {
	            _similarCommands.Clear();
	            GUI.Label(new Rect(5f, y, Screen.width - 10f, 20f), "Please enter 3+ letters to search correctly.");
	            y += 20f + 5f;
            }

            input = receivedInput;

            if (_similarCommands.Count > 0)
            {
	            var scrollViewPosition = new Rect(5f, y, Screen.width - 10f, 100f);
	            var innerRect = new Rect(0f, 0f, Screen.width - 50f, (20f * _similarCommands.Count) + 10f);
	            _scroll = GUI.BeginScrollView(scrollViewPosition, _scroll, innerRect);

	            for (var i = 0; i < _similarCommands.Count; i++)
	            {
		            var infoRect = new Rect(5f, 5f + (i * 20f), innerRect.width, 20f);
		            GUI.Label(infoRect, _similarCommands[i]);
	            }

	            GUI.EndScrollView();

	            y += 100f + 5f;

	            GUI.Label(new Rect(5f, y, Screen.width - 10f, 20f), "Similar commands");
	            y += 20f + 5f;
            }

            if (input.Contains("`"))
            {
                controller.Toggle();
                GUI.FocusControl(null);
            }
            else if (input.Contains("\n"))
            {
	            var currentInput = input.Replace("\n", "");
	            input = "";
	            if (!ConsoleCommandRegistry.TryInvoke(currentInput))
		            throw new InvalidOperationException("Command unrecognized.");
            }
        }
    }
}