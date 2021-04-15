using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
	internal static class ConsoleCommandRegistry
	{
		private static readonly Dictionary<string, ConsoleCommand> database;
		public static readonly string[] COMMANDS;

		static ConsoleCommandRegistry()
		{
			var db = new Dictionary<string, ConsoleCommand>();
			var commandList = new List<string>();

			var consoleTypes = typeof(HiraConsoleAttribute)
				.GetTypesWithThisAttribute(true);

			foreach (var consoleType in consoleTypes)
            {
                var consoleAttributeOnType = consoleType.GetCustomAttribute<HiraConsoleAttribute>();
                var consoleMethods = consoleType
                    .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                var consoleTypeName = string.IsNullOrWhiteSpace(consoleAttributeOnType.Name) ? consoleType.Name : consoleAttributeOnType.Name;

				foreach (var consoleMethod in consoleMethods)
                {
                    // ignore any method that does not have HiraConsoleAttribute attribute
                    var consoleAttributeOnMethod = consoleMethod.GetCustomAttribute<HiraConsoleAttribute>();
                    if (consoleAttributeOnMethod == null) continue;

                    var methodTypeName = string.IsNullOrWhiteSpace(consoleAttributeOnMethod.Name) ? consoleMethod.Name : consoleAttributeOnMethod.Name;

					var commandName = $"{consoleTypeName}.{methodTypeName}".ToLower();

					if (db.ContainsKey(commandName))
					{
						Debug.LogError($"Duplicate command found: {commandName}.");
						continue;
					}

					if (!consoleMethod.TryConvertToCommand(out var command))
					{
						Debug.LogError($"Unsupported command detected: {commandName}.");
						continue;
					}
					
					commandList.Add(commandName);
					db.Add(commandName, command);
				}
			}

			COMMANDS = commandList.ToArray();
			database = db;
		}

		public static void Cleanup()
		{
			database.Clear();
		}

		private static bool TryConvertToCommand(this MethodInfo mi, out ConsoleCommand command)
		{
			var parameters = mi.GetParameters();
			var parametersCount = parameters.Length;
			
			if (parametersCount > ConsoleCommand.MAX_SUPPORTED_ARGUMENTS)
			{
				command = null;
				return false;
			}

			for (var i = 0; i < parametersCount; i++)
			{
				if (ConsoleCommand.IsTypeSupported(parameters[i].ParameterType)) continue;
				command = null;
				return false;
			}

			command = new ConsoleCommand(mi, parameters);
			return true;
		}

		public static bool TryInvoke(string command)
		{
			var parsedCommand = command.Split(new[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
			command = parsedCommand[0].ToLower();
			var args = parsedCommand.Length > 1 ? parsedCommand[1] : "";

			return database.ContainsKey(command) && database[command].TryInvoke(args);
		}

		public static void GetSimilarCommands(string match, List<string> outputBuffer)
		{
			foreach (var s in COMMANDS)
			{
				if (s.Contains(match))
					outputBuffer.Add(s);
			}
		}
	}
}