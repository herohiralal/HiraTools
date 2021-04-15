using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    public class ConsoleGUI : MonoBehaviour
    {
        [SerializeField] private string input = "";

        [SerializeField] private ConsoleController controller = null;
        private readonly List<CommandMetadata> _similarCommands = new List<CommandMetadata>();
        
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
            var y = 5f; // initial padding

            const string controlName = "Console";

            GUI.SetNextControlName(controlName);
            var receivedInput = GUI.TextArea(new Rect(5f, y, Screen.width - 10f, 20f), input);
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
                var infoLabelRect = new Rect(5f, y, Screen.width - 10f, 20f);
	            GUI.Label(infoLabelRect, "Please enter 3+ letters to search correctly.");
	            y += infoLabelRect.height + 5f;
            }

            input = receivedInput;

            var similarCommandsCount = _similarCommands.Count;
            if (similarCommandsCount > 0)
            {
	            var scrollViewPosition = new Rect(5f, y, Screen.width - 10f, (Mathf.Min(similarCommandsCount, 4) * 30f) + 20f);
	            var innerRect = new Rect(0f, 0f, Screen.width - 50f, (30f * similarCommandsCount) + 10f);
                
                // background
                GUI.Box(scrollViewPosition, "");
                
                // scroll
	            _scroll = GUI.BeginScrollView(scrollViewPosition, _scroll, innerRect);

	            for (var i = 0; i < similarCommandsCount; i++)
	            {
		            var commandNameRect = new Rect(6f, 6f + (i * 30f), innerRect.width - 6f, 20f);

                    var currentSimilarCommand = _similarCommands[i];
                    if (GUI.Button(commandNameRect, currentSimilarCommand.DisplayName, GUI.skin.label))
                    {
                        var t = (TextEditor) GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                        t.text = currentSimilarCommand.CommandName;
                        input = currentSimilarCommand.CommandName;
                        t.MoveTextEnd();
                    }
                }

	            GUI.EndScrollView();

	            y += scrollViewPosition.height + 5f;

                var infoLabelRect = new Rect(5f, y, Screen.width - 10f, 20f);
	            GUI.Box(infoLabelRect, "Similar commands");
                // ReSharper disable once RedundantAssignment
	            y += infoLabelRect.height + 5f;
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