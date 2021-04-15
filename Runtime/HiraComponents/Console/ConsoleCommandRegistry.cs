using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    internal static class ConsoleCommandRegistry
	{
		private static readonly Dictionary<string, ConsoleCommand> database;
		public static readonly CommandMetadata[] COMMANDS;

		static ConsoleCommandRegistry()
		{
			var db = new Dictionary<string, ConsoleCommand>();
			var commandList = new List<CommandMetadata>();

			var consoleTypes = typeof(HiraConsoleTypeAttribute)
				.GetTypesWithThisAttribute(true);

			foreach (var consoleType in consoleTypes)
            {
                var consoleAttributeOnType = consoleType.GetCustomAttribute<HiraConsoleTypeAttribute>();
                var consoleMethods = consoleType
                    .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                var consoleTypeName = consoleAttributeOnType.Name ?? consoleType.Name;
                if (consoleAttributeOnType.AddDot) consoleTypeName += '.';

				foreach (var consoleMethod in consoleMethods)
                {
                    // ignore any method that does not have HiraConsoleAttribute attribute
                    var consoleAttributeOnMethod = consoleMethod.GetCustomAttribute<HiraConsoleCallableAttribute>();
                    if (consoleAttributeOnMethod == null) continue;

                    var methodTypeName = string.IsNullOrWhiteSpace(consoleAttributeOnMethod.Name) ? consoleMethod.Name : consoleAttributeOnMethod.Name;

					var commandName = $"{consoleTypeName}{methodTypeName}".ToLower();

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
					
					commandList.Add(new CommandMetadata(commandName, command.ArgumentMetadata));
					db.Add(commandName, command);
				}
			}

			COMMANDS = commandList.ToArray();
			database = db;
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

		public static void TryInvoke(string commandline)
		{
            ParsingUtility.ParseCommandline(commandline, out var command, out var args);
            
            if (!database.ContainsKey(command))
	            throw new InvalidOperationException($"Could not recognize the command {command}.");
            
            database[command].TryInvoke(args);
		}

		public static void GetSimilarCommands(string match, List<CommandMetadata> outputBuffer)
		{
			if (string.IsNullOrWhiteSpace(match))
			{
				foreach (var c in COMMANDS) outputBuffer.Add(c);
				return;
			}
			
			foreach (var s in COMMANDS)
			{
				if (s.CommandName.Contains(match))
					outputBuffer.Add(s);
			}
		}
	}
}