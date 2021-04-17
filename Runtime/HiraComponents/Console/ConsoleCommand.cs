using System;
using System.Globalization;
using System.Reflection;

namespace HiraEngine.Components.Console.Internal
{
	internal partial class ConsoleCommand
	{
        public ConsoleCommand(MethodInfo method, ParameterInfo[] parameters)
		{
			_method = method.Invoke;
			byte i;
			var count = parameters.Length;
			for (i = 0; i < count; i++)
			{
				switch (i)
				{
					case 0: _arg1 = SUPPORTED_TYPES[parameters[i].ParameterType];
						break;
					case 1: _arg2 = SUPPORTED_TYPES[parameters[i].ParameterType];
						break;
					case 2: _arg3 = SUPPORTED_TYPES[parameters[i].ParameterType];
						break;
					case 3: _arg4 = SUPPORTED_TYPES[parameters[i].ParameterType];
						break;
					case 4: _arg5 = SUPPORTED_TYPES[parameters[i].ParameterType];
						break;
					case 5: _arg6 = SUPPORTED_TYPES[parameters[i].ParameterType];
						break;
					default: throw new ArgumentOutOfRangeException();
				}
			}

			ArgumentCount = i;
		}

		public const byte MAX_SUPPORTED_ARGUMENTS = 5;

		private readonly Func<object, BindingFlags, Binder, object[], CultureInfo, object> _method;
		public readonly byte ArgumentCount;
		private readonly ConsoleCommandArgumentType _arg1;
		private readonly ConsoleCommandArgumentType _arg2;
		private readonly ConsoleCommandArgumentType _arg3;
		private readonly ConsoleCommandArgumentType _arg4;
		private readonly ConsoleCommandArgumentType _arg5;
		private readonly ConsoleCommandArgumentType _arg6;

		public static bool IsTypeSupported(Type type) => SUPPORTED_TYPES.ContainsKey(type);
        
        public ConsoleCommandArgumentType GetArgumentType(byte index) => index switch
			{
				0 => _arg1,
				1 => _arg2,
				2 => _arg3,
				3 => _arg4,
				4 => _arg5,
				5 => _arg6,
				_ => throw new ArgumentOutOfRangeException()
			};

        public void Execute(object[] parameters) => _method.Invoke(null, BindingFlags.Default, null, parameters, null);
	}
}