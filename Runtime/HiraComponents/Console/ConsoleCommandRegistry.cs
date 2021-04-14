using System;
using System.Collections.Generic;
using System.Linq;
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
				var consoleMethods = consoleType
					.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(mi => mi.GetCustomAttribute<HiraConsoleAttribute>() != null);

				foreach (var method in consoleMethods)
				{
					var commandName = $"{consoleType.Name}.{method.Name}".ToLower();

					if (db.ContainsKey(commandName))
					{
						Debug.LogError($"Duplicate command found: {commandName}.");
						continue;
					}

					if (!method.TryConvertToCommand(out var command))
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