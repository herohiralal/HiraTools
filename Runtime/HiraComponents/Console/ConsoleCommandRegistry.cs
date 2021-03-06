﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HiraEngine.Components.Console.Internal
{
    internal static class ConsoleCommandRegistry
	{
		private static readonly Dictionary<string, ConsoleCommand> database;
		private static readonly CommandMetadata[] commands;

		public static IReadOnlyDictionary<string, ConsoleCommand> Database => database;

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
					
					commandList.Add(new CommandMetadata(commandName, consoleMethod.GetArgumentMetadata()));
					db.Add(commandName, command);
				}
			}

			commands = commandList.ToArray();
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

        private static string GetArgumentMetadata(this MethodInfo methodInfo)
        {
            var sb = new System.Text.StringBuilder(100);
            var parameters = methodInfo.GetParameters();
            for (byte i = 0; i < parameters.Length; i++)
            {
                sb.Append($" [{ConsoleCommand.SUPPORTED_TYPES[parameters[i].ParameterType]} {parameters[i].Name}]");
            }

            return sb.ToString();
        }

		public static void GetSimilarCommands(string match, List<CommandMetadata> outputBuffer)
		{
			if (string.IsNullOrWhiteSpace(match))
			{
				foreach (var cm in commands) outputBuffer.Add(cm);
				return;
			}
			
			foreach (var cm in commands)
			{
				if (cm.CommandName.Contains(match))
					outputBuffer.Add(cm);
			}
		}
	}
}