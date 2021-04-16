using System;
using System.Collections.Generic;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
	public class ConsoleExecutor : MonoBehaviour
	{
		[SerializeField] public string commandLine = "";
		[SerializeField] private string command = "";
		[SerializeField] private List<string> arguments = new List<string>();

		public void Execute(string inputCommandLine)
		{
			commandLine = inputCommandLine;
			
			ParseInput();
            
			var executableCommand = ExecutableCommand;
			
			executableCommand.Method.Invoke(null, GetArguments(executableCommand));
		}

		private void ParseInput()
		{
			arguments.Clear();
			ParsingUtility.ParseCommandline(commandLine, out command, arguments);
		}

		private ConsoleCommand ExecutableCommand
		{
			get
			{
				if (!ConsoleCommandRegistry.Database.ContainsKey(command))
					throw new InvalidOperationException($"Could not recognize the command {command}.");
				
				return ConsoleCommandRegistry.Database[command];
			}
		}

		private object[] GetArguments(ConsoleCommand executableCommand)
		{
			var parameters = new object[executableCommand.ArgumentCount];

			if (arguments.Count < executableCommand.ArgumentCount)
				throw new InvalidOperationException("Not enough number of arguments.");
			
			for (byte i = 0; i < executableCommand.ArgumentCount; i++)
			{
				if (!ParsingUtility.TryParse(arguments[i], executableCommand.GetArgumentType(i), out var parsedParameter))
					throw new InvalidCastException($"Could not correctly parse the argument {arguments[i]} as {executableCommand.GetArgumentType(i)}.");

				parameters[i] = parsedParameter;
			}

			return parameters;
		}
	}
}